using System.Net;
using System.Runtime.Serialization;

namespace SampleProject.Application.Exceptions
{
    public class AppException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }

        public AppException(HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
        {
            HttpStatusCode = httpStatusCode;
        }

        public AppException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }

        public AppException(string message, Exception innerException, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest) : base(message, innerException)
        {
            HttpStatusCode = httpStatusCode;
        }

        protected AppException(SerializationInfo info, StreamingContext context, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest) : base(info, context)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}