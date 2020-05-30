﻿using ControlzEx.Standard;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Grpc.Core;
using Grpc.Net.Client;
using NewBankServer.Protos;
using NewBankWpfClient.Models;
using NewBankWpfClient.Navigators;
using NewBankWpfClient.ServiceClients;
using NewBankWpfClient.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using static NewBankWpfClient.Models.Enums;
using NewBankClientGrpc;
using NewBankClientGrpc.Localization;

namespace NewBankWpfClient.ViewModels
{
  public class SignUpViewModel : ViewModelBase
  {
    private string firstName;
    private string lastName;
    private string username;
    private string password;
    private ObservableCollection<AccountTypeViewModel> accountTypes;
    private AccountEnum accountType;
    private RelayCommand signUpCommand;
    private readonly ServiceClient serviceClient;
    private readonly SessionInstance sessionInstance;
    private bool signUpDetailsVisible;

    public SignUpViewModel(SessionInstance sessionInstance, ServiceClient serviceClient)
    {
      this.serviceClient = serviceClient;
      this.sessionInstance = sessionInstance ?? throw new ArgumentNullException(nameof(sessionInstance));
      SignUpDetailsVisible = !serviceClient.SessionCRUDClient.IsValidSession(new SessionRequest { SessionId = this.sessionInstance.SessionID.ToString() }).Valid;
      AccountTypes = new ObservableCollection<AccountTypeViewModel>
      {
        new AccountTypeViewModel(AccountEnum.Checking, new CheckingLabelTranslatable()),
        new AccountTypeViewModel(AccountEnum.Saving, new SavingLabelTranslatable())
      };
    }

    public string FirstNameLabel => $"{new FirstNameLabelTranslatable()}:";
    public string LastNameLabel => $"{new LastNameLabelTranslatable()}:";
    public string UsernameLabel => $"{new UsernameLabelTranslatable()}:";
    public string PasswordLabel => $"{new PasswordLabelTranslatable()}:";
    public string AccountTypeLabel => $"{new AccountTypeLabelTranslatable()}:";
    public string SignUpLabel => new SignUpLabelTranslatable();

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


    public ObservableCollection<AccountTypeViewModel> AccountTypes
    {
      get => accountTypes;
      set => Set(ref accountTypes, value);
    }


    public AccountEnum AccountType
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

    public RelayCommand SignUpCommand => signUpCommand ??= new RelayCommand(SignUpCommandExecute);

    private void SignUpCommandExecute()
    {
      if (!ValidInput())
        return;

      try
      {
        var users = serviceClient.UserCRUDClient.GetUsers(new Empty());
        if (users.Items.Any(u => u.Username == username))
          MessageBox.Show(new UsernameAlreadyExistsTranslatable(), new ErrorTranslatable(), MessageBoxButton.OK, MessageBoxImage.Error);
        else
        {
          DoSignUp();
          MessageBox.Show(new SignUpSuccessfulTranslatable(), new InformationTranslatable(), MessageBoxButton.OK, MessageBoxImage.Information);
        }
      }
      catch (RpcException rex)
      {
        MessageBox.Show(new SignUpFailedErrorTranslatable(rex.Status.Detail), new ErrorTranslatable(), MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void DoSignUp()
    {
      var accountID = Guid.NewGuid();
      var userID = Guid.NewGuid();
      var passwordSalt = SecurePassword.CreateSalt();

      var user = new User
      {
        Username = username,
        PasswordSalt = passwordSalt,
        PasswordHash = new SecurePassword(password, passwordSalt).ComputeSaltedHash(),
        FirstName = firstName,
        LastName = lastName,
        Id = userID.ToString(),
        AccountId = accountID.ToString(),
        UserType = UserProtoEnum.User
      };
      var account = new Account
      {
        Id = accountID.ToString(),
        UserId = userID.ToString(),
        Balance = 0.0,
        AccountType = AccountModel.ConvertAccountType(AccountType)
      };

      serviceClient.CreationClient.SignUp(new SignUpRequest { Account = account, User = user });
    }

    private bool ValidInput()
    {
      Translatable errorMessage = null;
      if (string.IsNullOrEmpty(username))
        errorMessage = new UsernameEmptyTranslatable();
      else if (string.IsNullOrEmpty(password))
        errorMessage = new PasswordEmptyTranslatable();
      else if (string.IsNullOrEmpty(firstName))
        errorMessage = new FirstNameEmptyTranslatable();
      else if (string.IsNullOrEmpty(lastName))
        errorMessage = new LastNameEmptyTranslatable();

      if (errorMessage == null)
        return true;

      MessageBox.Show(errorMessage, new ErrorTranslatable(), MessageBoxButton.OK, MessageBoxImage.Error);
      return false;
    }
  }

  public class AccountTypeViewModel
  {
    public AccountEnum AccType { get; set; }
    public string Name { get; set; }

    public AccountTypeViewModel(AccountEnum accountType, string name)
    {
      AccType = accountType;
      Name = name;
    }
  }
}