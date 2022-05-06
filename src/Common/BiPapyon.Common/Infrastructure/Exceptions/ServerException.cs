using BiPapyon.Common.ViewModels;
using sys = System;

namespace BiPapyon.Common.Infrastructure.Exceptions
{
    [sys.Serializable]
    public class ServerException : sys.Exception
    {
        public int ErrorCode;
        public string ServerMessageDetail;

        public ServerException(string message)
            : base(message)
        {
            this.ErrorCode = (int)ResponseCode.GeneralError;
        }

        public ServerException(string message, int errorCode)
            : base(message)
        {
            this.ErrorCode = errorCode;
        }

        public ServerException(string message, ResponseCode errorCode)
            : base(message)
        {
            this.ErrorCode = (int)errorCode;
        }

        public ServerException(string message, int errorCode, string serverMessageDetail)
            : base(message)
        {
            this.ErrorCode = errorCode;
            this.ServerMessageDetail = serverMessageDetail;
        }
    }
}
