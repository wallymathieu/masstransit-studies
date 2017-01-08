using System;
using System.Collections.Generic;
using MassTransitStudies.Messages;
using System.Linq;
namespace MassTransitStudies.Service
{
    public class ReceivedMessages
    {
        public readonly IValueEntered[] Values;
        public ReceivedMessages(IEnumerable<IValueEntered> values)
        {
            Values = values.ToArray(); 
        }
    }
}
