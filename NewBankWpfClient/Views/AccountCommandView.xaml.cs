using NewBankWpfClient.ViewModels;
using System;
using System.Windows;

namespace NewBankWpfClient.Views
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
