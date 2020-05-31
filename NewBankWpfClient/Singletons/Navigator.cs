using GalaSoft.MvvmLight;
using NewBankWpfClient.Models;
using NewBankWpfClient.ViewModels;
using System;
using System.ComponentModel;
using System.Windows.Input;
using NewBankShared.Localization;

namespace NewBankWpfClient.Singletons
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
    ICommand UpdateCurrentViewModelCommand { get; }
    bool IsActiveSession { get; }
  }

  public class Navigator : INavigator, INotifyPropertyChanged
  {
    private static readonly Lazy<Navigator> instance = new Lazy<Navigator>(() => new Navigator());
    public static Navigator Instance => instance.Value;

    private ViewModelBase currentViewModel;
    private bool isActiveSession;

    public Navigator()
    {
      SessionInstance.Instance.UserChanged += UserChanged;
    }

    #region Properties
    public string HomeLabel => new HomeLabelTranslatable();
    public string AccountLabel => new AccountLabelTranslatable();
    public string UserDetailsLabel => new UserDetailsLabelTranslatable();
    public string LoginLabel => new LoginLabelTranslatable();
    public string SignUpLabel => new SignUpLabelTranslatable();
    public string TransactionsLabel => new TransactionsLabelTranslatable();

    public ViewModelBase CurrentViewModel
    {
      get => currentViewModel;
      set
      {
        currentViewModel = value;
        OnPropertyChanged(nameof(CurrentViewModel));
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

    #endregion

    private void UserChanged(object sender, UserChangedEventArgs e) => IsActiveSession = e.UserExists;

    public ICommand UpdateCurrentViewModelCommand => new UpdateCurrentViewModelCommand(this);

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
          ViewType.Account => new AccountViewModel(),
          ViewType.UserDetails=> new UserDetailsViewModel(),
          ViewType.LogIn => new LoginViewModel(),
          ViewType.SignUp => new SignUpViewModel(),
          ViewType.Transactions => new TransactionLogsViewModel(),
          _ => throw new NotSupportedException(),
        };
      }
    }
  }
}