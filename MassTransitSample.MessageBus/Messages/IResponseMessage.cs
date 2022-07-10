using MassTransit;
using System;

namespace MassTransitSample.MessageBus.Messages
{
    public interface IResponseMessage : IMessage
    {
        Guid RequestId { get; set; }
        string ResponseAddress { get; set; }
        bool IsCompleted { get; }
        MessageException Exception { get; }
    }
}