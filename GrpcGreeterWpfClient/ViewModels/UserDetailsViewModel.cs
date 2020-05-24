using GalaSoft.MvvmLight;
using GrpcGreeterWpfClient.Models;
using GrpcGreeterWpfClient.Navigators;
using GrpcGreeterWpfClient.ServiceClients;
using System;

namespace GrpcGreeterWpfClient.ViewModels
{
  public class UserDetailsViewModel : ViewModelBase
  {
    private readonly ServiceClient serviceClient;
    private readonly SessionInstance sessionInstance;
    private string firstName;
    private string lastName;
    private string username;
    private string accountType;
    private bool detailsVisible;

    public UserDetailsViewModel(SessionInstance sessionInstance, ServiceClient serviceClient)
    {
      this.serviceClient = serviceClient;
      this.sessionInstance = sessionInstance ?? throw new ArgumentNullException(nameof(sessionInstance));
      DetailsVisible = sessionInstance.CurrentUser != null;
      if (sessionInstance.CurrentUser != null)
        UpdateUserDetails();
      else
        ClearDetails();
    }

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
