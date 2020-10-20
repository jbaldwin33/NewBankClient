using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewBankWpfClient.ViewModels
{
  public class LoginValidation : AbstractValidator<LoginViewModel>
  {
    public LoginValidation()
    {
      RuleFor(x => x.Username).NotEmpty().WithMessage("Please enter a username");
      RuleFor(x => x.SecurePassword.Length).NotEqual(0).WithMessage("Please enter a password");
    }
  }
}
