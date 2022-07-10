using MassTransitSample.MessageBus.Messages;
using System.Threading.Tasks;

namespace MassTransitSample.MessageBus.Producers
{
    public interface IMessageBusProducer
    {
        Task PublishAsync<T>(T message) where T : IMessage;
    }
}