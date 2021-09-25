using MVVMFramework.ViewModels;
using System;

namespace NewBankWpfClient.ViewModels
{
    public class TransactionViewModel : ViewModel
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
            set => SetProperty(ref createdDateTime, value);
        }

        public double Amount
        {
            get => amount;
            set => SetProperty(ref amount, value);
        }

        public string TransactionType
        {
            get => transactionType;
            set => SetProperty(ref transactionType, value);
        }

        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }
    }
}
