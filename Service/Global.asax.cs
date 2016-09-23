using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using MassTransit;
using System;
using MassTransitStudies.Service;

namespace Service
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            repository = new Repository();
            _busControl = ConfigureBus();
            _busControl.Start();

        }
        static IBusControl _busControl;
        public static Repository repository;

        public static IBus Bus
        {
            get { return _busControl; }
        }


        protected void Application_End()
        {
            _busControl.Stop();
        }

        IBusControl ConfigureBus()
        {
            return MassTransit.Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                cfg.UseDelayedExchangeMessageScheduler();
                cfg.ReceiveEndpoint(host, "customer_update_queue", e =>
                 {
                     e.Consumer<ValueEnteredConsumer>();
                     e.Consumer<DelayedMessageConsumer>();
                 });
            });
        }
    }
}
