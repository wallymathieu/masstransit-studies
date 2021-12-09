using System;
using MassTransit;
using MassTransit.Definition;
using MassTransitStudies.Service.Consumers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace MassTransitStudies.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _swagger = new SwaggerConfig(env);
        }

        public IConfiguration Configuration { get; }

        private readonly SwaggerConfig _swagger;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            _swagger.ConfigureServices(services);
            services.AddSingleton<IRepository,Repository>(); // in memory only ...
            services.AddSingleton<ValueEnteredConsumer>();
                
            services.Configure<AppConfig>(Configuration.GetSection("AppConfig"));
            services.AddMassTransit(cfg =>
            {
                cfg.AddConsumersFromNamespaceContaining<ValueEnteredConsumer>();
                cfg.AddBus(ConfigureBus);
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }
            _swagger.Configure(app);
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
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
                //cfg.ConfigureEndpoints(provider, new KebabCaseEndpointNameFormatter());
                //cfg.ReceiveEndpoint(host, "service", e => e.Consumer<ValueEnteredConsumer>(e.));
            });
        }
    }
}
