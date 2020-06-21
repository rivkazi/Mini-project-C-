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
    /// Interaction logic for DeleteHostingUnitWindow.xaml
    /// </summary>
    public partial class DeleteHostingUnitWindow : Window
    {
        string MyID;
        List<HostingUnit> HostingUnits;
        List<HostingUnit> MyHostingUnits;
        IBL myBL = BL.FactotyBL.GetBL();

        public DeleteHostingUnitWindow(string _HostID)
        {
            InitializeComponent();
            MyID = _HostID;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //myBL = new BL.BL_imp();
            HostingUnits = myBL.GetAllHostingUnits();
            refreshData();
        }

        private List<HostingUnit> sorting()
        {
            MyHostingUnits = new List<HostingUnit>();
            foreach (HostingUnit item in myBL.GetAllHostingUnits())
            {
                if (item.MyOwner.MyHostKey == int.Parse(MyID))
                    MyHostingUnits.Add(item);
            }
            if (MyHostingUnits.Count == 0)
                this.Close();
            return MyHostingUnits;
        }

        private void delete_HostingUnit_click(object sender, RoutedEventArgs e)
        {
            HostingUnit obj = this.HostingUnitDataGrid.SelectedItem as HostingUnit;
            if (obj != null)
            {
                MessageBoxResult res = MessageBox.Show($"Are you sure you want to delete this Tester?", "DELETION", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    try
                    {
                        myBL.DeleteHostingUnit(obj);
                        this.refreshData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void refreshData()
        {
            try
            {
                this.HostingUnitDataGrid.ItemsSource = sorting();
            }
            catch
            {
                this.Close();
            }
        }

        private void more_details_click(object sender, RoutedEventArgs e)
        {
            HostingUnit obj = this.HostingUnitDataGrid.SelectedItem as HostingUnit;
            if (obj != null)
            {
                MessageBox.Show($"Datails of Hosting Unit: \n{obj}", "DETAILS", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
