using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GrpcGreeter.Protos;
using GrpcGreeterWpfClient.Navigators;
using GrpcGreeterWpfClient.Views;
using Microsoft.Extensions.Options;

namespace GrpcGreeterWpfClient.ViewModels
{
  public class MainViewModel : ViewModelBase
  {
    private readonly UserCRUD.UserCRUDClient userCRUDClient;
    private readonly AccountCRUD.AccountCRUDClient accountCRUDClient;
    public INavigator Navigator { get; set; }

    public MainViewModel(UserCRUD.UserCRUDClient userCRUDClient, AccountCRUD.AccountCRUDClient accountCRUDClient)
    {
      this.userCRUDClient = userCRUDClient;
      this.accountCRUDClient = accountCRUDClient;
      Navigator = new Navigator(userCRUDClient, accountCRUDClient);
    }
  }
}
