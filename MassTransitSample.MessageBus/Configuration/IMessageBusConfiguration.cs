namespace MassTransitSample.MessageBus.Configuration
{
    public interface IMessageBusConfiguration
    {
        //MessageBusFramework Framework { get; }
        //MessageBusProvider Provider { get; }
        //MessageBusScheduler Scheduler { get; }
        string Connection { get; }
        int MessageBusSchedulerWorkers { get; }
    }
}