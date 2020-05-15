using GalaSoft.MvvmLight;
using GrpcGreeterWpfClient.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

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
    ViewModelBase CurrentViewModel { get; set; }
    ICommand UpdateCurrentViewModelCommand { get; }
  }
  public class Navigator : INavigator, INotifyPropertyChanged
  {
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
    public ICommand UpdateCurrentViewModelCommand => new UpdateCurrentViewModelCommand(this);

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
        switch (viewType)
        {
          case ViewType.Home:
            navigator.CurrentViewModel = new HomeViewModel();
            break;
          case ViewType.Account:
            navigator.CurrentViewModel = new AccountViewModel();
            break;
          case ViewType.LogIn:
            navigator.CurrentViewModel = new LoginViewModel();
            break;
          case ViewType.SignUp:
            navigator.CurrentViewModel = new SignUpViewModel();
            break;
          default:
            throw new NotSupportedException();
        }
      }
    }
  }
}