using SampleProject.Framework.Contracts;
using SampleProject.Framework.Guards;

namespace SampleProject.Framework
{
    public abstract class AggregateRoot : IDomainEvent, IEntity
    {
        private readonly List<DomainEvent> _events = new();

        public virtual Guid Id
        {
            get;
            protected set;
        }

        public byte[] RowVersion
        {
            get;
        }

        protected AggregateRoot()
        {
        }

        protected AggregateRoot(Guid id)
        {
            Guard.AgainstNavigateOrZero(id, "id");
            Id = id;
        }

        public IReadOnlyCollection<DomainEvent> GetEvents()
        {
            return _events.AsReadOnly();
        }

        protected void AddEvent(DomainEvent domainEvent)
        {
            if (domainEvent != null)
            {
                if (_events.Contains(domainEvent))
                    throw new ArgumentException("Can't add duplicate event");

                _events.Add(domainEvent);
            }
        }

        public override bool Equals(object obj)
        {
            var aggregateRoot = obj as AggregateRoot;
            if ((object)aggregateRoot == null)
                return false;

            if ((object)this == aggregateRoot)
                return true;

            if (GetUnproxiedType(this) != GetUnproxiedType(aggregateRoot))
                return false;

            if (Id.Equals(default) || aggregateRoot.Id.Equals(default))
                return false;

            return Id.Equals(aggregateRoot.Id);
        }

        public override int GetHashCode()
        {
            return (GetUnproxiedType(this).ToString() + Id).GetHashCode();
        }

        public static bool operator ==(AggregateRoot left, AggregateRoot right)
        {
            if ((object)left == null && (object)right == null)
                return true;

            if ((object)left == null || (object)right == null)
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(AggregateRoot left, AggregateRoot right)
        {
            return !(left == right);
        }

        public static Type GetUnproxiedType(object obj)
        {
            var type = obj.GetType();
            var text = type.ToString();
            if (text.Contains("Castle.Proxies.") || text.EndsWith("Proxy"))
                return type.BaseType;

            return type;
        }
    }
}
