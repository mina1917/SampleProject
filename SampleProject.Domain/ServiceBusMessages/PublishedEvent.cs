namespace SampleProject.Domain.ServiceBusMessages
{
    public class PublishedEvent
    {
        public Guid EventId { get; private set; }
        public long Id { get; set; }
        public DateTime PublishedOn { get; private set; }
        public DateTime? DelayedPublishedOn { get; private set; }
        public string Body { get; private set; }
        public bool IsPublished { get; private set; }
        public string EventType { get; private set; }

        public PublishedEvent(Guid eventId, string body, bool isPublished, string eventType)
        {
            EventId = eventId;
            PublishedOn = DateTime.UtcNow;
            DelayedPublishedOn = null;
            Body = body;
            IsPublished = isPublished;
            EventType = eventType;
        }

        protected PublishedEvent()
        {
        }
        public void SetAsPublished()
        {
            IsPublished = true;
        }
        public void SetDelayedPublishedOn()
        {
            DelayedPublishedOn = DateTime.UtcNow;
        }
    }
}