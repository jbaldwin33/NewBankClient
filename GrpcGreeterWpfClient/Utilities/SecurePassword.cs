using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GrpcGreeterWpfClient.Utilities
{
  public class SecurePassword
  {
    private readonly string _password;
    private readonly string _salt;

    public SecurePassword(string password, string salt)
    {
      _password = password;
      _salt = salt;
    }

    public string ComputeSaltedHash()
    {
      ASCIIEncoding encoder = new ASCIIEncoding();
      byte[] secretBytes = encoder.GetBytes(_password);

      //create a new salt
      byte[] saltBytes = Convert.FromBase64String(_salt);

      byte[] toHash = new byte[secretBytes.Length + saltBytes.Length];
      Array.Copy(secretBytes, 0, toHash, 0, secretBytes.Length);
      Array.Copy(saltBytes, 0, toHash, secretBytes.Length, saltBytes.Length);

      SHA1 sha1 = SHA1.Create();
      byte[] computedHash = sha1.ComputeHash(toHash);

      return encoder.GetString(computedHash);
    }

    public static string CreateSalt()
    {
      byte[] saltBytes = new byte[16];
      RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
      rng.GetBytes(saltBytes);

      return Convert.ToBase64String(saltBytes);
    }
  }
}
