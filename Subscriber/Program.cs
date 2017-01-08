using MassTransit;
using MassTransitStudies.Messages;
using System;
using System.Threading.Tasks;

namespace MassTransitStudies.Generator
{
    class ValueEnteredConsumer : IConsumer<IValueEntered>
    {
        public Task Consume(ConsumeContext<IValueEntered> c)
        {
            Console.WriteLine($"Value entered { c.Message.Value}");
            return Task.FromResult(0);
        }
    }

    class Program
    {
        public static IBusControl ConfigureBus()
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                cfg.ReceiveEndpoint(host, "subscriber", e => e.Consumer<ValueEnteredConsumer>());
            });
        }

        static void Main(string[] args)
        {
            var busControl = ConfigureBus();
            busControl.Start();
            Console.WriteLine("Press enter to finish");
            try
            {
                Console.ReadLine();
            }
            finally
            {
                busControl.Stop();
            }
        }

    }
}
