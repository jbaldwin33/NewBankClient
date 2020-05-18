using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;
using GrpcGreeter.Protos;
using Microsoft.Extensions.DependencyInjection;

namespace GrpcGreeterWpfClient.ViewModels
{
  public class HomeViewModel : ViewModelBase
  {
    private UserCRUD.UserCRUDClient client;

    public HomeViewModel()
    {

    }

    public HomeViewModel(UserCRUD.UserCRUDClient client)
    {
      this.client = client;
    }
  }
}
