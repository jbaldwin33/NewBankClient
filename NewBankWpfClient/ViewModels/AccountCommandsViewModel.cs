﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NewBankShared.Localization;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Runtime.CompilerServices;
using System.Text;

namespace NewBankWpfClient.ViewModels
{
  public class AccountCommandsViewModel : ViewModelBase
  {
    private readonly double currentBalance;
    private double amount;
    private string toUsername;
    private bool usernameVisible;
    private CommandEnum commandType;
    private Translatable questionText;

    private RelayCommand okCommand;
    private RelayCommand cancelCommand;

    public event EventHandler<WindowPopupEventArgs> OnFinishEventHandler;
    public event EventHandler OnCancelledEventHandler;

    public AccountCommandsViewModel(CommandEnum command, double currentBalance)
    {
      this.currentBalance = currentBalance;
      CommandType = command;
      UsernameVisible = command == CommandEnum.Transfer;
      Translatable text;
      text = command switch
      {
        CommandEnum.Deposit => new DepositTranslatable(),
        CommandEnum.Withdraw => new WithdrawTranslatable(),
        CommandEnum.Transfer => new TransferTranslatable(),
        _ => throw new NotSupportedException($"{command} is not supported"),
      };
      QuestionText = new OperationQuestionTranslatable(text);
    }

    public string RecipientLabel => $"{new NameOfRecipientTranslatable()}:";
    public string ConfirmLabel => new ConfirmTranslatable();
    public string CancelLabel => new CancelTranslatable();

    public double Amount
    {
      get => amount;
      set => Set(ref amount, value);
    }

    public string ToUsername
    {
      get => toUsername;
      set => Set(ref toUsername, value);
    }

    public bool UsernameVisible
    {
      get => usernameVisible;
      set => Set(ref usernameVisible, value);
    }

    public CommandEnum CommandType
    {
      get => commandType;
      set => Set(ref commandType, value);
    }

    public Translatable QuestionText
    {
      get => questionText;
      set => Set(ref questionText, value);
    }

    public RelayCommand OkCommand => okCommand ??= new RelayCommand(OkCommandExecute);
    public RelayCommand CancelCommand => cancelCommand ??= new RelayCommand(CancelCommandExecute);

    private void OkCommandExecute()
    {
      if (CommandAllowed())
        OnFinishEventHandler?.Invoke(this, new WindowPopupEventArgs(amount, commandType, toUsername));
    }

    private void CancelCommandExecute()
    {
      OnCancelledEventHandler?.Invoke(this, new EventArgs());
    }

    private bool CommandAllowed()
    {
      switch (commandType)
      {
        case CommandEnum.Deposit:
          return true;
        case CommandEnum.Withdraw:
        case CommandEnum.Transfer:
          return amount < currentBalance;
        default:
          throw new NotSupportedException($"{commandType} is not supported");
      }
    }
  }

  public class WindowPopupEventArgs : EventArgs
  {
    public CommandEnum CommandType { get; set; }
    public double Amount { get; set; }
    public string Username { get; set; }
    public WindowPopupEventArgs(double amount, CommandEnum commandType, string username)
    {
      Amount = amount;
      CommandType = commandType;
      Username = username;
    }
  }
}