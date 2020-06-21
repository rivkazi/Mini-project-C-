using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Net;
using BE;
using BL;
using System.ComponentModel;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBL myBL = BL.FactotyBL.GetBL();
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        

        private void BtnClient_Click(object sender, RoutedEventArgs e)
        {
            new AddGuestRequestWindow().ShowDialog();
        }

        private void BtnHost_Click(object sender, RoutedEventArgs e)
        {
            new HostPasswordWindow().ShowDialog();
        }

        private void BtnManager_Click(object sender, RoutedEventArgs e)
        {
            new ManagerPasswordwindow().ShowDialog();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            BL_imp.flag = false;
        }
    }

}