using MassTransit;
using MassTransitStudies.Messages;
using System;

namespace MassTransitStudies.Generator
{
    class Program
    {
        public static IBusControl ConfigureBus()
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
                        cfg.Host(new Uri("rabbitmq://localhost"), h =>
                        {
                            h.Username("guest");
                            h.Password("guest");
                        }));
        }

        static void Main(string[] args)
        {
            var busControl = ConfigureBus();
            var run = true;
            busControl.Start();
            Console.WriteLine("Enter message (or ctrl+c to exit)");
            try
            {
                while (run)
                {
                    Console.Write("> ");
                    var value = Console.ReadLine();
                    if (!string.IsNullOrEmpty(value))
                    {
                        busControl.Publish(ValueEntered.Create(value));
                        Console.WriteLine($"published {value}");
                    }
                }
            }
            finally
            {
                busControl.Stop();
            }
        }

    }
}
