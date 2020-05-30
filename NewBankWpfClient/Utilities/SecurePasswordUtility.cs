using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NewBankWpfClient.Utilities
{
  public class SecurePasswordUtility
  {
    private readonly SecureString _password;
    private readonly string _salt;

    public SecurePasswordUtility(SecureString password, string salt)
    {
      _password = password;
      _salt = salt;
    }

    public string ComputeSaltedHash()
    {
      byte[] secretBytes = new byte[_password.Length];

      IntPtr valuePtr = IntPtr.Zero;
      try
      {
        valuePtr = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(_password);
        for (int i = 0; i < _password.Length; i++)
        {
          short unicodeChar = System.Runtime.InteropServices.Marshal.ReadInt16(valuePtr, i * 2);
          secretBytes[i] = Convert.ToByte(unicodeChar);
        }
      }
      finally
      {
        System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
      }


      //create a new salt
      byte[] saltBytes = Convert.FromBase64String(_salt);

      byte[] toHash = new byte[secretBytes.Length + saltBytes.Length];
      Array.Copy(secretBytes, 0, toHash, 0, secretBytes.Length);
      Array.Copy(saltBytes, 0, toHash, secretBytes.Length, saltBytes.Length);

      SHA1 sha1 = SHA1.Create();
      byte[] computedHash = sha1.ComputeHash(toHash);

      return Convert.ToBase64String(computedHash);
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
