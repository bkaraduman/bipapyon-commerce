using BiPapyon.Common.ViewModels;
namespace BiPapyon.Common.Infrastructure.Exceptions
{
    public class ServiceException : ServerException
    {
        public ServiceException(string message) : base(message)
        {
            base.ErrorCode = (int)ResponseCode.InternalError;
        }

        public ServiceException(string message, ResponseCode errorCode) : base(message)
        {
            ErrorCode = (int)errorCode;
        }
    }
}
