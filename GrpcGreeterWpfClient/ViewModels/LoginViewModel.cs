using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Grpc.Net.Client;
using GrpcGreeter.Protos;
using GrpcGreeterWpfClient.Models;
using GrpcGreeterWpfClient.Navigators;
using GrpcGreeterWpfClient.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GrpcGreeterWpfClient.ViewModels
{
  public class LoginViewModel : ViewModelBase
  {
    private string username;
    private string password;
    private UserCRUD.UserCRUDClient client;
    private readonly INavigator navigator;
    private RelayCommand loginCommand;
    private RelayCommand logoutCommand;
    private UserModel currentUser;
    private bool loggedIn;


    public LoginViewModel(INavigator navigator, UserCRUD.UserCRUDClient client, UserModel currentUser)
    {
      this.client = client;
      this.navigator = navigator;
      CurrentUser = currentUser;
      LoggedIn = false;
    }

    public string Username
    {
      get => username;
      set => Set(ref username, value);
    }

    public string Password
    {
      get => password;
      set => Set(ref password, value);
    }

    public UserModel CurrentUser
    {
      get => currentUser;
      set => Set(ref currentUser, value);
    }

    public bool LoggedIn
    {
      get => loggedIn;
      set => Set(ref loggedIn, value);
    }


    public event EventHandler<LoginEventArgs> LoginEventHandler;

    public void OnLogin(UserModel user)
    {
      LoginEventHandler?.Invoke(this, new LoginEventArgs(user));
    }

    public RelayCommand LoginCommand => loginCommand ??= new RelayCommand(LoginCommandExecute);
    public RelayCommand LogoutCommand => logoutCommand ??= new RelayCommand(LogoutCommandExecute);

    private void LoginCommandExecute()
    {
      using var channel = GrpcChannel.ForAddress("https://localhost:5001");
      var userCRUDClient = new UserCRUD.UserCRUDClient(channel);

      var user = userCRUDClient.GetUsers(new Empty()).Items.FirstOrDefault(u => u.Username == username);
      if (user == null)
        MessageBox.Show("This username does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      else
      {
        var hash = new SecurePassword(password, user.PasswordSalt).ComputeSaltedHash();
        if (hash == user.PasswordHash)
        {
          var skillClient = new SkillCRUD.SkillCRUDClient(channel);
          var skill = skillClient.GetByID(new SkillFilter { Id = user.SkillId });
          var newUser = new UserModel(
            user.Username,
            password,
            user.FirstName,
            user.LastName,
            user.Age,
            UserModel.ConvertToUserDbType(user.UserType),
            new SkillModel
            {
              ID = Guid.Parse(user.SkillId),
              Name = skill.Name,
              SkillProficiency = SkillModel.ConvertFromProtoType(skill.Proficiency)
            });
          LoggedIn = true;
          navigator.CurrentUser = newUser;

          MessageBox.Show("Log in successful.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
      }
    }

    private void LogoutCommandExecute()
    {
      CurrentUser = null;
      LoggedIn = false;
      navigator.CurrentUser = null;
    }
  }

  public class LoginEventArgs : EventArgs
  {
    public UserModel User { get; set; }
    public LoginEventArgs(UserModel user)
    {
      User = user;
    }
  }
}
