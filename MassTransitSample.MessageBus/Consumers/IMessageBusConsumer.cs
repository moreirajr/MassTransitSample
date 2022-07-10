using MassTransit;
using MassTransitSample.MessageBus.Messages;
using System.Threading;
using System.Threading.Tasks;

namespace MassTransitSample.MessageBus.Consumers
{
    public interface IMessageBusConsumer<in TMessage> : IConsumer<TMessage>
        where TMessage : class, IMessage
    {
        Task ConsumeAsync(ConsumeContext<TMessage> consumerContext, CancellationToken cancellationToken);
    }
}