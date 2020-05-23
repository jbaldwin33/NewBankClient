//using BankServer.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GrpcGreeter.Protos;
using GrpcGreeterWpfClient.Models;
using GrpcGreeterWpfClient.Navigators;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace GrpcGreeterWpfClient.ViewModels
{
  public class UserDetailsViewModel : ViewModelBase
  {
    //private readonly SessionService sessionService;
    private readonly SessionInstance sessionInstance;
    private string firstName;
    private string lastName;
    private string username;
    private double? balance;
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

    public double? Balance
    {
      get => balance;
      set => Set(ref balance, value);
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

    public UserDetailsViewModel(/*SessionService sessionService, */SessionInstance sessionInstance)
    {

      this.sessionInstance = sessionInstance ?? throw new ArgumentNullException(nameof(sessionInstance));
      //this.sessionService = sessionService;
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
      Balance = sessionInstance.CurrentUser?.Age;
      AccountType = sessionInstance.CurrentAccount?.AccountType.ToString();
    }

    private void ClearDetails()
    {
      FirstName = string.Empty;
      LastName =  string.Empty;
      Username =  string.Empty;
      Balance = 0;
      AccountType = string.Empty;
    }
  }
}
