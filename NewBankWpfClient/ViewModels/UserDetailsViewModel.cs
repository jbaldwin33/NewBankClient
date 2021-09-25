using MVVMFramework.Localization;
using MVVMFramework.ViewModels;
using NewBankWpfClient.Singletons;

namespace NewBankWpfClient.ViewModels
{
    public class UserDetailsViewModel : ViewModel
    {
        private readonly SessionInstance sessionInstance = SessionInstance.Instance;
        private string firstName;
        private string lastName;
        private string username;
        private string accountType;
        private bool detailsVisible;

        public UserDetailsViewModel()
        {
            
        }

        public override void OnLoaded()
        {
            DetailsVisible = sessionInstance.CurrentUser != null;
            if (sessionInstance.CurrentUser != null)
                UpdateUserDetails();
            else
                ClearDetails();
            base.OnLoaded();
        }

        public string FirstNameLabel => $"{new FirstNameLabelTranslatable()}:";
        public string LastNameLabel => $"{new LastNameLabelTranslatable()}:";
        public string UsernameLabel => $"{new UsernameLabelTranslatable()}:";
        public string AccountTypeLabel => $"{new AccountTypeLabelTranslatable()}:";

        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }


        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        public string AccountType
        {
            get => accountType;
            set => SetProperty(ref accountType, value);
        }

        public bool DetailsVisible
        {
            get => detailsVisible;
            set => SetProperty(ref detailsVisible, value);
        }

        private void UpdateUserDetails()
        {
            FirstName = sessionInstance.CurrentUser.FirstName;
            LastName = sessionInstance.CurrentUser.LastName;
            Username = sessionInstance.CurrentUser.Username;
            AccountType = sessionInstance.CurrentAccount?.AccountType.ToString();
        }

        private void ClearDetails()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Username = string.Empty;
            AccountType = string.Empty;
        }
    }
}
