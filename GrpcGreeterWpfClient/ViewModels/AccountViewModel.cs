using GalaSoft.MvvmLight;
using GrpcGreeter.Protos;
using GrpcGreeterWpfClient.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text;
using System.Threading.Tasks;

namespace GrpcGreeterWpfClient.ViewModels
{
  public class AccountViewModel : ViewModelBase
  {
    private UserCRUD.UserCRUDClient userCRUDClient;
    private readonly UserModel currentUser;
    private string firstName;
    private string lastName;
    private string username;
    private int age;
    private string userSkill;
    private bool detailsVisible;

    public string FirstName
    {
      get => firstName;
      set => Set(ref firstName, value);
    }

    public string LastName
    {
      get => lastName;
      set => Set(ref lastName, value);
    }


    public string Username
    {
      get => username;
      set => Set(ref username, value);
    }

    public int Age
    {
      get => age;
      set => Set(ref age, value);
    }

    public string UserSkill
    {
      get => userSkill;
      set => Set(ref userSkill, value);
    }

    public bool DetailsVisible
    {
      get => detailsVisible;
      set => Set(ref detailsVisible, value);
    }

    public AccountViewModel(UserCRUD.UserCRUDClient client, UserModel user)
    {
      userCRUDClient = client;
      currentUser = user;
      DetailsVisible = currentUser != null;
      if (currentUser != null)
        UpdateUserDetails();
    }

    private void UpdateUserDetails()
    {
      FirstName = currentUser.FirstName;
      LastName = currentUser.LastName;
      Username = currentUser.Username;
      Age = currentUser.Age;
      UserSkill = currentUser.Skill.Name;
    }
  }
}
