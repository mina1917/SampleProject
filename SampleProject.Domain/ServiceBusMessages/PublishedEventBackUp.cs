namespace SampleProject.Domain.ServiceBusMessages
{
    public class PublishedEventBackUp
    {
        public Guid EventId { get; private set; }
        public long Id { get; set; }
        public DateTime PublishedOn { get; private set; }
        public DateTime? DelayedPublishedOn { get; private set; }
        public string Body { get; private set; }
        public bool IsPublished { get; private set; }
        public string EventType { get; private set; }

        public PublishedEventBackUp(Guid eventId, long id, DateTime publishedOn, DateTime? delayedPublishedOn, string body, bool isPublished, string eventType)
        {
            EventId = eventId;
            Id = id;
            PublishedOn = publishedOn;
            DelayedPublishedOn = delayedPublishedOn;
            Body = body;
            IsPublished = isPublished;
            EventType = eventType;
        }

        protected PublishedEventBackUp() { }
    }
}