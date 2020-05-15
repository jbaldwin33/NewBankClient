using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GrpcGreeterWpfClient.Navigators;
using GrpcGreeterWpfClient.Views;
using Microsoft.Extensions.Options;

namespace GrpcGreeterWpfClient.ViewModels
{
  public class MainViewModel : ViewModelBase
  {
    public INavigator Navigator { get; set; } = new Navigator();

    public MainViewModel()
    {
   
    }
  }
}
