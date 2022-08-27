using SampleProject.Framework;

namespace SampleProject.Query.DomainEvents
{
    public class UserCreated : DomainEvent
    {
        public UserCreated(Guid id, DateTime dateCreated, string email, string mobile)
        {
            Id = id;
            DateCreated = dateCreated;
            Email = email;
            Mobile = mobile;
        }

        public Guid Id { get; private set; }
        public DateTime DateCreated { get; private set; }
        public string Email { get; private set; }
        public string Mobile { get; set; }


    }
}