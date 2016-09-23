using System;
using System.Collections.Generic;
using MassTransitStudies.Messages;
using System.Linq;
namespace MassTransitStudies.Service
{
    public class ReceivedMessages
    {
        public readonly ValueEntered[] Values;

        public readonly DelayedMessage[] Delayed;

        public ReceivedMessages(IEnumerable<ValueEntered> values, IEnumerable<DelayedMessage> delayed)
        {
            Values = values.ToArray();
            Delayed = delayed.ToArray();
        }
    }
}
