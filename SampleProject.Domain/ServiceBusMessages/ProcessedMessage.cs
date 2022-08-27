namespace SampleProject.Domain.ServiceBusMessages
{
    public class ProcessedMessage
    {
        public string EventId { get; private set; }
        public string Handler { get; private set; }
        public DateTime CreateOn { get; private set; }

        public ProcessedMessage(string eventId, string handler)
        {
            EventId = eventId;
            Handler = handler;
            CreateOn = DateTime.UtcNow;
        }

        public ProcessedMessage()
        {
        }
    }
}