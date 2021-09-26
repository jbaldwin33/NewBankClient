﻿using Grpc.Core;
using NewBankServer.Protos;
using NewBankWpfClient.Models;
using NewBankWpfClient.Singletons;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using static NewBankWpfClient.Models.Enums;
using System.Security;
using MVVMFramework.ViewModels;
using MVVMFramework.Utilities;
using MVVMFramework.Localization;

namespace NewBankWpfClient.ViewModels
{
    public class SignUpViewModel : ViewModel
    {
        #region Props and fields

        private string firstName;
        private string lastName;
        private string username;
        private SecureString securePassword;
        private ObservableCollection<AccountTypeViewModel> accountTypes;
        private AccountEnum accountType;
        private RelayCommand signUpCommand;
        private readonly ServiceClient serviceClient = ServiceClient.Instance;
        private readonly SessionInstance sessionInstance = SessionInstance.Instance;
        private bool signUpDetailsVisible;
        
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

        public SecureString SecurePassword
        {
            get => securePassword;
            set => SetProperty(ref securePassword, value);
        }

        public ObservableCollection<AccountTypeViewModel> AccountTypes
        {
            get => accountTypes;
            set => SetProperty(ref accountTypes, value);
        }


        public AccountEnum AccountType
        {
            get => accountType;
            set => SetProperty(ref accountType, value);
        }


        public bool SignUpDetailsVisible
        {
            get => signUpDetailsVisible;
            set => SetProperty(ref signUpDetailsVisible, value);
        }

        #endregion

        #region Labels

        public string FirstNameLabel => $"{new FirstNameLabelTranslatable()}:";
        public string LastNameLabel => $"{new LastNameLabelTranslatable()}:";
        public string UsernameLabel => $"{new UsernameLabelTranslatable()}:";
        public string PasswordLabel => $"{new PasswordLabelTranslatable()}:";
        public string AccountTypeLabel => $"{new AccountTypeLabelTranslatable()}:";
        public string SignUpLabel => new SignUpLabelTranslatable();

        #endregion

        public RelayCommand SignUpCommand => signUpCommand ??= new RelayCommand(SignUpCommandExecute);

        public SignUpViewModel()
        {
            SignUpDetailsVisible = sessionInstance.CurrentUser == null;
            AccountTypes = new ObservableCollection<AccountTypeViewModel>
            {
                new AccountTypeViewModel(AccountEnum.Checking, new CheckingLabelTranslatable()),
                new AccountTypeViewModel(AccountEnum.Saving, new SavingLabelTranslatable())
            };
        }


        private void SignUpCommandExecute()
        {
            if (!ValidInput())
                return;

            try
            {
                //this should only get usernames, not passwords
                var users = serviceClient.UserCRUDClient.GetUsers(new Empty());
                if (users.Items.Any(u => u.Username == username))
                    ShowMessage(new MessageBoxEventArgs(new UsernameAlreadyExistsTranslatable(), MessageBoxEventArgs.MessageTypeEnum.Error, MessageBoxButton.OK, MessageBoxImage.Error));
                else
                {
                    DoSignUp();
                    ShowMessage(new MessageBoxEventArgs(new SignUpSuccessfulTranslatable(), MessageBoxEventArgs.MessageTypeEnum.Information, MessageBoxButton.OK, MessageBoxImage.Information));
                }
            }
            catch (RpcException rex)
            {
                ShowMessage(new MessageBoxEventArgs(new SignUpFailedErrorTranslatable(rex.Status.Detail), MessageBoxEventArgs.MessageTypeEnum.Error, MessageBoxButton.OK, MessageBoxImage.Error));
            }
        }

        private void DoSignUp()
        {
            var accountID = Guid.NewGuid();
            var userID = Guid.NewGuid();
            var serverSalt = serviceClient.CreationClient.CreatePasswordSalt(new Empty());
            var clientSalt = SecurePasswordUtility.CreateSalt();
            var combinedSalt = $"{serverSalt.ServerSalt}";
            var user = new User
            {
                Username = username,
                PasswordSalt = combinedSalt,
                PasswordHash = new SecurePasswordUtility(securePassword, combinedSalt).ComputeSaltedHash(),
                FirstName = firstName,
                LastName = lastName,
                Id = userID.ToString(),
                AccountId = accountID.ToString(),
                UserType = UserProtoEnum.User
            };
            var account = new Account
            {
                Id = accountID.ToString(),
                UserId = userID.ToString(),
                Balance = 0.0,
                AccountType = AccountModel.ConvertAccountType(AccountType)
            };

            serviceClient.CreationClient.SignUp(new SignUpRequest { Account = account, User = user });
        }

        private bool ValidInput()
        {
            Translatable errorMessage = null;

            if (string.IsNullOrEmpty(firstName))
                errorMessage = new FirstNameEmptyTranslatable();
            else if (string.IsNullOrEmpty(lastName))
                errorMessage = new LastNameEmptyTranslatable();
            else if (string.IsNullOrEmpty(username))
                errorMessage = new UsernameEmptyTranslatable();
            else if (securePassword == null)
                errorMessage = new PasswordEmptyTranslatable();

            if (errorMessage == null)
                return true;

            ShowMessage(new MessageBoxEventArgs(errorMessage, MessageBoxEventArgs.MessageTypeEnum.Error, MessageBoxButton.OK, MessageBoxImage.Error));
            return false;
        }
    }

    public class AccountTypeViewModel
    {
        public AccountEnum AccType { get; set; }
        public string Name { get; set; }

        public AccountTypeViewModel(AccountEnum accountType, string name)
        {
            AccType = accountType;
            Name = name;
        }
    }
}
