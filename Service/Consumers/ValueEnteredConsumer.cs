using System.Threading.Tasks;
using MassTransit;
using MassTransit.Definition;
using MassTransitStudies.Messages;
using Microsoft.Extensions.Logging;

namespace MassTransitStudies.Service.Consumers
{
    public class ValueEnteredConsumer:IConsumer<ValueEntered>
    {
        private readonly IRepository repository;
        private readonly ILogger<ValueEnteredConsumer> _logger;

        public ValueEnteredConsumer(ILogger<ValueEnteredConsumer> logger, IRepository repository)
        {
            _logger=logger;
            logger.LogDebug($"Created ValueEnteredConsumer");
            this.repository = repository;
        }
        public Task Consume(ConsumeContext<ValueEntered> context)
        {
            repository.Add(context.Message);
            _logger.LogDebug($"Value entered { context.Message.Value}");
            return Task.FromResult("");
        }
    }
    public class ValueEnteredConsumerDefinition :
        ConsumerDefinition<ValueEnteredConsumer>
    {
        public ValueEnteredConsumerDefinition()
        {
            EndpointName = "service";
            ConcurrentMessageLimit = 10;
        }
    }
}
