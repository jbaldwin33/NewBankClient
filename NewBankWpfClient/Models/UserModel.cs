using System;
using static NewBankWpfClient.Models.Enums;
using NewBankServer.Protos;
using System.Security;
using MVVMFramework.Utilities;

namespace NewBankWpfClient.Models
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
        public UserEnum UserType { get; set; }

        public UserModel() { }
        public UserModel(string username, SecureString password, string firstName, string lastName, UserEnum userType)
        {
            Username = username;
            PasswordSalt = SecurePasswordUtility.CreateSalt();
            PasswordHash = new SecurePasswordUtility(password, PasswordSalt).ComputeSaltedHash();
            FirstName = firstName;
            LastName = lastName;
            UserType = userType;
            ID = Guid.NewGuid();
        }

        public static UserEnum ConvertUserType(UserProtoEnum userProtoType)
        {
            return userProtoType switch
            {
                UserProtoEnum.Admin => UserEnum.Administrator,
                UserProtoEnum.User => UserEnum.User,
                _ => throw new NotSupportedException()
            };
        }

        public static UserProtoEnum ConvertUserType(UserEnum userDbType)
        {
            return userDbType switch
            {
                UserEnum.Administrator => UserProtoEnum.Admin,
                UserEnum.User => UserProtoEnum.User,
                _ => throw new NotSupportedException()
            };
        }

        public static UserModel ConvertUser(User user) => new UserModel
        {
            AccountID = Guid.Parse(user.AccountId),
            FirstName = user.FirstName,
            ID = Guid.Parse(user.Id),
            LastName = user.LastName,
            PasswordHash = user.PasswordHash,
            PasswordSalt = user.PasswordSalt,
            Username = user.Username,
            UserType = ConvertUserType(user.UserType)
        };

        public static User ConvertUser(UserModel user) => new User
        {
            AccountId = user.AccountID.ToString(),
            FirstName = user.FirstName,
            Id = user.ID.ToString(),
            LastName = user.LastName,
            PasswordHash = user.PasswordHash,
            PasswordSalt = user.PasswordSalt,
            Username = user.Username,
            UserType = ConvertUserType(user.UserType)
        };

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
