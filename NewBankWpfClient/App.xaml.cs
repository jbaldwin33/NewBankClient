using System;
using System.Windows;
using NewBankServer.Protos;
using NewBankWpfClient.ViewModels;
using NewBankWpfClient.Views;
using NewBankWpfClient.Singletons;
using Grpc.Core;
using NewBankShared.Localization;

namespace NewBankWpfClient
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>

  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      Initialize();
      var window = new MainWindow { DataContext = new MainViewModel(Navigator.Instance) };
      window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
      window.Show();
      base.OnStartup(e);
    }

    private void Initialize()
    {
      new Navigator();
      ServiceClient.Instance.CreateClients();
    }

    protected override void OnExit(ExitEventArgs e)
    {
      try
      {
        if (SessionInstance.Instance.SessionID != Guid.Empty)
          ServiceClient.Instance.SessionCRUDClient.RemoveSession(new SessionRequest { SessionId = SessionInstance.Instance.SessionID.ToString() });
      }
      catch(RpcException rex)
      {
        MessageBox.Show(new ErrorOccurredTranslatable(rex.Status.Detail), new ErrorTranslatable(), MessageBoxButton.OK, MessageBoxImage.Error);
      }

      ServiceClient.Instance.DisposeClients();
      base.OnExit(e);
    }
  }
}
