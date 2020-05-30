using NewBankWpfClient.Navigators;
using System;

namespace NewBankWpfClient.Utilities
{
  public static class Utilities
  {
    public static void SetPropertiesOnLogout(SessionInstance sessionInstance)
    {
      sessionInstance.CurrentAccount = null;
      sessionInstance.CurrentUser = null;
      sessionInstance.SessionID = Guid.Empty;
    }
  }
}
