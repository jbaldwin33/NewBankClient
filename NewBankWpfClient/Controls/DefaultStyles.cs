using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace NewBankWpfClient.Controls
{
  public class DefaultTextBlock : TextBlock
  {
    public DefaultTextBlock()
    {
      HorizontalAlignment = HorizontalAlignment.Left;
      Margin = new Thickness(5, 0, 5, 5);
      Width = 120;
      Height = Double.NaN;
      Padding = new Thickness(5, 5, 5, 5);
      FontSize = 15;
    }

    static DefaultTextBlock()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(DefaultTextBlock), new FrameworkPropertyMetadata(typeof(DefaultTextBlock)));
    }
  }
}
