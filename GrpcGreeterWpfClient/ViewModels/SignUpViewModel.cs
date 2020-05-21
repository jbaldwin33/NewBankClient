using BankServer.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Grpc.Net.Client;
using GrpcGreeter.Protos;
using GrpcGreeterWpfClient.Models;
using GrpcGreeterWpfClient.Navigators;
using GrpcGreeterWpfClient.Utilities;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows;
using System.Windows.Input;
using static GrpcGreeterWpfClient.Models.Enums;
using static GrpcGreeterWpfClient.Utilities.Utilities;

namespace GrpcGreeterWpfClient.ViewModels
{
  public class SignUpViewModel : ViewModelBase
  {
    //private readonly UserCRUD.UserCRUDClient userCRUDClient;
    //private readonly AccountCRUD.AccountCRUDClient accountCRUDClient;
    private string firstName;
    private string lastName;
    private string username;
    private string password;
    private int age;
    private ObservableCollection<AccountTypeViewModel> accountTypes;
    private AccountTypeEnum accountType;
    private RelayCommand signUpCommand;
    private readonly SessionService sessionService;
    private readonly SessionInstance sessionInstance;
    private bool signUpDetailsVisible;

    public SignUpViewModel(UserCRUD.UserCRUDClient userCRUDClient, AccountCRUD.AccountCRUDClient accountCRUDClient, SessionService sessionService, SessionInstance sessionInstance)
    {
      //this.userCRUDClient = userCRUDClient;
      //this.accountCRUDClient = accountCRUDClient;
      this.sessionService = sessionService;
      this.sessionInstance = sessionInstance ?? throw new ArgumentNullException(nameof(sessionInstance));
      SignUpDetailsVisible = !sessionService.IsValidSession(sessionInstance.SessionID);
      AccountTypes = new ObservableCollection<AccountTypeViewModel>
      {
        new AccountTypeViewModel(AccountTypeEnum.Checking, "Checking"),
        new AccountTypeViewModel(AccountTypeEnum.Saving, "Saving")
      };
    }

    #region Properties
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
    public string Password
    {
      get => password;
      set => Set(ref password, value);
    }

    public int Age
    {
      get => age;
      set => Set(ref age, value);
    }


    public ObservableCollection<AccountTypeViewModel> AccountTypes
    {
      get => accountTypes;
      set => Set(ref accountTypes, value);
    }


    public AccountTypeEnum AccountType
    {
      get => accountType;
      set => Set(ref accountType, value);
    }


    public bool SignUpDetailsVisible
    {
      get => signUpDetailsVisible;
      set => Set(ref signUpDetailsVisible, value);
    }

    #endregion

    public RelayCommand SignUpCommand => signUpCommand ?? (signUpCommand = new RelayCommand(SignUpCommandExecute));

    private void SignUpCommandExecute()
    {
      using var channel = GrpcChannel.ForAddress("https://localhost:5001");
      var userCRUDClient = new UserCRUD.UserCRUDClient(channel);
      var accountCRUDClient = new AccountCRUD.AccountCRUDClient(channel);
      try
      {
        var users = userCRUDClient.GetUsers(new Empty());
        if (users.Items.Any(u => u.Username == username))
          MessageBox.Show("This username already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        else
        {
          var accountID = Guid.NewGuid();
          var userID = Guid.NewGuid();
          var skillID = Guid.NewGuid();
          var passwordSalt = SecurePassword.CreateSalt();

          var userReply = userCRUDClient.Insert(new User
          {
            Username = username,
            PasswordSalt = passwordSalt,
            PasswordHash = new SecurePassword(password, passwordSalt).ComputeSaltedHash(),
            FirstName = firstName,
            LastName = lastName,
            Id = userID.ToString(),
            Age = age,
            AccountId = accountID.ToString(),
            UserType = UserProtoType.User
          });
          var accountReply = accountCRUDClient.Insert(new Account
          {
            Id = accountID.ToString(),
            UserId = userID.ToString(),
            Balance = 0.0,
            AccountType = AccountModel.ConvertAccountType(AccountType)
          });
          MessageBox.Show("Sign up successful", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Sign up failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }
  }

  public class AccountTypeViewModel
  {
    public AccountTypeEnum AccType { get; set; }
    public string Name { get; set; }

    public AccountTypeViewModel(AccountTypeEnum accountType, string name)
    {
      AccType = accountType;
      Name = name;
    }
  }
}
