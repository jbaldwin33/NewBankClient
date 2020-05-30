using GalaSoft.MvvmLight;
using NewBankShared.Localization;

namespace NewBankWpfClient.ViewModels
{
  public class HomeViewModel : ViewModelBase
  {
    public string WelcomeText { get; set; }
    public HomeViewModel()
    {
        WelcomeText = new WelcomeTranslatable();
    }
  }
}
