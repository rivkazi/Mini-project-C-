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
using System.Windows.Shapes;
using BE;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for HostPasswordWindow.xaml
    /// </summary>
    public partial class HostPasswordWindow : Window
    {
        public HostPasswordWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void BtnPassword_Click(object sender, RoutedEventArgs e)
        {
            if(PasswordBox.Password == Configuration.HostPassword)
            {
                PasswordBox.Password = "";
                this.Close();
                new HostingUnitWindow().ShowDialog();
            }
            else
                MessageBox.Show("Incorrect Password, please try Again");

        }
    }
}
