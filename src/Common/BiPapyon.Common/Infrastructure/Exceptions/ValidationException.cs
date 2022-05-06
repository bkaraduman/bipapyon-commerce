using BiPapyon.Common.ViewModels;

namespace BiPapyon.Common.Infrastructure.Exceptions
{
    public class ValidationException : ServerException
    {

        public ValidationException(string message) : base(message)
        {
            base.ErrorCode = (int)ResponseCode.BadRequest;
        }

        public ValidationException(string message, ResponseCode errorCode) : base(message)
        {
            ErrorCode = (int)errorCode;
        }

        public ValidationException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
