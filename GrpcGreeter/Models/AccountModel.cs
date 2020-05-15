using GrpcGreeter.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter.Models
{
  public enum AccountType
  {
    Checking,
    Saving
  }

  public class AccountModel
  {
    public Guid ID { get; set; }
    public double Balance { get; set; }
    public AccountType AccountType { get; set; }
    public Guid UserID { get; set; }

    public static AccountType ConvertFromProtoType(Protos.AccountType accountType)
    {
      return accountType switch
      {
        Protos.AccountType.Checking => AccountType.Checking,
        Protos.AccountType.Saving => AccountType.Saving,
        _ => throw new NotSupportedException(),
      };
    }

    public static Protos.AccountType ConvertFromDbType(AccountType accountType)
    {
      return accountType switch
      {
        AccountType.Checking => Protos.AccountType.Checking,
        AccountType.Saving =>   Protos.AccountType.Saving,
        _ => throw new NotSupportedException(),
      };
    }
  }

}
