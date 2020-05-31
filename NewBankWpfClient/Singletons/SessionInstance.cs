using NewBankWpfClient.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NewBankWpfClient.Singletons
{
  public class SessionInstance : INotifyPropertyChanged
  {
    private static readonly Lazy<SessionInstance> instance = new Lazy<SessionInstance>(() => new SessionInstance());
    public static SessionInstance Instance => instance.Value;

    private UserModel currentUser;
    private AccountModel currentAccount;
    private Guid sessionID;
    public event EventHandler<UserChangedEventArgs> UserChanged;
    public SessionInstance() { }
    public UserModel CurrentUser
    {
      get => currentUser;
      set
      {
        currentUser = value;
        OnPropertyChanged(nameof(currentUser));
        UserChanged?.Invoke(this, new UserChangedEventArgs(currentUser != null));
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

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }
}
