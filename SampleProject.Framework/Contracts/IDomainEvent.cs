namespace SampleProject.Framework.Contracts
{
    public interface IDomainEvent
    {
        IReadOnlyCollection<DomainEvent> GetEvents();
    }
}
