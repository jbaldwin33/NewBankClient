using System;
using System.Windows;
using NewBankServer.Protos;
using NewBankWpfClient.ViewModels;
using NewBankWpfClient.Singletons;
using Grpc.Core;
using MVVMFramework.Localization;
using MVVMFramework.ViewModels;
using MVVMFramework.Views;
using MVVMFramework.ViewNavigator;

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
            var types = new (Type, string, bool)[]
            {
                (typeof(HomeViewModel), new HomeLabelTranslatable(), true),
                (typeof(AccountViewModel), new AccountLabelTranslatable(), false),
                (typeof(UserDetailsViewModel), new UserDetailsLabelTranslatable(), false),
                (typeof(LoginViewModel), new LoginLabelTranslatable(), true),
                (typeof(SignUpViewModel), new SignUpLabelTranslatable(), true),
                (typeof(TransactionLogsViewModel), new TransactionsLabelTranslatable(), false),
            };

            var window = new BaseWindowView(types);
            window.Show();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                if (SessionInstance.Instance.SessionID != Guid.Empty)
                    ServiceClient.Instance.SessionCRUDClient.RemoveSession(new SessionRequest { SessionId = SessionInstance.Instance.SessionID.ToString() });
            }
            catch (RpcException rex)
            {
                MessageBox.Show(new BankErrorOccurredTranslatable(rex.Status.Detail), new ErrorTranslatable(), MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ServiceClient.Instance.DisposeClients();
            base.OnExit(e);
        }

        private void Initialize()
        {
            SessionInstance.Instance.UserChanged += UserChanged;
            ServiceClient.Instance.CreateClients();
            Navigator.Instance.BeforeUpdate += Instance_BeforeUpdate;
            Navigator.Instance.AfterUpdate += Instance_AfterUpdate;
        }

        private bool Instance_BeforeUpdate()
        {
            if (SessionInstance.Instance.SessionID == Guid.Empty)
                return true;

            var validSession = false;
            try
            {
                validSession = ServiceClient.Instance.SessionCRUDClient.IsValidSession(
                  new SessionRequest
                  {
                      SessionId = SessionInstance.Instance.SessionID.ToString()
                  }).Valid;
            }
            catch (RpcException rex)
            {
                validSession = false;
                //something happened and session is not valid
            }
            finally
            {
                SessionInstance.Instance.IsActiveSession = validSession;
            }
            return validSession;
        }
        private void Instance_AfterUpdate()
        {
            if (!SessionInstance.Instance.IsActiveSession)
                InvalidSession();
        }

        private void InvalidSession()
        {
            MessageBox.Show(new SessionInvalidLoggingOutTranslatable(), new ErrorTranslatable(), MessageBoxButton.OK, MessageBoxImage.Error);
            Utilities.Utilities.SetPropertiesOnLogout();
        }

        private void UserChanged(object sender, UserChangedEventArgs e) => SessionInstance.Instance.IsActiveSession = e.UserExists;

        public class UserChangedEventArgs : EventArgs
        {
            public bool UserExists { get; set; }
            public UserChangedEventArgs(bool userExists)
            {
                UserExists = userExists;
            }
        }
    }
}
