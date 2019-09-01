using System.Collections.Generic;
using MassTransitStudies.Messages;

namespace MassTransitStudies.Service
{

    public class Repository : IRepository
    {
        List<ValueEntered> messages = new List<ValueEntered>();
        public Repository()
        {
        }
        public void Add(ValueEntered val) { messages.Add(val); }
        public ReceivedMessages List()
        {
            return new ReceivedMessages(messages);
        }
    }
}
