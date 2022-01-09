using FluentValidation;

namespace NewBankWpfClient.ViewModels
{
    public class LoginValidation : AbstractValidator<LoginViewModel>
    {
        public LoginValidation()
        {
            RuleFor(x => x.Username).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("Please enter a {PropertyName}")
                .NotEmpty().WithMessage("Please enter a {PropertyName}")
                .Length(3, 10).WithMessage("{PropertyName} must be between 3-10 characters long.");
            RuleFor(x => x.SecurePassword).NotNull().WithMessage("Please enter a {PropertyName}");
        }
    }
}
