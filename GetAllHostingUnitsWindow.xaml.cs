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
    /// Interaction logic for GetAllHostingUnitsWindow.xaml
    /// </summary>
    public partial class GetAllHostingUnitsWindow : Window
    {
        IBL MyBL;
        public GetAllHostingUnitsWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MyBL = BL.FactotyBL.GetBL();
            this.HostingUnitDataGrid.ItemsSource = MyBL.GetAllHostingUnits();
            this.comBoxArea.ItemsSource = Enum.GetValues(typeof(Area));
        }

        private void comBoxArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Clear.Visibility = Visibility.Visible;
            Filter();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            this.comBoxArea.SelectedItem = null;
            this.Clear.Visibility = Visibility.Hidden;
        }

        List<HostingUnit> filtered = new List<HostingUnit>();
        private void Filter()
        {
            try
            {
                if(comBoxArea.SelectedItem != null)
                {
                    var v = from item in MyBL.GetAllHostingUnits()
                            let a = (Area)comBoxArea.SelectedItem
                            where item.MyArea == a
                            select item;
                    filtered = v.ToList();
                    this.HostingUnitDataGrid.ItemsSource = filtered;
                    return;
                }

                this.HostingUnitDataGrid.ItemsSource = MyBL.GetAllHostingUnits();// if no choose has been made- call the original list
                return;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void AllDetails_Click(object sender, RoutedEventArgs e)
        {
            HostingUnit hu = this.HostingUnitDataGrid.SelectedItem as HostingUnit;
            if(hu != null)
            {
                try
                {
                    MessageBox.Show($"Datails Of Hosting Unit: \n{hu}", "DETAILS", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
