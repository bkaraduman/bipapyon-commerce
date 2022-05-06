using System.Net;

namespace BiPapyon.Common.Infrastructure.Exceptions
{
    public class ServiceExceptionPoco
    {
        public HttpStatusCode HttpStatus;
        public int ServiceStatus;
        public string Message;
    }
}
