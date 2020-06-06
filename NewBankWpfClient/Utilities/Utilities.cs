using GalaSoft.MvvmLight;
using NewBankWpfClient.Singletons;
using NewBankWpfClient.ViewModels;
using System;

namespace NewBankWpfClient.Utilities
{
  public static class Utilities
  {
    public static ViewModelBase SetPropertiesOnLogout()
    {
      SessionInstance.Instance.CurrentAccount = null;
      SessionInstance.Instance.CurrentUser = null;
      SessionInstance.Instance.SessionID = Guid.Empty;
      return new LoginViewModel();
    }
  }
}
