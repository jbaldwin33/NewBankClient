using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using Grpc.Net.Client;
using GrpcGreeter.Protos;
using GrpcGreeterWpfClient.ViewModels;
using GrpcGreeterWpfClient.Views;
using GrpcGreeterWpfClient.ServiceClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GrpcGreeterWpfClient.Navigators;

namespace GrpcGreeterWpfClient
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>

  public partial class App : Application
  {
    INavigator navigator;
    protected override void OnStartup(StartupEventArgs e)
    {
      Initialize();
      var window = new MainWindow { DataContext = new MainViewModel(navigator) };
      window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
      window.Show();
      base.OnStartup(e);
    }

    private void Initialize()
    {
      navigator = new Navigator(new SessionInstance(null, null, Guid.Empty), ServiceClient.Instance);
      ServiceClient.Instance.CreateClients();
      
    }

    protected override void OnExit(ExitEventArgs e)
    {
      if (navigator.SessionInstance.SessionID != Guid.Empty)
        navigator.ServiceClient.SessionCRUDClient.RemoveSession(new SessionRequest { SessionId = navigator.SessionInstance.SessionID.ToString() });
      navigator.ServiceClient.DisposeClients();
      base.OnExit(e);
    }
  }
}
