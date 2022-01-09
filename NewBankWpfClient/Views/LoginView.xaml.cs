using MVVMFramework.Utilities;
using MVVMFramework.ViewNavigator;
using MVVMFramework.Views;
using NewBankWpfClient.Utilities;
using NewBankWpfClient.ViewModels;
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

namespace NewBankWpfClient.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : ViewBaseControl
    {
        private LoginViewModel vm;
        public LoginView() : base(Navigator.Instance.CurrentViewModel)
        {
            InitializeComponent();
            vm = Navigator.Instance.CurrentViewModel as LoginViewModel;
            vm.LogoutEventHandler += Vm_LogoutEventHandler;
        }

        private void Vm_LogoutEventHandler(object sender, EventArgs e)
        {
            pBox.Clear();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBoxMVVMAttachedProperties.SetEncryptedPassword(pBox, pBox.SecurePassword);
        }

        private void MediumTextBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
