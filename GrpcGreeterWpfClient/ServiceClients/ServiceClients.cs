using Grpc.Core;
using Grpc.Net.Client;
using GrpcGreeter.Protos;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
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
      //var credentials = CallCredentials.FromInterceptor((context, metadata) =>
      //{
      //  metadata.Add("Authorization", $"Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImp0aSI6Ijc1MjhlNGFmLWE3YzAtNDVlZC04YTc1LWNjYzVkODRjZGU2MiIsImlhdCI6MTU5MDMwMjQzNywiZXhwIjoxNTkwMzA2MDM3fQ.C9npZuOU-R1Oxy87lM-a2SMuX9ydbuMoe6l4Shc6IEM");
      //  return Task.CompletedTask;
      //});

      //// SslCredentials is used here because this channel is using TLS.
      //// CallCredentials can't be used with ChannelCredentials.Insecure on non-TLS channels.
      //channel = GrpcChannel.ForAddress("https://192.168.0.18:5001", new GrpcChannelOptions
      //{
      //  Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
      //});

      var httpClientHandler = new HttpClientHandler();
      // Return `true` to allow certificates that are untrusted/invalid
      httpClientHandler.ServerCertificateCustomValidationCallback =
          HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
      var httpClient = new HttpClient(httpClientHandler);

      channel = GrpcChannel.ForAddress("https://192.168.0.18:5001", new GrpcChannelOptions { HttpClient = httpClient });

      //channel = GrpcChannel.ForAddress("https://192.168.0.18:5001", new GrpcChannelOptions { Credentials = new SslCredentials() });
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
