﻿using Grpc.Core;
using GrpcGreeter.Protos;
using GrpcGreeterWpfClient.ServiceClients;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Windows.Markup.Localizer;

namespace GrpcGreeterWpfClient
{
  public interface ICommandProcessor
  {
    void ExecuteCommand(Type commandType, string method);
  }

  public class CommandProcessor : ICommandProcessor
  {
    public void ExecuteCommand(Type commandType, string method)
    {
      //ClientBase client;
      //if (commandType == typeof(AccountCRUD.AccountCRUDClient))
      //{
      //  var client = ServiceClient.Instance.AccountCRUDClient;
      //  client.
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
