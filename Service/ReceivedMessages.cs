using System.Collections.Generic;
using System.Linq;
using MassTransitStudies.Messages;

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
