using BiPapyon.Common.Models.RequestModels;
using FluentValidation;

namespace BiPapyon.Api.Application.Features.Commands.User
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible)
                .WithMessage("{PropertyName} not a valid email address");
            
            RuleFor(x => x.Password).NotNull()
                .MinimumLength(6).WithMessage("{PropertyName} must be at least {MinLength} characters long");
        }
    }
}
