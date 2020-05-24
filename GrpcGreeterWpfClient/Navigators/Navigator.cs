﻿//using BankServer.Services;
using GalaSoft.MvvmLight;
using GrpcGreeter.Protos;
using GrpcGreeterWpfClient.Models;
using GrpcGreeterWpfClient.ServiceClients;
using GrpcGreeterWpfClient.ViewModels;
using Microsoft.AspNetCore.Identity;
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
    UserDetails,
    LogIn,
    SignUp
  }
  public interface INavigator
  {
    ViewModelBase CurrentViewModel { get; set; }
    SessionInstance SessionInstance { get; set; }
    ServiceClient ServiceClient { get; set; }
    ICommand UpdateCurrentViewModelCommand { get; }
  }
  public class Navigator : INavigator, INotifyPropertyChanged
  {
    private ViewModelBase currentViewModel;
    private SessionInstance sessionInstance;
    private ServiceClient serviceClient;

    public ViewModelBase CurrentViewModel
    {
      get => currentViewModel;
      set
      {
        currentViewModel = value;
        OnPropertyChanged(nameof(CurrentViewModel));
      }
    }

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


    public Navigator(SessionInstance sessionInstance, ServiceClient serviceClient)
    {
      SessionInstance = sessionInstance;
      ServiceClient = serviceClient;
    }

    public ICommand UpdateCurrentViewModelCommand => new UpdateCurrentViewModelCommand(this);

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }

  public class SessionInstance
  {
    public UserModel CurrentUser { get; set; }
    public AccountModel CurrentAccount { get; set; }

    public Guid SessionID { get; set; }

    public SessionInstance(UserModel user, AccountModel account, Guid id)
    {
      CurrentUser = user;
      CurrentAccount = account;
      SessionID = id;
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
          ViewType.UserDetails=> new UserDetailsViewModel(navigator.SessionInstance, navigator.ServiceClient),
          ViewType.LogIn => new LoginViewModel(navigator.SessionInstance, navigator.ServiceClient),
          ViewType.SignUp => new SignUpViewModel(navigator.SessionInstance, navigator.ServiceClient),
          _ => throw new NotSupportedException(),
        };
      }
    }
  }
}