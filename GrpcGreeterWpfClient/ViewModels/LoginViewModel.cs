//using BankServer.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Grpc.Net.Client;
using GrpcGreeter.Protos;
using GrpcGreeterWpfClient.Models;
using GrpcGreeterWpfClient.Navigators;
using GrpcGreeterWpfClient.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GrpcGreeterWpfClient.ViewModels
{
  public class LoginViewModel : ViewModelBase
  {
    //private readonly SessionService sessionService;
    private readonly SessionInstance sessionInstance;
    private string username;
    private string password;
    private RelayCommand loginCommand;
    private RelayCommand logoutCommand;
    private bool loggedIn;


    public LoginViewModel(/*SessionService sessionService, */SessionInstance sessionInstance)
    {
      using var channel = GrpcChannel.ForAddress("https://localhost:5001");
      var sessionClient = new SessionCRUD.SessionCRUDClient(channel);
      //this.sessionService = sessionService;
      this.sessionInstance = sessionInstance ?? throw new ArgumentNullException(nameof(sessionInstance));
      LoggedIn = sessionClient.IsValidSession(new SessionRequest { SessionId = this.sessionInstance.SessionID.ToString() }).Valid;
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

    public bool LoggedIn
    {
      get => loggedIn;
      set => Set(ref loggedIn, value);
    }


    public event EventHandler<LoginEventArgs> LoginEventHandler;

    public void OnLogin(UserModel user)
    {
      LoginEventHandler?.Invoke(this, new LoginEventArgs(user));
    }

    public RelayCommand LoginCommand => loginCommand ??= new RelayCommand(LoginCommandExecute);
    public RelayCommand LogoutCommand => logoutCommand ??= new RelayCommand(LogoutCommandExecute);

    private void LoginCommandExecute()
    {
      using var channel = GrpcChannel.ForAddress("https://localhost:5001");
      var userCRUDClient = new UserCRUD.UserCRUDClient(channel);
      var authenticationClient = new Authentication.AuthenticationClient(channel);
      var accountClient = new AccountCRUD.AccountCRUDClient(channel);
      var user = userCRUDClient.GetByFilter(new UserFilter { Username = username }).Items.FirstOrDefault();
      if (user == null)
        MessageBox.Show("This username does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      else
      {
        var hash = new SecurePassword(password, user.PasswordSalt).ComputeSaltedHash();
        if (hash == user.PasswordHash)
        {
          try
          {
            var loginResponse = authenticationClient.Login(new LoginRequest
            {
              Username = username,
              PasswordHash = hash
            });

            var account = accountClient.GetByUserID(new AccountRequest { UserId = loginResponse.User.Id }).Account;

            sessionInstance.SessionID = Guid.Parse(loginResponse.SessionID);
            sessionInstance.CurrentUser = UserModel.ConvertUser(loginResponse.User);
            sessionInstance.CurrentAccount = AccountModel.ConvertAccount(account);
            LoggedIn = true;

            MessageBox.Show("Log in successful.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
          }
          catch (Exception ex)
          {
            MessageBox.Show($"Log in failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          }
        }
      }
    }

    private void LogoutCommandExecute()
    {
      using var channel = GrpcChannel.ForAddress("https://localhost:5001");
      var userCRUDClient = new UserCRUD.UserCRUDClient(channel);
      var authenticationClient = new Authentication.AuthenticationClient(channel);
      try
      {
        authenticationClient.Logout(new LogoutRequest { SessionId = sessionInstance.SessionID.ToString() });
        sessionInstance.CurrentUser = null;
        sessionInstance.SessionID = Guid.Empty;
        LoggedIn = false;
        MessageBox.Show("Log out successful.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Log in failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }
  }

  public class LoginEventArgs : EventArgs
  {
    public UserModel User { get; set; }
    public LoginEventArgs(UserModel user)
    {
      User = user;
    }
  }
}
