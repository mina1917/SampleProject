namespace SampleProject.Application.Exceptions
{
    public class InvalidUserNameOrPasswordException : AppException
    {
        public InvalidUserNameOrPasswordException()
        {
        }
    }
    public class UserCreationException : AppException
    {
        public UserCreationException(string message) : base(message)
        {
        }
    }
    public class UserModificationException : AppException
    {
        public UserModificationException(string message) : base(message)
        {
        }
    }
    public class UserPasswordUpdateException : AppException
    {
        public UserPasswordUpdateException(string message) : base(message)
        {
        }
    }
}
