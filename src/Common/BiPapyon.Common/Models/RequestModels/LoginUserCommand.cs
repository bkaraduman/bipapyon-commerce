using BiPapyon.Common.Models.Queries;
using MediatR;

namespace BiPapyon.Common.Models.RequestModels
{
    public class LoginUserCommand : IRequest<LoginUserViewModel>
    {
        public LoginUserCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
        public LoginUserCommand()
        {

        }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
