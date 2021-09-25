using MVVMFramework.ViewNavigator;
using NewBankWpfClient.Models;
using NewBankWpfClient.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NewBankWpfClient.Singletons
{
    public class SessionInstance : INotifyPropertyChanged
    {
        private static readonly Lazy<SessionInstance> instance = new(() => new SessionInstance());
        public static SessionInstance Instance => instance.Value;

        private UserModel currentUser;
        private AccountModel currentAccount;
        private Guid sessionID;
        private bool isActiveSession;
        public event EventHandler<App.UserChangedEventArgs> UserChanged;
        public SessionInstance() { }
        public UserModel CurrentUser
        {
            get => currentUser;
            set
            {
                currentUser = value;
                OnPropertyChanged(nameof(currentUser));
                UserChanged?.Invoke(this, new App.UserChangedEventArgs(currentUser != null));
            }
        }

        public AccountModel CurrentAccount
        {
            get => currentAccount;
            set
            {
                currentAccount = value;
                OnPropertyChanged(nameof(currentAccount));
            }
        }

        public Guid SessionID
        {
            get => sessionID;
            set
            {
                sessionID = value;
                OnPropertyChanged(nameof(sessionID));
            }
        }

        public bool IsActiveSession
        {
            get => isActiveSession;
            set
            {
                isActiveSession = value;
                OnPropertyChanged(nameof(isActiveSession));
                Navigator.Instance.SetButtonVisibility(typeof(AccountViewModel), value);
                Navigator.Instance.SetButtonVisibility(typeof(UserDetailsViewModel), value);
                Navigator.Instance.SetButtonVisibility(typeof(TransactionLogsViewModel), value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
