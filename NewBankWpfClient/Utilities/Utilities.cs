using MVVMFramework.ViewModels;
using MVVMFramework.ViewNavigator;
using NewBankWpfClient.Singletons;
using NewBankWpfClient.ViewModels;
using System;
using System.Linq;

namespace NewBankWpfClient.Utilities
{
    public static class Utilities
    {
        public static void SetPropertiesOnLogout()
        {
            SessionInstance.Instance.CurrentAccount = null;
            SessionInstance.Instance.CurrentUser = null;
            SessionInstance.Instance.SessionID = Guid.Empty;
            var vm = Navigator.Instance.ViewModels.First(vm => vm.GetType() == typeof(LoginViewModel)) as LoginViewModel;
            vm.LoggedIn = false;
            Navigator.Instance.UpdateCurrentViewModelCommand.Execute(vm);
        }

        public static void SetPropertiesAndNavigateToLogInPage()
        {
            SessionInstance.Instance.CurrentAccount = null;
            SessionInstance.Instance.CurrentUser = null;
            SessionInstance.Instance.SessionID = Guid.Empty;
            Navigator.Instance.UpdateCurrentViewModelCommand.Execute(Navigator.Instance.CurrentViewModel);
        }
    }
}
