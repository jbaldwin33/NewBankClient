using Grpc.Core;
using NewBankClientGrpc.Protos;
using NewBankWpfClient.ServiceClients;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Windows.Markup.Localizer;

namespace NewBankWpfClient
{
  public interface ICommandProcessor
  {
    void ExecuteCommand<T>(Type commandType, string method) where T : ClientBase;
  }

  public class CommandProcessor : ICommandProcessor
  {
    public void ExecuteCommand<T>(Type commandType, string method) where T : ClientBase
    {
      //T client;
      //if (commandType == typeof(AccountCRUD.AccountCRUDClient))
      //{
      //  (client as AccountCRUD.AccountCRUDClient).Update(new Account { });
        
      //}
      //else if (commandType == typeof(Authentication.AuthenticationClient))
      //  client = ServiceClient.Instance.AuthenticationClient;
      //else if (commandType == typeof(Creation.CreationClient))
      //  client = ServiceClient.Instance.CreationClient;
      //else if (commandType == typeof(SessionCRUD.SessionCRUDClient))
      //  client = ServiceClient.Instance.SessionCRUDClient;
      //else if (commandType == typeof(UserCRUD.UserCRUDClient))
      //  client = ServiceClient.Instance.UserCRUDClient;
      //else
      //  throw new NotSupportedException($"{commandType.Name} is not supported");




    }
  }
}
