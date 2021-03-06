﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBankWpfClient.Models
{
  public static class Enums
  {
    public enum AccountEnum
    {
      Checking,
      Saving
    }

    public enum UserEnum
    {
      Administrator = 0,
      User
    }

    public enum CommandType
    {
      Add,
      Edit,
      Delete
    }

    public enum TransactionEnum
    {
      Deposit,
      Withdraw,
      Transfer
    }
  }
}
