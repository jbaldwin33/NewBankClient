using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using GrpcGreeterWpfClient.ViewModels;
using GrpcGreeterWpfClient.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GrpcGreeterWpfClient
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>

  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      var window = new MainWindow { DataContext = new MainViewModel() };
      window.Show();
      base.OnStartup(e);
    }
  }
}
