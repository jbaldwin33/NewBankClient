using Grpc.Core;
using MVVMFramework.Localization;
using MVVMFramework.ViewModels;
using NewBankServer.Protos;
using NewBankWpfClient.Models;
using NewBankWpfClient.Singletons;
using NewBankWpfClient.Views;
using System;
using System.Windows;
using static NewBankWpfClient.Models.Enums;
using static NewBankWpfClient.Utilities.Utilities;

namespace NewBankWpfClient.ViewModels
{
    public enum CommandEnum
    {
        Deposit,
        Withdraw,
        Transfer
    }

    public class AccountViewModel : ViewModel
    {
        #region Props and fields

        private readonly ServiceClient serviceClient = ServiceClient.Instance;
        private readonly SessionInstance sessionInstance = SessionInstance.Instance;
        private double balance;
        private AccountEnum accountType;
        private bool detailsVisible;
        private RelayCommand depositCommand;
        private RelayCommand withdrawCommand;
        private RelayCommand transferCommand;

        public event EventHandler OnModelDialogFinished;

        public double Balance
        {
            get => balance;
            set => SetProperty(ref balance, value);
        }

        public AccountEnum AccountType
        {
            get => accountType;
            set => SetProperty(ref accountType, value);
        }

        public bool DetailsVisible
        {
            get => detailsVisible;
            set => SetProperty(ref detailsVisible, value);
        }

        #endregion

        #region Labels
        public string AccountTypeLabel => $"{new AccountTypeLabelTranslatable()}:";
        public string BalanceLabel => $"{new BalanceLabelTranslatable()}:";
        public string DepositLabel => new DepositCommandTranslatable();
        public string WithdrawLabel => new WithdrawCommandTranslatable();
        public string TransferLabel => new TransferCommandTranslatable();

        #endregion

        #region Commands

        public RelayCommand DepositCommand => depositCommand ??= new RelayCommand(() => CommandExecute(CommandEnum.Deposit), () => true);
        public RelayCommand WithdrawCommand => withdrawCommand ??= new RelayCommand(() => CommandExecute(CommandEnum.Withdraw), () => Balance > 0);
        public RelayCommand TransferCommand => transferCommand ??= new RelayCommand(() => CommandExecute(CommandEnum.Transfer), () => Balance > 0);

        #endregion

        public AccountViewModel() { }

        public override void OnLoaded()
        {
            SetVisibility();
            base.OnLoaded();
        }

        private void SetVisibility()
        {
            try
            {
                DetailsVisible = serviceClient.SessionCRUDClient.IsValidSession(new SessionRequest { SessionId = sessionInstance.SessionID.ToString() }).Valid;
            }
            catch (RpcException rex)
            {
                DetailsVisible = false;
                ShowMessage(new MessageBoxEventArgs(new BankErrorOccurredTranslatable(rex.Status.Detail), MessageBoxEventArgs.MessageTypeEnum.Error, MessageBoxButton.OK, MessageBoxImage.Error));
            }
            if (DetailsVisible)
                UpdateAccountDetails();
        }

        private void UpdateAccountDetails()
        {
            try
            {
                var account = serviceClient.AccountCRUDClient.GetByUserID(new AccountRequest { UserId = sessionInstance.CurrentUser.ID.ToString(), SessionId = sessionInstance.SessionID.ToString() }).Account;
                Balance = account.Balance;
                AccountType = AccountModel.ConvertAccountType(account.AccountType);
            }
            catch (RpcException rex)
            {
                ShowMessage(new MessageBoxEventArgs(new FailedToGetAccountDetailsErrorTranslatable(rex.Status.Detail), MessageBoxEventArgs.MessageTypeEnum.Error, MessageBoxButton.OK, MessageBoxImage.Error));
            }
        }

        private void CommandExecute(object commandType)
        {
            var vm = new AccountCommandsViewModel((CommandEnum)commandType, balance);
            vm.OnFinishEventHandler += ModalDialogClosed;
            var commandView = new AccountCommandView(vm, this) { WindowStartupLocation = WindowStartupLocation.CenterScreen };
            commandView.Show();
        }

        private void ModalDialogClosed(object sender, WindowPopupEventArgs e)
        {
            var failed = false;
            if (e.Amount < 1)
            {
                failed = true;
                ShowMessage(new MessageBoxEventArgs(new AmountGreaterThanZeroTranslatable(), MessageBoxEventArgs.MessageTypeEnum.Error, MessageBoxButton.OK, MessageBoxImage.Error));
            }
            else
            {
                switch (e.CommandType)
                {
                    case CommandEnum.Deposit:
                        try
                        {
                            serviceClient.AccountCRUDClient.Deposit(new DepositRequest
                            {
                                AccountId = sessionInstance.CurrentUser.AccountID.ToString(),
                                Amount = e.Amount,
                                SessionId = sessionInstance.SessionID.ToString()
                            });
                            Balance += e.Amount;
                        }
                        catch (RpcException rex) when (rex.Status.StatusCode == StatusCode.PermissionDenied)
                        {
                            GlobalExceptionHandler.ProcessException(rex);
                            SetPropertiesOnLogout();
                        }
                        catch (RpcException rex)
                        {
                            ShowMessage(new MessageBoxEventArgs(new OperationFailedErrorTranslatable(rex.Status.Detail), MessageBoxEventArgs.MessageTypeEnum.Error, MessageBoxButton.OK, MessageBoxImage.Error));
                        }
                        break;
                    case CommandEnum.Withdraw:
                        try
                        {
                            serviceClient.AccountCRUDClient.Withdraw(new WithdrawRequest
                            {
                                AccountId = sessionInstance.CurrentUser.AccountID.ToString(),
                                Amount = e.Amount,
                                SessionId = sessionInstance.SessionID.ToString()
                            });
                            Balance -= e.Amount;
                        }
                        catch (RpcException rex) when (rex.Status.StatusCode == StatusCode.PermissionDenied)
                        {
                            ShowMessage(new MessageBoxEventArgs(new SessionInvalidLoggingOutTranslatable(), MessageBoxEventArgs.MessageTypeEnum.Error, MessageBoxButton.OK, MessageBoxImage.Error));
                            SetPropertiesOnLogout();
                        }
                        catch (RpcException rex)
                        {
                            ShowMessage(new MessageBoxEventArgs(new OperationFailedErrorTranslatable(rex.Status.Detail), MessageBoxEventArgs.MessageTypeEnum.Error, MessageBoxButton.OK, MessageBoxImage.Error));
                        }
                        break;
                    case CommandEnum.Transfer:
                        try
                        {
                            if (string.IsNullOrEmpty(e.Username))
                            {
                                failed = true;
                                ShowMessage(new MessageBoxEventArgs(new EnterUserForTransferTranslatable(), MessageBoxEventArgs.MessageTypeEnum.Error, MessageBoxButton.OK, MessageBoxImage.Error));
                            }
                            else
                            {
                                serviceClient.AccountCRUDClient.Transfer(new TransferRequest
                                {
                                    Amount = e.Amount,
                                    ToUsername = e.Username,
                                    FromUsername = sessionInstance.CurrentUser.Username,
                                    SessionId = sessionInstance.SessionID.ToString()
                                });
                                Balance -= e.Amount;
                            }
                        }
                        catch (RpcException rex) when (rex.Status.StatusCode == StatusCode.PermissionDenied)
                        {
                            ShowMessage(new MessageBoxEventArgs(new SessionInvalidLoggingOutTranslatable(), MessageBoxEventArgs.MessageTypeEnum.Error, MessageBoxButton.OK, MessageBoxImage.Error));
                            SetPropertiesOnLogout();
                        }
                        catch (RpcException rex)
                        {
                            failed = true;
                            ShowMessage(new MessageBoxEventArgs(new TransferFailedErrorTranslatable(rex.Status.Detail), MessageBoxEventArgs.MessageTypeEnum.Error, MessageBoxButton.OK, MessageBoxImage.Error));
                        }
                        break;
                    default:
                        throw new NotSupportedException($"{e.CommandType} is not supported");
                }
            }

            if (!failed)
                OnModelDialogFinished?.Invoke(this, new EventArgs());
        }
    }
}
