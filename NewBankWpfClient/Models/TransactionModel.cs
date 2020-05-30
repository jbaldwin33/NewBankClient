using NewBankServer.Protos;
using System;
using System.Collections.Generic;
using System.Text;
using static NewBankWpfClient.Models.Enums;

namespace NewBankWpfClient.Models
{
  public class TransactionModel
  {
    public Guid ID { get; set; }
    public DateTime TransactionCreatedTime { get; set; }
    public string Message { get; set; }
    public Guid UserID { get; set; }
    public TransactionEnum TransactionType { get; set; }
    public double Amount { get; set; }

    public static TransactionEnum ConvertTransactionType(TransactionProtoEnum transactionType)
      => transactionType switch
      {
        TransactionProtoEnum.Deposit =>  TransactionEnum.Deposit,
        TransactionProtoEnum.Withdraw => TransactionEnum.Withdraw,
        TransactionProtoEnum.Transfer => TransactionEnum.Transfer,
        _ => throw new NotSupportedException($"{transactionType} not supported")
      };

    public static TransactionProtoEnum ConvertTransactionType(TransactionEnum transactionType)
      => transactionType switch
      {
        TransactionEnum.Deposit => TransactionProtoEnum.Deposit,
        TransactionEnum.Withdraw => TransactionProtoEnum.Withdraw,
        TransactionEnum.Transfer => TransactionProtoEnum.Transfer,
        _ => throw new NotSupportedException($"{transactionType} not supported")
      };
  }
}
