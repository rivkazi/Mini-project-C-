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
    /// Interaction logic for GroupGuestByAreasUserControl.xaml
    /// </summary>
    public partial class GroupGuestByAreasUserControl : UserControl
    {
        IBL myBL = BL.FactotyBL.GetBL();
        List<GuestRequest> guestRequests;
        public GroupGuestByAreasUserControl()
        {
            InitializeComponent();
            this.comBoxArea.ItemsSource = Enum.GetValues(typeof(Area));
            guestRequests = new List<GuestRequest>();
        }

        private void comBoxArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            guestRequests = new List<GuestRequest>();
            Area a = (Area)this.comBoxArea.SelectedItem;
            var v = myBL.GuestRequestGroupsByAreas();
            foreach(var item in v)
            {
                if(item.Key == a)
                {
                    foreach(var x in item)
                    {
                        guestRequests.Add(x);
                    }
                }
            }
            this.GroupsGuestAreasDataGrid.ItemsSource = guestRequests;
        }

        private void btnMoreDetails_Click(object sender, RoutedEventArgs e)
        {
            GuestRequest gr = this.GroupsGuestAreasDataGrid.SelectedItem as GuestRequest;
            if (gr != null)
            {
                try
                {
                    MessageBox.Show($"Datails Of Guest Request: \n{gr}", "DETAILS", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
