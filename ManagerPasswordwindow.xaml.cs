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
    /// Interaction logic for ManagerPasswordwindow.xaml
    /// </summary>
    public partial class ManagerPasswordwindow : Window
    {
        public ManagerPasswordwindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void BtnPassword_Click(object sender, RoutedEventArgs e)
        {
            if (MPasswordBox.Password == Configuration.ManagerPassword)
            {
                MPasswordBox.Password = "";
                this.Close();
                new MoreOptionsWindow().ShowDialog();
            }
            else
                MessageBox.Show("Incorrect Password, please try Again");

        }
    }
}
