using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewBankWpfClient.ViewModels
{
  public class TransactionViewModel : ViewModelBase
  {
    private DateTime createdDateTime;
    private double amount;
    private string transactionType;
    private string message;

    public TransactionViewModel(DateTime createdDateTime, double amount, string transactionType, string message)
    {
      CreatedDateTime = createdDateTime;
      Amount = amount;
      TransactionType = transactionType;
      Message = message;
    }

    public DateTime CreatedDateTime
    {
      get => createdDateTime;
      set => Set(ref createdDateTime, value);
    }

    public double Amount
    {
      get => amount;
      set => Set(ref amount, value);
    }
    
    public string TransactionType
    {
      get => transactionType;
      set => Set(ref transactionType, value);
    }

    public string Message
    {
      get => message;
      set => Set(ref message, value);
    }


  }
}
