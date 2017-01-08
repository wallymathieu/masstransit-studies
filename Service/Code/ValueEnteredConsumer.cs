using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Pipeline;
using MassTransitStudies.Messages;
using Service;

namespace MassTransitStudies.Service
{
    public class ValueEnteredConsumer:IConsumer<IValueEntered>
    {
        public Task Consume(ConsumeContext<IValueEntered> context)
        {
            Global.repository.Add(context.Message);
            return Task.FromResult("");
        }
    }
}

