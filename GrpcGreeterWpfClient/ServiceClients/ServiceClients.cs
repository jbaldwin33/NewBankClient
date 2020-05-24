using Grpc.Core;
using Grpc.Net.Client;
using GrpcGreeter.Protos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Channels;
using System.Windows;

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
      channel = GrpcChannel.ForAddress("https://192.168.0.18:5001", new GrpcChannelOptions { Credentials = new SslCredentials(File.ReadAllText("C:\\Program Files\\OpenSSL-Win64\\bin\\server.crt")) });
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
