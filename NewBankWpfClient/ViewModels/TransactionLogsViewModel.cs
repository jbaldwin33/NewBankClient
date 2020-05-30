﻿using GalaSoft.MvvmLight;
using Grpc.Core;
using NewBankServer.Protos;
using NewBankShared.Localization;
using NewBankWpfClient.Navigators;
using NewBankWpfClient.ServiceClients;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using static NewBankWpfClient.Utilities.Utilities;

namespace NewBankWpfClient.ViewModels
{
  public class TransactionLogsViewModel : ViewModelBase
  {
    private ObservableCollection<TransactionViewModel> transactions;
    private readonly SessionInstance sessionInstance;
    private readonly ServiceClient serviceClient;

    public TransactionLogsViewModel(SessionInstance sessionInstance, ServiceClient serviceClient)
    {
      this.sessionInstance = sessionInstance;
      this.serviceClient = serviceClient;
      SortDirection = ListSortDirection.Descending;
      SortedProp = nameof(TransactionViewModel.CreatedDateTime);
      Transactions = new ObservableCollection<TransactionViewModel>();
      GetTransactions();
    }

    public Translatable RecentTransactionsLabel => new RecentTransactionsLabelTranslatable();

    public ObservableCollection<TransactionViewModel> Transactions
    {
      get => transactions;
      set
      {
        Set(ref transactions, value);
      }
    }

    private CollectionView collectionView;

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

    private string sortedProp;
    private ListSortDirection sortDirection;

    public string SortedProp
    {
      get => sortedProp;
      set => Set(ref sortedProp, value);
    }

    public ListSortDirection SortDirection
    {
      get => sortDirection;
      set => Set(ref sortDirection, value);
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
        MessageBox.Show(new SessionInvalidLoggingOutTranslatable(), new ErrorTranslatable(), MessageBoxButton.OK, MessageBoxImage.Error);
        SetPropertiesOnLogout(sessionInstance);
      }
      catch (RpcException rex)
      {
        MessageBox.Show(new FailedToGetTransactionsErrorTranslatable(rex.Status.Detail), new ErrorTranslatable(), MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }
  }
}
