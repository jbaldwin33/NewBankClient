﻿using ControlzEx.Standard;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Grpc.Core;
using Grpc.Net.Client;
using NewBankClientGrpc;
using NewBankClientGrpc.Localization;
using NewBankServer.Protos;
using NewBankWpfClient.Models;
using NewBankWpfClient.Navigators;
using NewBankWpfClient.ServiceClients;
using NewBankWpfClient.Views;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using static NewBankWpfClient.Models.Enums;

namespace NewBankWpfClient.ViewModels
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

    public Translatable DepositLabel => new DepositCommandTranslatable();
    public Translatable WithdrawLabel => new WithdrawCommandTranslatable();
    public Translatable TransferLabel => new TransferCommandTranslatable();

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
        MessageBox.Show(new FailedToGetAccountDetailsErrorTranslatable(rex.Status.Detail), new ErrorTranslatable(), MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private bool DepositCommandCanExecute() => true;
    private bool WithdrawCommandCanExecute() => Balance > 0;
    private bool TransferCommandCanExecute() => Balance > 0;

    private void DepositCommandExecute()
    {
      var vm = new AccountCommandsViewModel(CommandEnum.Deposit, balance);
      vm.OnFinishEventHandler += ModalDialogClosed;
      var commandView = new AccountCommandView(vm, this)
      {
        WindowStartupLocation = WindowStartupLocation.CenterScreen
      };
      commandView.Show();
    }

    private void WithdrawCommandExecute()
    {
      var vm = new AccountCommandsViewModel(CommandEnum.Withdraw, balance);
      vm.OnFinishEventHandler += ModalDialogClosed;
      var commandView = new AccountCommandView(vm, this)
      {
        WindowStartupLocation = WindowStartupLocation.CenterScreen
      };
      commandView.Show();
    }

    private void TransferCommandExecute()
    {
      var vm = new AccountCommandsViewModel(CommandEnum.Transfer, balance);
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
        MessageBox.Show(new AmountGreaterThanZeroTranslatable());
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
              MessageBox.Show(new OperationFailedErrorTranslatable(rex.Status.Detail), new ErrorTranslatable(), MessageBoxButton.OK, MessageBoxImage.Error);
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
              MessageBox.Show(new OperationFailedErrorTranslatable(rex.Status.Detail), new ErrorTranslatable(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            break;
          case CommandEnum.Transfer:
            Balance -= e.Amount;
            try
            {
              if (string.IsNullOrEmpty(e.Username))
              {
                failed = true;
                MessageBox.Show(new EnterUserForTransferTranslatable(), new ErrorTranslatable(), MessageBoxButton.OK, MessageBoxImage.Error);
              }
              else
              {
                serviceClient.AccountCRUDClient.Transfer(new TransferRequest { Amount = e.Amount, ToUsername = e.Username, FromUsername = sessionInstance.CurrentUser.Username });
              }
            }
            catch (RpcException rex)
            {
              failed = true;
              MessageBox.Show(new TransferFailedErrorTranslatable(rex.Status.Detail), new ErrorTranslatable(), MessageBoxButton.OK, MessageBoxImage.Error);
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
