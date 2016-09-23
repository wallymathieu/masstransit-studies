using System;
using System.Collections.Generic;
using MassTransitStudies.Messages;

namespace MassTransitStudies.Service
{
    public class Repository
    {
        List<ValueEntered> messages = new List<ValueEntered>();
        List<DelayedMessage> delayedMessages = new List<DelayedMessage>();
        public Repository()
        {
        }
        public void Add(ValueEntered val) { messages.Add(val); }
        public void Add(DelayedMessage val) { delayedMessages.Add(val); }
        public ReceivedMessages List() 
        {
            return new ReceivedMessages(messages, delayedMessages);
        }
    }
}

