using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Grpc.Net.Client;
using GrpcGreeter.Protos;
using GrpcGreeterWpfClient.Models;
using GrpcGreeterWpfClient.Utilities;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using static GrpcGreeterWpfClient.Utilities.Utilities;

namespace GrpcGreeterWpfClient.ViewModels
{
  public class SignUpViewModel : ViewModelBase
  {
		//private readonly UserCRUD.UserCRUDClient userCRUDClient;
		//private readonly AccountCRUD.AccountCRUDClient accountCRUDClient;
		private string firstName;
		private string lastName;
		private string username;
		private string password;
		private int age;
		private string userSkill;
		private ObservableCollection<Enums.Proficiency> proficiencyItems;
		private Enums.Proficiency proficiency;
		private bool proficiencyVisible;
		private RelayCommand signUpCommand;

		public SignUpViewModel(UserCRUD.UserCRUDClient userCRUDClient, AccountCRUD.AccountCRUDClient accountCRUDClient)
		{
			//this.userCRUDClient = userCRUDClient;
			//this.accountCRUDClient = accountCRUDClient;
			ProficiencyItems = new ObservableCollection<Enums.Proficiency>
			{
				Enums.Proficiency.Beginner,
				Enums.Proficiency.Adept,
				Enums.Proficiency.Expert,
				Enums.Proficiency.Master,
			};
			ProficiencyVisible = false;
		}

    #region Properties
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
		public string Password
		{
			get => password;
			set => Set(ref password, value);
		}

		public int Age
		{
			get => age;
			set => Set(ref age, value);
		}

		public string UserSkill
		{
			get => userSkill;
			set
			{
				Set(ref userSkill, value);
				ProficiencyVisible = !string.IsNullOrEmpty(value);
			}
		}

		public ObservableCollection<Enums.Proficiency> ProficiencyItems
		{
			get => proficiencyItems;
			set => Set(ref proficiencyItems, value);
		}

		public Enums.Proficiency Proficiency
		{
			get => proficiency;
			set => Set(ref proficiency, value);
		}

		public bool ProficiencyVisible
		{
			get => proficiencyVisible;
			set => Set(ref proficiencyVisible, value);
		}

    #endregion

    public RelayCommand SignUpCommand => signUpCommand ?? (signUpCommand = new RelayCommand(SignUpCommandExecute));

		private void SignUpCommandExecute()
		{
			using var channel = GrpcChannel.ForAddress("https://localhost:5001");
			var userCRUDClient = new UserCRUD.UserCRUDClient(channel);
			var accountCRUDClient = new AccountCRUD.AccountCRUDClient(channel);
			var skillCRUDClient = new SkillCRUD.SkillCRUDClient(channel);

			var users = userCRUDClient.GetUsers(new Empty());
			if (users.Items.Any(u => u.Username == username))
				MessageBox.Show("This username already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			else
			{
				var accountID = Guid.NewGuid();
				var userID = Guid.NewGuid();
				var skillID = Guid.NewGuid();
				var passwordSalt = SecurePassword.CreateSalt();
				
				var userReply = userCRUDClient.Insert(new User
				{
					Username = username,
					PasswordSalt = passwordSalt,
					PasswordHash = new SecurePassword(password, passwordSalt).ComputeSaltedHash(),
					FirstName = firstName,
					LastName = lastName,
					Id = userID.ToString(),
					Age = age,
					SkillId = skillID.ToString(),
					AccountId = accountID.ToString(),
					UserType = UserProtoType.User
				});
				var accountReply = accountCRUDClient.Insert(new Account
				{
					Id = accountID.ToString(),
					UserId = userID.ToString(),
					Balance = 0.0,
					AccountType = AccountType.Checking
				});
				var skillReply = skillCRUDClient.Insert(new Skill
				{
					Id = skillID.ToString(),
					Name = userSkill,
					Proficiency = ConvertFromDbType(Proficiency),
					OwnerId = userID.ToString()
				});
				MessageBox.Show("Sign up successful", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}
	}
}
