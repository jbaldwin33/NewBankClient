using GalaSoft.MvvmLight;
using GrpcGreeter.Protos;
using GrpcGreeterWpfClient.Models;
using GrpcGreeterWpfClient.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace GrpcGreeterWpfClient.Navigators
{
  public enum ViewType
  {
    Home,
    Account,
    LogIn,
    SignUp
  }
  public interface INavigator
  {
    UserCRUD.UserCRUDClient UserCRUDClient { get; }
    AccountCRUD.AccountCRUDClient AccountCRUDClient { get; }
    ViewModelBase CurrentViewModel { get; set; }
    UserModel CurrentUser { get; set; }
    ICommand UpdateCurrentViewModelCommand { get; }
    //ICommand UpdateCurrentUserCommand { get; }
  }
  public class Navigator : INavigator, INotifyPropertyChanged
  {
    private UserCRUD.UserCRUDClient userCRUDClient;
    public UserCRUD.UserCRUDClient UserCRUDClient
    {
      get => userCRUDClient;
      private set
      {
        userCRUDClient = value;
        OnPropertyChanged(nameof(UserCRUDClient));
      }
    }

    private AccountCRUD.AccountCRUDClient accountCRUDClient;
    public AccountCRUD.AccountCRUDClient AccountCRUDClient
    {
      get => accountCRUDClient;
      private set
      {
        accountCRUDClient = value;
        OnPropertyChanged(nameof(accountCRUDClient));
      }
    }

    private ViewModelBase currentViewModel;
    public ViewModelBase CurrentViewModel
    {
      get => currentViewModel;
      set
      {
        currentViewModel = value;
        OnPropertyChanged(nameof(CurrentViewModel));
      }
    }

    private UserModel currentUser;
    public UserModel CurrentUser
    {
      get => currentUser;
      set
      {
        currentUser = value;
        OnPropertyChanged(nameof(currentUser));
      }
    }

    public Navigator(UserCRUD.UserCRUDClient userCRUDClient, AccountCRUD.AccountCRUDClient accountCRUDClient)
    {
      UserCRUDClient = userCRUDClient;
      AccountCRUDClient = accountCRUDClient;
    }

    public ICommand UpdateCurrentViewModelCommand => new UpdateCurrentViewModelCommand(this);

    //public ICommand UpdateCurrentUserCommand => new UpdateCurrentUserCommand(this);
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
  public class UpdateCurrentViewModelCommand : ICommand
  {
    public event EventHandler CanExecuteChanged;
    private INavigator navigator;

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
          ViewType.Home => new HomeViewModel(navigator.UserCRUDClient),
          ViewType.Account => new AccountViewModel(navigator.UserCRUDClient, navigator.CurrentUser),
          ViewType.LogIn => new LoginViewModel(navigator, navigator.UserCRUDClient, navigator.CurrentUser),
          ViewType.SignUp => new SignUpViewModel(navigator.UserCRUDClient, navigator.AccountCRUDClient, navigator.CurrentUser),
          _ => throw new NotSupportedException(),
        };
      }
    }
  }

  //public class UpdateCurrentUserCommand : ICommand
  //{
  //  public event EventHandler CanExecuteChanged;
  //  private INavigator navigator;

  //  public UpdateCurrentUserCommand(INavigator navigator)
  //  {
  //    this.navigator = navigator;
  //  }

  //  public bool CanExecute(object parameter) => true;

  //  public void Execute(object parameter)
  //  {
  //    navigator.CurrentUser = parameter is UserModel user
  //      ? user
  //      : null;
  //  }
  //}
}