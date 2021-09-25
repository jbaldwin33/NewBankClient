using MVVMFramework.Localization;
using MVVMFramework.ViewModels;

namespace NewBankWpfClient.ViewModels
{
    public class HomeViewModel : ViewModel
    {
        public string WelcomeText { get; set; }
        public HomeViewModel()
        {
            WelcomeText = new WelcomeTranslatable();
        }
    }
}
