using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static GrpcGreeterWpfClient.Models.Enums;
using GrpcGreeterWpfClient.Utilities;

namespace GrpcGreeterWpfClient.Models
{
  public class UserModel
  {
    public Guid ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Guid AccountID { get; set; }
    public string Username { get; set; }
    public SecurePassword SecurePassword { get; set; }
    public string PasswordHash { get; set; }
    public int PasswordSalt { get; set; }
    public UserEnum UserType { get; set; }

    public UserModel() { }
    public UserModel(string username, string password, string firstName, string lastName, UserEnum userType)
    {
      Username = username;
      PasswordSalt = SecurePassword.CreateSalt();
      SecurePassword = new SecurePassword(password, PasswordSalt);
      PasswordHash = SecurePassword.ComputeSaltedHash();
      FirstName = firstName;
      LastName = lastName;
      UserType = userType;
      ID = Guid.NewGuid();
    }

    public void SeeActivity(UserModel user, string password, UserEnum type)
    {
      //if (string.Equals(user.Password, password))
      //{
      //  //Utilities.ShowActivity(user);
      //}
    }
    public void Validate()
    {

    }
  }
}
