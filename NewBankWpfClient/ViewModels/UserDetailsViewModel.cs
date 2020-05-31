using GalaSoft.MvvmLight;
using NewBankShared.Localization;
using NewBankWpfClient.Models;
using NewBankWpfClient.Singletons;
using System;

namespace NewBankWpfClient.ViewModels
{
  public class UserDetailsViewModel : ViewModelBase
  {
    private readonly SessionInstance sessionInstance = SessionInstance.Instance;
    private string firstName;
    private string lastName;
    private string username;
    private string accountType;
    private bool detailsVisible;

    public UserDetailsViewModel()
    {
      DetailsVisible = sessionInstance.CurrentUser != null;
      if (sessionInstance.CurrentUser != null)
        UpdateUserDetails();
      else
        ClearDetails();
    }

    public string FirstNameLabel => $"{new FirstNameLabelTranslatable()}:";
    public string LastNameLabel => $"{new LastNameLabelTranslatable()}:";
    public string UsernameLabel => $"{new UsernameLabelTranslatable()}:";
    public string AccountTypeLabel => $"{new AccountTypeLabelTranslatable()}:";

    public string FirstName
    {
      get => firstName;
      set => Set(ref firstName, value);
    }

    public string LastName
    {
      get => lastName;
      set => Set(ref lastName, value);
    }


    public string Username
    {
      get => username;
      set => Set(ref username, value);
    }

    public string AccountType
    {
      get => accountType;
      set => Set(ref accountType, value);
    }

    public bool DetailsVisible
    {
      get => detailsVisible;
      set => Set(ref detailsVisible, value);
    }

    private void UpdateUserDetails()
    {
      FirstName = sessionInstance.CurrentUser.FirstName;
      LastName = sessionInstance.CurrentUser.LastName;
      Username = sessionInstance.CurrentUser.Username;
      AccountType = sessionInstance.CurrentAccount?.AccountType.ToString();
    }

    private void ClearDetails()
    {
      FirstName = string.Empty;
      LastName =  string.Empty;
      Username =  string.Empty;
      AccountType = string.Empty;
    }
  }
}
