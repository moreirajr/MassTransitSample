using MassTransit;
using MassTransitSample.MessageBus.Consumers;
using MassTransitSample.MessageBusSample.Messages;
using System.Threading;
using System.Threading.Tasks;

namespace MassTransitSample.MessageBusSample.Consumers
{
    public class CriarProdutoConsumer : IMessageBusConsumer<CriarProdutoMessage>
    {
        public async Task Consume(ConsumeContext<CriarProdutoMessage> context)
        {
            var message = context.Message;
            await Task.CompletedTask;
        }

        public async Task ConsumeAsync(ConsumeContext<CriarProdutoMessage> consumerContext, CancellationToken cancellationToken)
        {
            var message = consumerContext.Message;
            await Task.CompletedTask;
        }
    }
}