using SampleProject.Domain.Users;

namespace SampleProject.Messaging.Senders
{
    public interface IUserCreateSender
    {
        void SendUser(User customer);
    }
}