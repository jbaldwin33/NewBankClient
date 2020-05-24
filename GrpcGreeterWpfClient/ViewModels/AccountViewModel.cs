//using BankServer.Services;
using ControlzEx.Standard;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcGreeter.Protos;
using GrpcGreeterWpfClient.Models;
using GrpcGreeterWpfClient.Navigators;
using GrpcGreeterWpfClient.ServiceClients;
using GrpcGreeterWpfClient.Views;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using static GrpcGreeterWpfClient.Models.Enums;

namespace GrpcGreeterWpfClient.ViewModels
{
  public enum CommandEnum
  {
    Deposit,
    Withdraw,
    Transfer
  }

  public class AccountViewModel : ViewModelBase
  {
    private readonly ServiceClient serviceClient;
    private readonly SessionInstance sessionInstance;
    private double balance;
    private AccountEnum accountType;
    private bool detailsVisible;
    private double depositAmount;
    private double withdrawAmount;
    private double transferAmount;
    private RelayCommand depositCommand;
    private RelayCommand withdrawCommand;
    private RelayCommand transferCommand;

    public event EventHandler OnModelDialogFinished;

    public AccountViewModel(SessionInstance sessionInstance, ServiceClient serviceClient)
    {
      this.serviceClient = serviceClient;
      this.sessionInstance = sessionInstance;
      DetailsVisible = serviceClient.SessionCRUDClient.IsValidSession(new SessionRequest { SessionId = this.sessionInstance.SessionID.ToString() }).Valid;
      if (detailsVisible)
        UpdateAccountDetails();
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

    private void UpdateAccountDetails()
    {
      try
      {
        var account = serviceClient.AccountCRUDClient.GetByUserID(new AccountRequest { UserId = sessionInstance.CurrentUser.ID.ToString() }).Account;
        Balance = account.Balance;
        AccountType = AccountModel.ConvertAccountType(account.AccountType);
      }
      catch (RpcException rex)
      {
        MessageBox.Show($"Failed to get account details: {rex.Status.Detail}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private bool DepositCommandCanExecute() => true;
    private bool WithdrawCommandCanExecute() => Balance > 0;
    private bool TransferCommandCanExecute() => Balance > 0;

    private void DepositCommandExecute()
    {
      var vm = new AccountCommandsViewModel(CommandEnum.Deposit);
      vm.OnFinishEventHandler += ModalDialogClosed;
      var commandView = new AccountCommandView(vm, this)
      {
        WindowStartupLocation = WindowStartupLocation.CenterScreen
      };
      commandView.Show();
    }

    private void WithdrawCommandExecute()
    {
      var vm = new AccountCommandsViewModel(CommandEnum.Withdraw);
      vm.OnFinishEventHandler += ModalDialogClosed;
      var commandView = new AccountCommandView(vm, this)
      {
        WindowStartupLocation = WindowStartupLocation.CenterScreen
      };
      commandView.Show();
    }

    private void TransferCommandExecute()
    {
      var vm = new AccountCommandsViewModel(CommandEnum.Transfer);
      vm.OnFinishEventHandler += ModalDialogClosed;
      var commandView = new AccountCommandView(vm, this)
      {
        WindowStartupLocation = WindowStartupLocation.CenterScreen
      };
      commandView.Show();
    }

    private void ModalDialogClosed(object sender, WindowPopupEventArgs e)
    {
      var failed = false;
      if (e.Amount < 1)
      {
        failed = true;
        MessageBox.Show("Please enter an amount greater than 0");
      }
      else
      {
        switch (e.CommandType)
        {
          case CommandEnum.Deposit:
            Balance += e.Amount;
            try
            {
              serviceClient.AccountCRUDClient.Deposit(new DepositRequest { AccountId = sessionInstance.CurrentUser.AccountID.ToString(), Amount = e.Amount });
            }
            catch (RpcException rex)
            {
              MessageBox.Show($"Operation failed: {rex.Status.Detail}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            break;
          case CommandEnum.Withdraw:
            Balance -= e.Amount;
            try
            {
              serviceClient.AccountCRUDClient.Withdraw(new WithdrawRequest { AccountId = sessionInstance.CurrentUser.AccountID.ToString(), Amount = e.Amount });
            }
            catch (RpcException rex)
            {
              MessageBox.Show($"Operation failed: {rex.Status.Detail}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            break;
          case CommandEnum.Transfer:
            TransferAmount = e.Amount;
            try
            {
              if (string.IsNullOrEmpty(e.Username))
              {
                failed = true;
                MessageBox.Show("Please enter a user to transfer funds to", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
              }
              else
              {
                serviceClient.AccountCRUDClient.Transfer(new TransferRequest { Amount = transferAmount, Username = e.Username });
              }
            }
            catch (RpcException rex)
            {
              failed = true;
              MessageBox.Show($"Transfer failed: {rex.Status.Detail}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            break;
          default:
            throw new NotSupportedException($"{e.CommandType} is not supported");
        }
      }

      if (!failed)
        OnModelDialogFinished?.Invoke(this, new EventArgs());
    }


    public RelayCommand DepositCommand => depositCommand ??= new RelayCommand(DepositCommandExecute, DepositCommandCanExecute);
    public RelayCommand WithdrawCommand => withdrawCommand ??= new RelayCommand(WithdrawCommandExecute, WithdrawCommandCanExecute);
    public RelayCommand TransferCommand => transferCommand ??= new RelayCommand(TransferCommandExecute, TransferCommandCanExecute);

  }
}
