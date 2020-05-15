using GalaSoft.MvvmLight;
using GrpcGreeterWpfClient.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GrpcGreeterWpfClient.ViewModels
{
  public class LoginViewModel : ViewModelBase
  {
		private string username;
		private string password;

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

		


	}
}
