using GalaSoft.MvvmLight;
using GrpcGreeterWpfClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GrpcGreeterWpfClient.Views
{
  /// <summary>
  /// Interaction logic for AccountCommandView.xaml
  /// </summary>
  public partial class AccountCommandView : Window
  {
    private readonly AccountViewModel parentViewModel;
    public AccountCommandView(AccountCommandsViewModel viewModel, AccountViewModel parentViewModel)
    {
      InitializeComponent();
      viewModel.OnCancelledEventHandler += CancelledEventHandler;
      DataContext = viewModel;
      this.parentViewModel = parentViewModel;
      this.parentViewModel.OnModelDialogFinished += OnModelDialogFinished;
    }

    private void OnModelDialogFinished(object sender, EventArgs e) => Close();

    private void OnFinishEventHandler(object sender, WindowPopupEventArgs e) => Close();

    private void CancelledEventHandler(object sender, EventArgs e) => Close();

  }
}
