using MassTransitSample.MessageBus.Producers;
using MassTransitSample.MessageBusSample.Messages;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MassTransitSample.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly IMessageBusProducer _messageBusProducer;

        public ProdutoController(IMessageBusProducer messageBusProducer)
        {
            _messageBusProducer = messageBusProducer;
        }

        /// <summary>
        /// Send message to a queue.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("command")]
        public async Task<IActionResult> Publish(CriarProdutoMessage message)
        {
            await _messageBusProducer.PublishAsync(message);
            return Ok();
        }
    }
}