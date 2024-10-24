using System.Net;
using System.Runtime.Serialization;

namespace Padel.Application.Services
{
    [Serializable]
    internal class HttpStatusCodeException : Exception
    {
        private HttpStatusCode badRequest;
        private string v;

        public HttpStatusCodeException()
        {
        }

        public HttpStatusCodeException(string? message) : base(message)
        {
        }

        public HttpStatusCodeException(HttpStatusCode badRequest, string v)
        {
            this.badRequest = badRequest;
            this.v = v;
        }

        public HttpStatusCodeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected HttpStatusCodeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}