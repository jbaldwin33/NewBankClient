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
    private readonly int _salt;

    public SecurePassword(string password, int salt)
    {
      _password = password;
      _salt = salt;
    }

    public string ComputeSaltedHash()
    {
      ASCIIEncoding encoder = new ASCIIEncoding();
      byte[] secretBytes = encoder.GetBytes(_password);

      //create a new salt
      byte[] saltBytes = new byte[4];
      saltBytes[0] = (byte)(_salt >> 24);
      saltBytes[1] = (byte)(_salt >> 16);
      saltBytes[2] = (byte)(_salt >> 8);
      saltBytes[3] = (byte)(_salt);

      byte[] toHash = new byte[secretBytes.Length + saltBytes.Length];
      Array.Copy(secretBytes, 0, toHash, 0, secretBytes.Length);
      Array.Copy(saltBytes, 0, toHash, secretBytes.Length, saltBytes.Length);

      SHA1 sha1 = SHA1.Create();
      byte[] computedHash = sha1.ComputeHash(toHash);

      return encoder.GetString(computedHash);
    }

    public static int CreateSalt()
    {
      byte[] saltBytes = new byte[4];
      RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
      rng.GetBytes(saltBytes);

      return (((int)saltBytes[0]) << 24) + (((int)saltBytes[1]) << 16) + (((int)(saltBytes[2]) << 8) + ((int)saltBytes[3]));
    }
  }
}
