using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;
using NewBankServer.Protos;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Globalization;
using NewBankWpfClient.Properties;
using NewBankClientGrpc;

namespace NewBankWpfClient.ViewModels
{
  public class HomeViewModel : ViewModelBase
  {
    public string WelcomeText { get; set; }
    public HomeViewModel()
    {
        WelcomeText = new WelcomeTranslatable();
    }
  }
}
