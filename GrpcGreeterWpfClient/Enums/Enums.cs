using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcGreeterWpfClient.Models
{
  public static class Enums
  {
    public enum AccountTypeEnum
    {
      Checking,
      Saving
    }

    public enum UserEnum
    {
      Administrator = 0,
      User
    }

    public enum Proficiency
    {
      Beginner,
      Adept,
      Advanced,
      Master
    }

    public enum CommandType
    {
      Add,
      Edit,
      Delete
    }
  }
}
