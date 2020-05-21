using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using static GrpcGreeterWpfClient.Models.Enums;
using GrpcGreeter.Protos;

namespace GrpcGreeterWpfClient.Models
{
  public class AccountModel
  {
    public Guid ID { get; private set; }
    public double Balance { get; set; }
    public Guid UserID { get; set; }
    public AccountTypeEnum AccountType { get; set; }

    public AccountModel() { }
    public AccountModel(double balance, AccountTypeEnum accountType)
    {
      ID = Guid.NewGuid();
      Balance = balance;
      AccountType = accountType;
    }
    
    public void AddToBalance(float amount)
    {
      Balance += amount;
    }

    public static AccountModel ConvertAccount(Account account) => new AccountModel
    {
      AccountType = ConvertAccountType(account.AccountType),
      Balance = account.Balance,
      ID = Guid.Parse(account.Id),
      UserID = Guid.Parse(account.UserId)
    };

    public static AccountTypeEnum ConvertAccountType(AccountType accountType)
    {
      return accountType switch
      {
        GrpcGreeter.Protos.AccountType.Checking => AccountTypeEnum.Checking,
        GrpcGreeter.Protos.AccountType.Saving => AccountTypeEnum.Saving,
        _ => throw new NotSupportedException($"{accountType} is not supported"),
      };
    }

    public static AccountType ConvertAccountType(AccountTypeEnum accountType)
    {
      return accountType switch
      {
        AccountTypeEnum.Checking => GrpcGreeter.Protos.AccountType.Checking,
        AccountTypeEnum.Saving => GrpcGreeter.Protos.AccountType.Saving,
        _ => throw new NotSupportedException($"{accountType} is not supported"),
      };
    }
  }
}