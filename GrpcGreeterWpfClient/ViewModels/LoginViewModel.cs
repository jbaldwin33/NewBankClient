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

    public LoginViewModel(INavigator navigator, UserCRUD.UserCRUDClient client)
    {
      this.client = client;
			this.navigator = navigator;
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



		public RelayCommand LoginCommand => loginCommand ??= new RelayCommand(LoginCommandExecute);

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
					navigator.UpdateCurrentUserCommand.Execute(
						new UserModel(
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
							}));

					MessageBox.Show("Log in successful.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}
		}
	}
}
