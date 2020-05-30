using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Grpc.Core;
using Grpc.Net.Client;
using NewBankClientGrpc;
using NewBankServer.Protos;
using NewBankWpfClient.Models;
using NewBankWpfClient.Navigators;
using NewBankWpfClient.ServiceClients;
using NewBankWpfClient.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace NewBankWpfClient.ViewModels
{
  public class LoginViewModel : ViewModelBase
  {
    private readonly ServiceClient serviceClient;
    private readonly SessionInstance sessionInstance;
    private string username;
    private string password;
    private RelayCommand loginCommand;
    private RelayCommand logoutCommand;
    private bool loggedIn;


    public LoginViewModel(SessionInstance sessionInstance, ServiceClient serviceClient)
    {
      this.serviceClient = serviceClient;
      this.sessionInstance = sessionInstance ?? throw new ArgumentNullException(nameof(sessionInstance));
      
      LoggedIn = serviceClient.SessionCRUDClient.IsValidSession(new SessionRequest { SessionId = this.sessionInstance.SessionID.ToString() }).Valid;
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
      User user = null;
      try
      {
        user = serviceClient.UserCRUDClient.GetByFilter(new UserFilter { Username = username }).Items.FirstOrDefault();
      }
      catch (RpcException rex)
      {
        //user doesn't exist
      }
      if (user == null)
        MessageBox.Show(new UsernameDoesNotExistTranslatable(), new ErrorTranslatable(), MessageBoxButton.OK, MessageBoxImage.Error);
      else
      {
        var hash = new SecurePassword(password, user.PasswordSalt).ComputeSaltedHash();
        if (hash == user.PasswordHash)
        {
          try
          {
            var loginResponse = serviceClient.AuthenticationClient.Login(new LoginRequest
            {
              Username = username,
              PasswordHash = hash
            });

            var account = serviceClient.AccountCRUDClient.GetByUserID(new AccountRequest { UserId = loginResponse.User.Id }).Account;

            sessionInstance.SessionID = Guid.Parse(loginResponse.SessionID);
            sessionInstance.CurrentUser = UserModel.ConvertUser(loginResponse.User);
            sessionInstance.CurrentAccount = AccountModel.ConvertAccount(account);
            LoggedIn = true;

            MessageBox.Show(new LoginSuccessfulTranslatable(), new InformationTranslatable(), MessageBoxButton.OK, MessageBoxImage.Information);
          }
          catch (RpcException rex)
          {
            MessageBox.Show(new LoginFailedErrorTranslatable(rex.Status.Detail), new ErrorTranslatable(), MessageBoxButton.OK, MessageBoxImage.Error);
          }
        }
      }
    }

    private void LogoutCommandExecute()
    {
      try
      {
        serviceClient.AuthenticationClient.Logout(new LogoutRequest { SessionId = sessionInstance.SessionID.ToString(), User = UserModel.ConvertUser(sessionInstance.CurrentUser) });
        sessionInstance.CurrentUser = null;
        sessionInstance.SessionID = Guid.Empty;
        LoggedIn = false;
        MessageBox.Show(new LogoutSuccessfulTranslatable(), new InformationTranslatable(), MessageBoxButton.OK, MessageBoxImage.Information);
      }
      catch (RpcException rex)
      {
        MessageBox.Show(new LogoutFailedErrorTranslatable(rex.Status.Detail), new ErrorTranslatable(), MessageBoxButton.OK, MessageBoxImage.Error);
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
