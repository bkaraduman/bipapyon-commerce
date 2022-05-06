using BiPapyon.Common.ViewModels;

namespace BiPapyon.Common.Infrastructure.Exceptions
{
    public class NotFoundException : ServerException
    {
        public NotFoundException(string message) : base(message)
        {
            base.ErrorCode = (int)ResponseCode.NotFound;
        }

        public NotFoundException(string message, ResponseCode errorCode) : base(message)
        {
            ErrorCode = (int)errorCode;
        }
    }
}
