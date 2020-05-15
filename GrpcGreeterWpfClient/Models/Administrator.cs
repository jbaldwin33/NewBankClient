using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static GrpcGreeterWpfClient.Models.Enums;

namespace GrpcGreeterWpfClient.Models
{
  public class Administrator
  {
    public string Username { get; set; }
    //public SecurePassword SecurePassword { get; set; }
    public string PasswordHash { get; set; }
    public int PasswordSalt { get; set; }
    public UserEnum UserType { get; set; }
    public Guid ID { get; set; }

    public Administrator(string username, string password, UserEnum userType)
    {
      Username = username;
      //Password = password;
      UserType = userType;
      ID = Guid.NewGuid();
    }

    public void SeeActivity(UserModel user, string password, UserEnum type)
    {
      //Utilities.ShowActivity(user);
    }

    public void Validate()
    {
      throw new NotImplementedException();
    }
  }
}
