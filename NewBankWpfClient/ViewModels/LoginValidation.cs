using FluentValidation;

namespace NewBankWpfClient.ViewModels
{
    public class LoginValidation : AbstractValidator<LoginViewModel>
    {
        public LoginValidation()
        {
            RuleFor(x => x.Username).NotNull().WithMessage("Please enter a username");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Please enter a username");
            RuleFor(x => x.SecurePassword).NotNull().WithMessage("Please enter a password");
        }
    }
}
