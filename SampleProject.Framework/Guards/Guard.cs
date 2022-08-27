using SampleProject.Framework.Exceptions;

namespace SampleProject.Framework.Guards
{
    public static class Guard
    {
        public static void AgainstNullValue(object parameter, string? message = null)
        {
            if (parameter == null)
                throw new BusinessException(message);
        }

        public static void AgainstDefultGuidValue(Guid parameter, string? message = null)
        {
            if (parameter == Guid.Empty)
                throw new BusinessException(message);
        }

        public static void AgainstDefultDateValue(DateTime parameter, string? message = null)
        {
            if (parameter == DateTime.MinValue)
                throw new BusinessException(message);
        }

        public static void AgainstNullOrEmpty(string value, string? message = null)
        {
            if (string.IsNullOrEmpty(value))
                throw new BusinessException(message);
        }

        public static void AgainstNullOrWhiteSpace(string value, string? message = null)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new BusinessException(message);
        }

        public static void AgainstZero(IComparable value, string? message = null)
        {
            if (value.CompareTo(0) == 0)
                throw new BusinessException(message);
        }

        public static void AgainstNavigateOrZero(IComparable value, string? message = null)
        {
            if (!(value.CompareTo(0) <= 0))
                throw new BusinessException(message);
        }
    }
}
