using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Runtime.CompilerServices;
using System.Text;

namespace GrpcGreeterWpfClient.ViewModels
{
  public class AccountCommandsViewModel : ViewModelBase
  {
    private double amount;
    private string toUsername;
    private bool usernameVisible;
    private CommandEnum commandType;
    private string questionText;

    private RelayCommand okCommand;
    private RelayCommand cancelCommand;

    public event EventHandler<WindowPopupEventArgs> OnFinishEventHandler;
    public event EventHandler OnCancelledEventHandler;

    public AccountCommandsViewModel(CommandEnum command)
    {
      CommandType = command;
      UsernameVisible = command == CommandEnum.Transfer;
      string text = string.Empty;
      text = command switch
      {
        CommandEnum.Deposit => "deposit",
        CommandEnum.Withdraw => "withdraw",
        CommandEnum.Transfer => "transfer",
        _ => throw new NotSupportedException($"{command} is not supported"),
      };
      QuestionText = $"How much would you like to {text}?";
    }

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

    public string QuestionText
    {
      get => questionText;
      set => Set(ref questionText, value);
    }

    public RelayCommand OkCommand => okCommand ??= new RelayCommand(OkCommandExecute);
    public RelayCommand CancelCommand => cancelCommand ??= new RelayCommand(CancelCommandExecute);

    private void OkCommandExecute()
    {
      OnFinishEventHandler?.Invoke(this, new WindowPopupEventArgs(amount, commandType, toUsername));
    }

    private void CancelCommandExecute()
    {
      OnCancelledEventHandler?.Invoke(this, new EventArgs());
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
