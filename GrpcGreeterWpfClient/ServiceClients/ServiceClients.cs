using Grpc.Net.Client;
using GrpcGreeter.Protos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace GrpcGreeterWpfClient.ServiceClients
{
  public interface IServiceClient
  {
    void CreateClients();
    void DisposeClients();
  }

  public class ServiceClient : IServiceClient
  {
    private static readonly Lazy<ServiceClient> instance = new Lazy<ServiceClient>(() => new ServiceClient());
    private readonly GrpcChannel channel;

    public ServiceClient()
    {
      channel = GrpcChannel.ForAddress("https://localhost:5001");
    }
    
    public static ServiceClient Instance => instance.Value;


    public UserCRUD.UserCRUDClient UserCRUDClient { get; internal set; }
    public AccountCRUD.AccountCRUDClient AccountCRUDClient { get; internal set; }
    public Authentication.AuthenticationClient AuthenticationClient { get; internal set; }
    public SessionCRUD.SessionCRUDClient SessionCRUDClient { get; internal set; }
    public Creation.CreationClient CreationClient { get; internal set; }

    public void CreateClients()
    {
      UserCRUDClient = new UserCRUD.UserCRUDClient(channel);
      AccountCRUDClient = new AccountCRUD.AccountCRUDClient(channel);
      AuthenticationClient = new Authentication.AuthenticationClient(channel);
      SessionCRUDClient = new SessionCRUD.SessionCRUDClient(channel);
      CreationClient = new Creation.CreationClient(channel);
    }

    public void DisposeClients()
    {
      channel.Dispose();
    }
  }
}
