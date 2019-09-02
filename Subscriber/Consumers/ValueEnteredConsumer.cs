using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Definition;
using MassTransitStudies.Messages;
using Microsoft.Extensions.Logging;

namespace MassTransitStudies.Subscriber.Consumers
{
    public class ValueEnteredConsumer : IConsumer<ValueEntered>
    {
        private readonly ILogger<ValueEnteredConsumer> _logger;

        public ValueEnteredConsumer(ILogger<ValueEnteredConsumer> logger)
        {
            _logger=logger;
            logger.LogDebug($"Created ValueEnteredConsumer");
        }
        public Task Consume(ConsumeContext<ValueEntered> c)
        {
            _logger.LogDebug($"Value entered { c.Message.Value}");
            return Task.FromResult(0);
        }
    }
    public class ValueEnteredConsumerDefinition :
        ConsumerDefinition<ValueEnteredConsumer>
    {
        public ValueEnteredConsumerDefinition()
        {
            EndpointName = "subscriber";
            ConcurrentMessageLimit = 10;
        }
    }
}