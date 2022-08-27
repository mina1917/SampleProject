namespace SampleProject.Framework.NServiceBus
{
    public interface IServiceBusSender
    {
        Task PublishMessage<T>(T domainEvent) where T : DomainEvent;
        Task SendMessage<T>(T domainEvent, TimeSpan timeSpan) where T : Command;
        Task SendMessage<T>(T command) where T : Command;
    }
}
