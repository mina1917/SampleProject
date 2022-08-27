namespace SampleProject.Framework
{
    public abstract class Publisher
    {
        private readonly List<DomainEvent> _publishedEvents = new List<DomainEvent>();
        protected Publisher() { }

        public void Publish<TEvent>(TEvent @event) where TEvent : DomainEvent
        {
            _publishedEvents.Add(@event);
        }
        public IReadOnlyList<DomainEvent> GetChanges()
        {
            return _publishedEvents;
        }
        public void ClearChanges()
        {
            _publishedEvents.Clear();
        }
    }
}