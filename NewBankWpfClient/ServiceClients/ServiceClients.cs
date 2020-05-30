using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using NewBankServer.Protos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;

namespace NewBankWpfClient.ServiceClients
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

      string s = GetRootCertificates();
      var channel_creds = new SslCredentials(s);

      var httpClient = new HttpClient(new HttpClientHandler
      {
        SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
      });
      channel = GrpcChannel.ForAddress("https://192.168.0.10:443", new GrpcChannelOptions//"https://192.168.44.128:443", new GrpcChannelOptions
      {
        HttpClient = httpClient
      });
    }

    public static string GetRootCertificates()
    {
      StringBuilder builder = new StringBuilder();
      X509Store store = new X509Store(StoreName.Root);
      store.Open(OpenFlags.ReadOnly);
      foreach (X509Certificate2 mCert in store.Certificates)
      {
        builder.AppendLine(
            "# Issuer: " + mCert.Issuer.ToString() + "\n" +
            "# Subject: " + mCert.Subject.ToString() + "\n" +
            "# Label: " + mCert.FriendlyName.ToString() + "\n" +
            "# Serial: " + mCert.SerialNumber.ToString() + "\n" +
            "# SHA1 Fingerprint: " + mCert.GetCertHashString().ToString() + "\n" +
            ExportToPEM(mCert) + "\n");
      }
      return builder.ToString();
    }
    public static string ExportToPEM(X509Certificate cert)
    {
      StringBuilder builder = new StringBuilder();

      builder.AppendLine("-----BEGIN CERTIFICATE-----");
      builder.AppendLine(Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks));
      builder.AppendLine("-----END CERTIFICATE-----");

      return builder.ToString();
    }


    public static ServiceClient Instance => instance.Value;


    public UserCRUD.UserCRUDClient UserCRUDClient { get; internal set; }
    public AccountCRUD.AccountCRUDClient AccountCRUDClient { get; internal set; }
    public Authentication.AuthenticationClient AuthenticationClient { get; internal set; }
    public SessionCRUD.SessionCRUDClient SessionCRUDClient { get; internal set; }
    public Creation.CreationClient CreationClient { get; internal set; }
    public TransactionCRUD.TransactionCRUDClient TransactionCRUDClient { get; internal set; }

    public void CreateClients()
    {
      UserCRUDClient = new UserCRUD.UserCRUDClient(channel);
      AccountCRUDClient = new AccountCRUD.AccountCRUDClient(channel);
      AuthenticationClient = new Authentication.AuthenticationClient(channel);
      SessionCRUDClient = new SessionCRUD.SessionCRUDClient(channel);
      CreationClient = new Creation.CreationClient(channel);
      TransactionCRUDClient = new TransactionCRUD.TransactionCRUDClient(channel);
    }

    public void DisposeClients()
    {
      channel.Dispose();
    }
  }
}
