using BankServer.Services;
using GalaSoft.MvvmLight;
using GrpcGreeter.Protos;
using GrpcGreeterWpfClient.Models;
using GrpcGreeterWpfClient.Navigators;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text;
using System.Threading.Tasks;

namespace GrpcGreeterWpfClient.ViewModels
{
  public class AccountViewModel : ViewModelBase
  {
    private UserCRUD.UserCRUDClient userCRUDClient;
    private readonly SessionService sessionService;
    private readonly SessionInstance sessionInstance;
    private string firstName;
    private string lastName;
    private string username;
    private int? age;
    private string accountType;
    private bool detailsVisible;

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

    public int? Age
    {
      get => age;
      set => Set(ref age, value);
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

    public AccountViewModel(UserCRUD.UserCRUDClient client, SessionService sessionService, SessionInstance sessionInstance)
    {

      userCRUDClient = client;
      this.sessionInstance = sessionInstance ?? throw new ArgumentNullException(nameof(sessionInstance));
      this.sessionService = sessionService;
      DetailsVisible = sessionInstance.CurrentUser != null;
      if (sessionInstance.CurrentUser != null)
        UpdateUserDetails();
      else
        ClearDetails();
    }

    private void UpdateUserDetails()
    {
      FirstName = sessionInstance.CurrentUser.FirstName;
      LastName = sessionInstance.CurrentUser.LastName;
      Username = sessionInstance.CurrentUser.Username;
      Age = sessionInstance.CurrentUser?.Age;
      AccountType = sessionInstance.CurrentAccount?.AccountType.ToString();
    }

    private void ClearDetails()
    {
      FirstName = string.Empty;
      LastName =  string.Empty;
      Username =  string.Empty;
      Age = 0;
      AccountType = string.Empty;
    }
  }
}
