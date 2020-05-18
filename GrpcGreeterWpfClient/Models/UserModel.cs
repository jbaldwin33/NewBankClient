using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static GrpcGreeterWpfClient.Models.Enums;
using GrpcGreeterWpfClient.Utilities;
using GrpcGreeter.Protos;

namespace GrpcGreeterWpfClient.Models
{
  public class UserModel
  {
    public Guid ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Guid AccountID { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public int Age { get; set; }
    public UserEnum UserType { get; set; }
    public SkillModel Skill { get; set; }

    public UserModel() { }
    public UserModel(string username, string password, string firstName, string lastName, int age, UserEnum userType, SkillModel skill)
    {
      Username = username;
      PasswordSalt = SecurePassword.CreateSalt();
      PasswordHash = new SecurePassword(password, PasswordSalt).ComputeSaltedHash();
      FirstName = firstName;
      LastName = lastName;
      Age = age;
      UserType = userType;
      Skill = skill;
      ID = Guid.NewGuid();
    }

    public static UserEnum ConvertToUserDbType(UserProtoType userProtoType)
    {
      return userProtoType switch
      {
        UserProtoType.Admin => UserEnum.Administrator,
        UserProtoType.User => UserEnum.User,
        _ => throw new NotSupportedException()
      };
    }

    public static UserProtoType ConvertToUserProtoType(UserEnum userDbType)
    {
      return userDbType switch
      {
        UserEnum.Administrator => UserProtoType.Admin,
        UserEnum.User => UserProtoType.User,
        _ => throw new NotSupportedException()
      };
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
