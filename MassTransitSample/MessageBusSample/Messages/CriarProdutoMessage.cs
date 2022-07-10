using MassTransitSample.MessageBus.Messages;

namespace MassTransitSample.MessageBusSample.Messages
{
    public class CriarProdutoMessage : ICommandMessage
    {
        public string Nome { get; set; }
        public decimal Valor { get; set; }
    }
}