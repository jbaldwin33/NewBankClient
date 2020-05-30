using Grpc.Core;
using Grpc.Net.Client;
using NewBankServer.Protos;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewBankWpfClient.Factories
{
  public interface IServiceFactory
  {
    object GetService(Type serviceType);
  }
  public class ServiceFactory : IServiceFactory
  {
    private readonly GrpcChannel channel = GrpcChannel.ForAddress("https://mywebserver.hopto.org:443");
    public object GetService(Type serviceType)
    {
      if (serviceType == typeof(AccountCRUD.AccountCRUDClient))
        return new AccountCRUD.AccountCRUDClient(channel);
      else if (serviceType == typeof(Authentication.AuthenticationClient))
        return new AccountCRUD.AccountCRUDClient(channel);
      else if (serviceType == typeof(Creation.CreationClient))
        return new Creation.CreationClient(channel);
      else if (serviceType == typeof(SessionCRUD.SessionCRUDClient))
        return new SessionCRUD.SessionCRUDClient(channel);
      else if (serviceType == typeof(UserCRUD.UserCRUDClient))
        return new UserCRUD.UserCRUDClient(channel);
      else
        throw new NotSupportedException($"{serviceType.Name} is not supported");
    }
  }
}
