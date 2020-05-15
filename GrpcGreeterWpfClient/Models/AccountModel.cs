using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using static GrpcGreeterWpfClient.Models.Enums;

namespace GrpcGreeterWpfClient.Models
{
  public class AccountModel
  {
    public Guid ID { get; private set; }
    public float Balance { get; set; }
    public Guid UserID { get; set; }
    public AccountTypeEnum AccountType { get; set; }

    public AccountModel(float balance, AccountTypeEnum accountType)
    {
      ID = Guid.NewGuid();
      Balance = balance;
      AccountType = accountType;
    }
    
    public void AddToBalance(float amount)
    {
      Balance += amount;
    }
  }
}
