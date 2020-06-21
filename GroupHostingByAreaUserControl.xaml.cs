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
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for GroupHostingByAreaUserControl.xaml
    /// </summary>
    public partial class GroupHostingByAreaUserControl : UserControl
    {
        IBL myBL = BL.FactotyBL.GetBL();
        List<HostingUnit> hostingUnits;
        public GroupHostingByAreaUserControl()
        {
            InitializeComponent();
            this.comBoxArea.ItemsSource = Enum.GetValues(typeof(Area));
            hostingUnits = new List<HostingUnit>();
        }

        private void comBoxArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            hostingUnits = new List<HostingUnit>();
            Area a = (Area)this.comBoxArea.SelectedItem;
            var v = myBL.HostingUnitGroupsByAreas();
            foreach (var item in v)
            {
                if (item.Key == a)
                {
                    foreach (var x in item)
                    {
                        hostingUnits.Add(x);
                    }
                }
            }
            this.GroupsHostingAreasDataGrid.ItemsSource = hostingUnits;
        }

        private void btnMoreDetails_Click(object sender, RoutedEventArgs e)
        {
            HostingUnit hu = this.GroupsHostingAreasDataGrid.SelectedItem as HostingUnit;
            if (hu != null)
            {
                try
                {
                    MessageBox.Show($"Datails Of Guest Request: \n{hu}", "DETAILS", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
