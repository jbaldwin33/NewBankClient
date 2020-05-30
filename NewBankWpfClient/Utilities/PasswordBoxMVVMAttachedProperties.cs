﻿using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Windows;

namespace NewBankWpfClient.Utilities
{
  public static class PasswordBoxMVVMAttachedProperties
  {
    public static SecureString GetEncryptedPassword(DependencyObject obj)
    {
      return (SecureString)obj.GetValue(EncryptedPasswordProperty);
    }

    public static void SetEncryptedPassword(DependencyObject obj, SecureString value)
    {
      obj.SetValue(EncryptedPasswordProperty, value);
    }

    public static readonly DependencyProperty EncryptedPasswordProperty =
        DependencyProperty.RegisterAttached("EncryptedPassword", typeof(SecureString), typeof(PasswordBoxMVVMAttachedProperties));
  }
}
