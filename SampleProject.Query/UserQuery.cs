using SampleProject.Framework;

namespace SampleProject.Query
{
    public class UserQuery : QueryModelBase<string>
    {
        public UserQuery(string userName, string password, string email)
        {
            UserName = userName;
            Password = password;
            Email = email;
        }

        public string UserName { get; private set; }
        public string Password { get; private set; }

        public string Email { get; private set; }

    }
}
