using MassTransitSample.MessageBus.Messages;

namespace MassTransitSample.MessageBusSample.Messages
{
    public class AtualizarProdutoMessage : IEventMessage
    {
        public string Nome { get; set; }
        public decimal Valor { get; set; }
    }
}