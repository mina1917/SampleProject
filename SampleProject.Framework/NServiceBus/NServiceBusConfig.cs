namespace SampleProject.Framework.NServiceBus
{
    public class NServiceBusConfig
    {
        public string EndpointName { get; set; }
        public string RabbitConnectionString { get; set; }
        public int ImmediateRetryCount { get; set; }
        public int DelayRetryCount { get; set; }
        public int DelayRetryTimeToIncrease { get; set; }
        public string License { get; set; }
        public bool SetupMonitoring { get; set; }
    }
}