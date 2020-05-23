//using BankServer.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GrpcGreeterWpfClient.Navigators;
using System;
using System.Collections.Generic;
using System.Text;
using static GrpcGreeterWpfClient.Models.Enums;

namespace GrpcGreeterWpfClient.ViewModels
{
  public class AccountViewModel : ViewModelBase
  {
		//private readonly SessionService sessionService;
		private readonly SessionInstance sessionInstance;
		private double balance;
		private AccountEnum accountType;
		private double depositAmount;
		private double withdrawAmount;
		private double transferAmount;
		private bool detailsVisible;
		private RelayCommand depositCommand;
		private RelayCommand withdrawCommand;
		private RelayCommand transferCommand;


		public AccountViewModel(/*SessionService sessionService, */SessionInstance sessionInstance)
		{
			//this.sessionService = sessionService;
			this.sessionInstance = sessionInstance;
		}

		public double Balance
		{
			get => balance;
			set => Set(ref balance, value);
		}

		public AccountEnum AccountType
		{
			get => accountType;
			set => Set(ref accountType, value);
		}

		public double DepositAmount
		{
			get => depositAmount;
			set => Set(ref depositAmount, value);
		}

		public double WithdrawAmount
		{
			get => withdrawAmount;
			set => Set(ref withdrawAmount, value);
		}

		public double TransferAmount
		{
			get => transferAmount;
			set => Set(ref transferAmount, value);
		}

		public bool DetailsVisible
		{
			get => detailsVisible;
			set => Set(ref detailsVisible, value);
		}

		public RelayCommand DepositCommand => depositCommand ??= new RelayCommand(DepositCommandExecute, DepositCommandCanExecute);

		private bool DepositCommandCanExecute() => true;

		private void DepositCommandExecute()
		{
			
		}
	}
}
