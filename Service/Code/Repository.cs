using System;
using System.Collections.Generic;
using MassTransitStudies.Messages;

namespace MassTransitStudies.Service
{
    public class Repository
    {
        List<IValueEntered> messages = new List<IValueEntered>();
        public Repository()
        {
        }
        public void Add(IValueEntered val) { messages.Add(val); }
        public ReceivedMessages List() 
        {
            return new ReceivedMessages(messages);
        }
    }
}

