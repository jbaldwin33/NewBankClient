using MVVMFramework.ViewNavigator;
using MVVMFramework.Views;
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
    /// Interaction logic for ViewBase.xaml
    /// </summary>
    public partial class HomeView : ViewBaseControl
    {
        public HomeView() : base(Navigator.Instance.CurrentViewModel)
        {
            InitializeComponent();
        }
    }
}
