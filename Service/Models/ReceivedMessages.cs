using System;
using System.Collections.Generic;
using MassTransitStudies.Messages;
using System.Linq;
namespace MassTransitStudies.Service
{
    public class ReceivedMessages
    {
        public readonly ValueEntered[] Values;
        public ReceivedMessages(IEnumerable<ValueEntered> values)
        {
            Values = values.ToArray(); 
        }
    }
}
