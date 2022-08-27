namespace SampleProject.Framework
{
    public abstract class QueryModelBase<T>
    {
        public T Id { get; set; }
        public string PartitionKey { get; set; }
        public DateTime LastUpdate { get; set; }
        public string _etag { get; set; }
        protected QueryModelBase(T id, string partitionKey)
        {
            Id = id;
            PartitionKey = partitionKey;
            LastUpdate = DateTime.UtcNow;
        }

        protected QueryModelBase() { }
    }
}