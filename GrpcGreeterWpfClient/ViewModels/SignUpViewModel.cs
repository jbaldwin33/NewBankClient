using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GrpcGreeterWpfClient.Models;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace GrpcGreeterWpfClient.ViewModels
{
  public class SignUpViewModel : ViewModelBase
  {
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

		public SignUpViewModel()
		{
			ProficiencyItems = new ObservableCollection<Enums.Proficiency>
			{
				Enums.Proficiency.Beginner,
				Enums.Proficiency.Adept,
				Enums.Proficiency.Advanced,
				Enums.Proficiency.Master,
			};
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
			var newUser = new UserModel(username, password, firstName, lastName, Enums.UserEnum.User);
			
		}
	}
}
