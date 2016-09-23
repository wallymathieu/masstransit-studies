using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Pipeline;
using MassTransitStudies.Messages;
using Service;

namespace MassTransitStudies.Service
{
    public class ValueEnteredConsumer:IConsumer<ValueEntered>
    {
        public Task Consume(ConsumeContext<ValueEntered> context)
        {
            Global.repository.Add(context.Message);
            return Task.FromResult("");
        }
    }
    public class DelayedMessageConsumer:IConsumer<DelayedMessage>
    {
        public Task Consume(ConsumeContext<DelayedMessage> context)
        {
            Global.repository.Add(context.Message);
            return Task.FromResult("");
        }            
    }
}

