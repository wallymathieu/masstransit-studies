using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace MassTransitStudies.Service
{
    class SwaggerConfig
    {
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment env;

        public SwaggerConfig(Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            this.env = env;
        }

        ///
        public void Configure(IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger(c => { c.RouteTemplate = "swagger/{documentName}/swagger.json"; });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
                c.EnableDeepLinking();
            });
        }

        ///
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(options => { });

            services.ConfigureSwaggerGen(options =>
            {
                var webAssembly = typeof(Startup).GetTypeInfo().Assembly;
                var informationalVersion =
                    (webAssembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute))
                        as AssemblyInformationalVersionAttribute[])?.First()?.InformationalVersion;

                options.SwaggerDoc("v1", new Info
                {
                    Version = informationalVersion ?? "dev",
                    Title = "API",
                    Description = "Some API",
                    TermsOfService = "See license agreement",
                    Contact = new Contact
                    { Name = "Dev", Email = "developers@somecompany.com", Url = "https://somecompany.com" }
                });

                    //Set the comments path for the swagger json and ui.
                    var xmlPath = typeof(Startup).Assembly.Location.Replace(".dll", ".xml").Replace(".exe", ".xml");
                if (File.Exists(xmlPath))
                    options.IncludeXmlComments(xmlPath);
            });
        }
    }
}
