using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcGreeterWpfClient.Models
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
  }
}
