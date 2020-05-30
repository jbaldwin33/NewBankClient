using System;
using System.Windows;
using NewBankServer.Protos;
using NewBankWpfClient.ViewModels;
using NewBankWpfClient.Views;
using NewBankWpfClient.ServiceClients;
using NewBankWpfClient.Navigators;
using Grpc.Core;
using NewBankShared.Localization;

namespace NewBankWpfClient
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
      try
      {
        if (navigator.SessionInstance.SessionID != Guid.Empty)
          navigator.ServiceClient.SessionCRUDClient.RemoveSession(new SessionRequest { SessionId = navigator.SessionInstance.SessionID.ToString() });
      }
      catch(RpcException rex)
      {
        MessageBox.Show(new ErrorOccurredTranslatable(rex.Status.Detail), new ErrorTranslatable(), MessageBoxButton.OK, MessageBoxImage.Error);
      }

      navigator.ServiceClient.DisposeClients();
      base.OnExit(e);
    }
  }
}
