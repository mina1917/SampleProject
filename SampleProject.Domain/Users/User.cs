using SampleProject.Framework;
using SampleProject.Framework.Contracts;
using SampleProject.Query.DomainEvents;

namespace SampleProject.Domain.Users
{
    public class User : Publisher, IAuditable<Guid>
    {
        public User()
        {
        }

        public User(string userName, string password, string email)
        {
            UserName = userName;
            Password = password;
            Email = email;
        }

        public User(Guid id, string userName, string password, string email)
        {
            Id = id;
            UserName = userName;
            Password = password;
            Email = email;
            Publish(new UserCreated(Id, DateTime.Now, Email, Password));
        }

        public Guid Id { get; set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }

        public string Email { get; private set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid Creator { get; set; }
        public Guid? Modifire { get; set; }
    }
}
