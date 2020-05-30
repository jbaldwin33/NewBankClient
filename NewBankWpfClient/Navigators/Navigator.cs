using GalaSoft.MvvmLight;
using NewBankServer.Protos;
using NewBankWpfClient.Models;
using NewBankWpfClient.ServiceClients;
using NewBankWpfClient.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using NewBankShared.Localization;

namespace NewBankWpfClient.Navigators
{
  public enum ViewType
  {
    Home,
    Account,
    UserDetails,
    LogIn,
    SignUp,
    Transactions
  }
  public interface INavigator
  {
    ViewModelBase CurrentViewModel { get; set; }
    SessionInstance SessionInstance { get; set; }
    ServiceClient ServiceClient { get; set; }
    ICommand UpdateCurrentViewModelCommand { get; }
    bool IsActiveSession { get; }
  }
  public class Navigator : INavigator, INotifyPropertyChanged
  {
    private ViewModelBase currentViewModel;
    private SessionInstance sessionInstance;
    private ServiceClient serviceClient;
    private bool isActiveSession;

    public ViewModelBase CurrentViewModel
    {
      get => currentViewModel;
      set
      {
        currentViewModel = value;
        OnPropertyChanged(nameof(CurrentViewModel));
      }
    }

    public string HomeLabel => new HomeLabelTranslatable();
    public string AccountLabel => new AccountLabelTranslatable();
    public string UserDetailsLabel => new UserDetailsLabelTranslatable();
    public string LoginLabel => new LoginLabelTranslatable();
    public string SignUpLabel => new SignUpLabelTranslatable();
    public string TransactionsLabel => new TransactionsLabelTranslatable();

    public SessionInstance SessionInstance
    {
      get => sessionInstance;
      set
      {
        sessionInstance = value;
        OnPropertyChanged(nameof(sessionInstance));
      }
    }

    public ServiceClient ServiceClient
    {
      get => serviceClient;
      set
      {
        serviceClient = value;
        OnPropertyChanged(nameof(serviceClient));
      }
    }

    public bool IsActiveSession
    {
      get => isActiveSession;
      set
      {
        isActiveSession = value;
        OnPropertyChanged(nameof(isActiveSession));
      }
    }

    public Navigator(SessionInstance sessionInstance, ServiceClient serviceClient)
    {
      ServiceClient = serviceClient;
      SessionInstance = sessionInstance;
      SessionInstance.UserChanged += UserChanged;
    }

    private void UserChanged(object sender, UserChangedEventArgs e) => IsActiveSession = e.UserExists;

    public ICommand UpdateCurrentViewModelCommand => new UpdateCurrentViewModelCommand(this);

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }

  public class SessionInstance : INotifyPropertyChanged
  {
    private UserModel currentUser;
    private AccountModel currentAccount;
    private Guid sessionID;
    public event EventHandler<UserChangedEventArgs> UserChanged;
    public SessionInstance(UserModel user, AccountModel account, Guid id)
    {
      CurrentUser = user;
      CurrentAccount = account;
      SessionID = id;
    }
    public UserModel CurrentUser 
    { 
      get => currentUser;
      set
      {
        currentUser = value;
        OnPropertyChanged(nameof(currentUser));
        UserChanged?.Invoke(this, new UserChangedEventArgs(currentUser != null));
      } 
    }

    public AccountModel CurrentAccount 
    {
      get => currentAccount;
      set
      {
        currentAccount = value;
        OnPropertyChanged(nameof(currentAccount));
      }
    }

    public Guid SessionID 
    {
      get => sessionID;
      set
      {
        sessionID = value;
        OnPropertyChanged(nameof(sessionID));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }

  public class UserChangedEventArgs : EventArgs
  {
    public bool UserExists { get; set; }
    public UserChangedEventArgs(bool userExists)
    {
      UserExists = userExists;
    }
  }

  public class UpdateCurrentViewModelCommand : ICommand
  {
    public event EventHandler CanExecuteChanged;
    private INavigator navigator;
    //private readonly SessionService sessionService = SessionService.Instance;

    public UpdateCurrentViewModelCommand(INavigator navigator)
    {
      this.navigator = navigator;
    }

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter)
    {
      if (parameter is ViewType viewType)
      {
        navigator.CurrentViewModel = viewType switch
        {
          ViewType.Home => new HomeViewModel(),
          ViewType.Account => new AccountViewModel(navigator.SessionInstance, navigator.ServiceClient),
          ViewType.UserDetails=> new UserDetailsViewModel(navigator.SessionInstance),
          ViewType.LogIn => new LoginViewModel(navigator.SessionInstance, navigator.ServiceClient),
          ViewType.SignUp => new SignUpViewModel(navigator.SessionInstance, navigator.ServiceClient),
          ViewType.Transactions => new TransactionLogsViewModel(navigator.SessionInstance, navigator.ServiceClient),
          _ => throw new NotSupportedException(),
        };
      }
    }
  }
}