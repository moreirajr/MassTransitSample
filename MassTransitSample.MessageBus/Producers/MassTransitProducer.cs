using MassTransit;
using MassTransitSample.MessageBus.Messages;
using System.Threading.Tasks;

namespace MassTransitSample.MessageBus.Producers
{
    public class MassTransitProducer : IMessageBusProducer
    {
        private readonly IBusControl _busControl;

        public MassTransitProducer(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public async Task PublishAsync<T>(T message) where T : IMessage
        {
            await _busControl.Publish(message);
        }
    }
}