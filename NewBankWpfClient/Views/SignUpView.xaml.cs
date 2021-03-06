﻿using NewBankWpfClient.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NewBankWpfClient.Views
{
  /// <summary>
  /// Interaction logic for SignUpView.xaml
  /// </summary>
  public partial class SignUpView : UserControl
  {
    public SignUpView()
    {
      InitializeComponent();
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
      var pBox = sender as PasswordBox;
      PasswordBoxMVVMAttachedProperties.SetEncryptedPassword(pBox, pBox.SecurePassword);
    }

  }
}
