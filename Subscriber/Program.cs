using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Definition;
using MassTransit.Saga;
using MassTransitStudies.Subscriber.Consumers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MassTransitStudies.Subscriber
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", true);
                    config.AddEnvironmentVariables();

                    if (args != null)
                        config.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging();
                    services.Configure<AppConfig>(hostContext.Configuration.GetSection("AppConfig"));
                    services.AddSingleton<ValueEnteredConsumer>();

                    services.AddMassTransit(cfg =>
                    {
                        cfg.AddConsumersFromNamespaceContaining<ValueEnteredConsumer>();
                        cfg.AddBus(ConfigureBus);
                    });

                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                });

            await builder.RunConsoleAsync().ConfigureAwait(false);
        }

        static IBusControl ConfigureBus(IServiceProvider provider)
        {
            var options = provider.GetRequiredService<IOptions<AppConfig>>().Value;

            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(options.Host, options.VirtualHost, h =>
                {
                    h.Username(options.Username);
                    h.Password(options.Password);
                });

                cfg.UseInMemoryScheduler();

                //cfg.ConfigureEndpoints(provider, new KebabCaseEndpointNameFormatter());
            });
        }
    }
}
