using Grpc.Core;
using MVVMFramework.Localization;
using MVVMFramework.ViewModels;
using NewBankServer.Protos;
using NewBankWpfClient.Singletons;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using static NewBankWpfClient.Utilities.Utilities;

namespace NewBankWpfClient.ViewModels
{
    public class TransactionLogsViewModel : ViewModel
    {
        #region Props and fields

        private ObservableCollection<TransactionViewModel> transactions;
        private CollectionView collectionView;
        private string sortedProp;
        private ListSortDirection sortDirection;
        private readonly SessionInstance sessionInstance = SessionInstance.Instance;
        private readonly ServiceClient serviceClient = ServiceClient.Instance;

        public ObservableCollection<TransactionViewModel> Transactions
        {
            get => transactions;
            set => SetProperty(ref transactions, value);
        }

        public CollectionView CollectionView
        {
            get
            {
                collectionView = (CollectionView)CollectionViewSource.GetDefaultView(Transactions);
                if (collectionView != null)
                    collectionView.SortDescriptions.Add(new SortDescription(sortedProp, sortDirection));
                return collectionView;
            }
        }

        public string SortedProp
        {
            get => sortedProp;
            set => SetProperty(ref sortedProp, value);
        }

        public ListSortDirection SortDirection
        {
            get => sortDirection;
            set => SetProperty(ref sortDirection, value);
        }

        #endregion

        #region Labels
        public string DateHeader => "Transaction date";
        public string MessageHeader => "Transaction details";
        public string AmountHeader => "Amount";
        public string TypeHeader => "Transaction type";
        public Translatable RecentTransactionsLabel => new RecentTransactionsLabelTranslatable();

        #endregion

        public TransactionLogsViewModel() { }

        public override void OnLoaded()
        {
            SortDirection = ListSortDirection.Descending;
            SortedProp = nameof(TransactionViewModel.CreatedDateTime);
            Transactions = new ObservableCollection<TransactionViewModel>();
            GetTransactions();
            base.OnLoaded();
        }

        public async Task GetTransactions()
        {
            try
            {
                using var logs = serviceClient.TransactionCRUDClient.GetAllUserTransactions(new GetAllUserTransactionsRequest
                {
                    UserId = sessionInstance.CurrentUser.ID.ToString(),
                    SessionId = sessionInstance.SessionID.ToString()
                });
                await foreach (var transaction in logs.ResponseStream.ReadAllAsync())
                    Transactions.Add(new TransactionViewModel(transaction.TransactionCreatedTime.ToDateTime(), transaction.Amount, transaction.TransactionType.ToString(), transaction.Message));
            }
            catch (RpcException rex) when (rex.Status.StatusCode == StatusCode.PermissionDenied)
            {
                ShowMessage(new MessageBoxEventArgs(new SessionInvalidLoggingOutTranslatable(), MessageBoxEventArgs.MessageTypeEnum.Error, MessageBoxButton.OK, MessageBoxImage.Error));
                SetPropertiesOnLogout();
            }
            catch (RpcException rex)
            {
                
                ShowMessage(new MessageBoxEventArgs(new FailedToGetTransactionsErrorTranslatable(rex.Status.Detail), MessageBoxEventArgs.MessageTypeEnum.Error, MessageBoxButton.OK, MessageBoxImage.Error));
            }
        }
    }
}
