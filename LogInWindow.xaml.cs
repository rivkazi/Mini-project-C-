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
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        IBL myBL = BL.FactotyBL.GetBL();
        string HostID;
        bool exists = false;
        public LogInWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void btnInput_Click(object sender, RoutedEventArgs e)
        {
            HostID = txtBoxID.Text;
            this.Close();
            foreach(var v in myBL.GetAllHostingUnits())
            {
                if(HostID.Length != 9)
                    MessageBox.Show($"Unpossible ID", "UNKNOWN HOST", MessageBoxButton.OK, MessageBoxImage.Information);
                if (v.MyOwner.MyHostKey == int.Parse(HostID))
                    exists = true;
            }
            if (exists == true)
            {
                new PersonalHostingUnitWindow(HostID).ShowDialog();
                this.Close();
            }
            else
                MessageBox.Show($"Hello, Your ID Doesn't Exists in the System", "UNKNOWN HOST", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
