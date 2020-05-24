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
    private CommandEnum commandType;
    private RelayCommand okCommand;
    private RelayCommand cancelCommand;

    public event EventHandler<WindowPopupEventArgs> OnFinishEventHandler;
    public event EventHandler OnCancelledEventHandler;

    public AccountCommandsViewModel(CommandEnum command)
    {
      CommandType = command;
    }

    public double Amount
    {
      get => amount;
      set => Set(ref amount, value);
    }

    public CommandEnum CommandType
    {
      get => commandType;
      set => Set(ref commandType, value);
    }

    public RelayCommand OkCommand => okCommand ??= new RelayCommand(OkCommandExecute);
    public RelayCommand CancelCommand => cancelCommand ??= new RelayCommand(CancelCommandExecute);

    private void OkCommandExecute()
    {
      OnFinishEventHandler?.Invoke(this, new WindowPopupEventArgs(amount, commandType));
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
    public WindowPopupEventArgs(double amount, CommandEnum commandType)
    {
      Amount = amount;
      CommandType = commandType;
    }
  }
}
