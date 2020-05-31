using NewBankWpfClient.Singletons;
using System;

namespace NewBankWpfClient.Utilities
{
  public static class Utilities
  {
    public static void SetPropertiesOnLogout()
    {
      SessionInstance.Instance.CurrentAccount = null;
      SessionInstance.Instance.CurrentUser = null;
      SessionInstance.Instance.SessionID = Guid.Empty;
      Navigator.Instance.UpdateCurrentViewModelCommand.Execute(ViewType.LogIn);
    }
  }
}
