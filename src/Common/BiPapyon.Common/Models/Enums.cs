namespace BiPapyon.Common.ViewModels
{
    public enum VoteType
    {
        None = -1,
        DownVote = 0,
        UpVote = 1
    }
    public enum ResponseCode : int
    {
        Success = 200,
        InternalError = 500,
        GeneralError = 700,
        PaymentError = 900,
        NotFound = 404,
        BadRequest = 400,
        NoContent = 204,
        Created = 201,
        Unauthorized = 401,
        Forbid = 403
    }
}
