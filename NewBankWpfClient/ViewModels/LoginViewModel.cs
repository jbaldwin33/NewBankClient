using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Grpc.Core;
using Grpc.Net.Client;
using NewBankServer.Protos;
using NewBankShared.Localization;
using NewBankWpfClient.Models;
using NewBankWpfClient.Singletons;
using NewBankWpfClient.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace NewBankWpfClient.ViewModels
{
  public class LoginViewModel : ViewModelBase
  {
    private readonly ServiceClient serviceClient = ServiceClient.Instance;
    private readonly SessionInstance sessionInstance = SessionInstance.Instance;
    private string username;
    private SecureString securePassword;
    private RelayCommand loginCommand;
    private RelayCommand logoutCommand;
    private bool loggedIn;


    public LoginViewModel()
    {
      LoggedIn = sessionInstance.CurrentUser != null;
    }

    public string LoginLabel => new LoginLabelTranslatable();
    public string LogoutLabel => new LogoutLabelTranslatable();
    public string UsernameLabel => $"{new UsernameLabelTranslatable()}:";
    public string PasswordLabel => $"{new PasswordLabelTranslatable()}:";

    public string Username
    {
      get => username;
      set => Set(ref username, value);
    }

    public SecureString SecurePassword
    {
      get => securePassword;
      set => Set(ref securePassword, value);
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
      if (!InputIsValid())
        return;
      User user;
      try
      {
        user = serviceClient.UserCRUDClient.GetByFilter(new UserFilter { Username = username }).Items.FirstOrDefault();
      }
      catch (RpcException rex) when (rex.StatusCode == StatusCode.NotFound)
      {
        MessageBox.Show(new UsernameDoesNotExistTranslatable(), new ErrorTranslatable(), MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      var hash = new SecurePasswordUtility(securePassword, user.PasswordSalt).ComputeSaltedHash();
      if (hash == user.PasswordHash)
      {
        try
        {
          var loginResponse = serviceClient.AuthenticationClient.Login(new LoginRequest
          {
            Username = username,
            PasswordHash = hash
          });

          var account = serviceClient.AccountCRUDClient.GetByUserID(new AccountRequest { UserId = loginResponse.User.Id, SessionId = loginResponse.SessionID.ToString() }).Account;

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

    private bool InputIsValid()
    {
      Translatable errorMessage = null;
      if (string.IsNullOrEmpty(username))
        errorMessage = new UsernameEmptyTranslatable();
      else if (securePassword == null)
        errorMessage = new PasswordEmptyTranslatable();

      if (errorMessage == null)
        return true;

      MessageBox.Show(errorMessage, new ErrorTranslatable(), MessageBoxButton.OK, MessageBoxImage.Error);
      return false;
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
