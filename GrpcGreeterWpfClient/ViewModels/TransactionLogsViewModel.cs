using GalaSoft.MvvmLight;
using Grpc.Core;
using GrpcGreeter.Protos;
using GrpcGreeterWpfClient.Navigators;
using GrpcGreeterWpfClient.ServiceClients;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GrpcGreeterWpfClient.ViewModels
{
  public class TransactionLogsViewModel : ViewModelBase
  {
    private ObservableCollection<TransactionViewModel> transactions;
    private readonly SessionInstance sessionInstance;
    private readonly ServiceClient serviceClient;

    public event EventHandler OnShown;
    public TransactionLogsViewModel(SessionInstance sessionInstance, ServiceClient serviceClient)
    {
      this.sessionInstance = sessionInstance;
      this.serviceClient = serviceClient;
      Transactions = new ObservableCollection<TransactionViewModel>();
    }

    public ObservableCollection<TransactionViewModel> Transactions
    {
      get => transactions;
      set => Set(ref transactions, value);
    }

    public async Task GetTransactions()
    {
      var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(20));
      using var logs = serviceClient.TransactionCRUDClient.GetAllUserTransactions(new GetAllUserTransactionsRequest { UserId = sessionInstance.CurrentUser.ID.ToString() }, cancellationToken: tokenSource.Token);
      try
      {
        await foreach (var transaction in logs.ResponseStream.ReadAllAsync(tokenSource.Token))
          Transactions.Add(new TransactionViewModel(transaction.TransactionCreatedTime.ToDateTime(), transaction.Amount, transaction.TransactionType.ToString(), transaction.Message));
      }
      catch (RpcException rex)
      {
        MessageBox.Show($"Failed to get transactions: {rex.Status.Detail}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }
  }
}
