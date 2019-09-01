using System;

using System.Threading.Tasks;
using MassTransit;
using MassTransit.Definition;
using MassTransit.ExtensionsLoggingIntegration;
using MassTransit.Logging;
using MassTransit.Logging.Tracing;
using MassTransitStudies.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
namespace MassTransitStudies.Generator
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; private set; }

        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder();
                    config.AddJsonFile("appsettings.json", true);
                    config.AddEnvironmentVariables();

                    if (args != null)
                        config.AddCommandLine(args);
            Configuration = config.Build();
            var services = new ServiceCollection();
    
            services.AddLogging();
            services.Configure<AppConfig>(Configuration.GetSection("AppConfig"));

            services.AddMassTransit(cfg =>
            {
                cfg.AddBus(ConfigureBus);
            });

            var svcProvider = services.BuildServiceProvider();
            var logging=svcProvider.GetRequiredService<ILoggingBuilder>();
            logging.AddConfiguration(Configuration.GetSection("Logging"));
            logging.AddConsole();
            var _bus = svcProvider.GetRequiredService<IBusControl>();
            if (Logger.Current.GetType() == typeof(TraceLogger))
                ExtensionsLogger.Use(svcProvider.GetRequiredService<ILoggerFactory>());
            var _logger = svcProvider
                            .GetRequiredService<ILoggerFactory>()
                            .CreateLogger<Program>();
            await _bus.StartAsync().ConfigureAwait(false);
            Console.WriteLine("Enter message (or ctrl+c to exit)");
            try
            {
                while (true) // hackish
                {
                    Console.Write("> ");
                    var value = Console.ReadLine();
                    if (!string.IsNullOrEmpty(value))
                    {
                        await _bus.Publish(ValueEnteredFactory.Create(value))
                                .ConfigureAwait(false);
                        Console.WriteLine($"published {value}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"'while' exception");
            }
            finally
            {
                _bus.Stop();
            }
        }

        static IBusControl ConfigureBus(IServiceProvider provider)
        {
            var options = provider.GetRequiredService<IOptions<AppConfig>>().Value;

            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(options.Host, options.VirtualHost, h =>
                {
                    h.Username(options.Username);
                    h.Password(options.Password);
                });

                cfg.UseInMemoryScheduler();

                cfg.ConfigureEndpoints(provider, new KebabCaseEndpointNameFormatter());
            });
        }
    }
}