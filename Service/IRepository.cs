using MassTransitStudies.Messages;

namespace MassTransitStudies.Service
{
    public interface IRepository
    {
        void Add(ValueEntered val);
        ReceivedMessages List();
    }
}
