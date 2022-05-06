using BiPapyon.Common.ViewModels;

namespace BiPapyon.Common.Infrastructure.Exceptions
{
    public class PermissionException : ServerException
    {
        public PermissionException(string message) : base(message)
        {
            base.ErrorCode = (int)ResponseCode.Unauthorized;
        }
    }
}
