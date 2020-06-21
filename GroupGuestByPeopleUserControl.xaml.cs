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
using System.Collections;
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for GroupGuestByPeopleUserControl.xaml
    /// </summary>
    public partial class GroupGuestByPeopleUserControl : UserControl
    {
        IBL myBL = BL.FactotyBL.GetBL();
        List<GuestRequest> guestRequests;
        List<int> Vacationers = new List<int>();

        public GroupGuestByPeopleUserControl()
        {
            InitializeComponent();
            guestRequests = new List<GuestRequest>();
            getGrouping();
        }
   
        private void getGrouping()
        {
            var v = myBL.GuestRequesGroupstByNumOfPeople();
            foreach(var item in v)
            {
                Vacationers.Add(item.Key);
            }
            this.comBoxNum.ItemsSource = Vacationers;
        }

        private void comBoxNum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            guestRequests = new List<GuestRequest>();
            int keyG = Convert.ToInt32(this.comBoxNum.SelectedItem);
            var v = myBL.GuestRequesGroupstByNumOfPeople();
            foreach (var item in v)
            {
                if (item.Key == keyG)
                {
                    foreach (var x in item)
                    {
                        guestRequests.Add(x);
                    }  
                }
            }
            this.GroupsGuestPeopleDataGrid.ItemsSource = guestRequests;
        }

        private void btnMoreDetails_Click(object sender, RoutedEventArgs e)
        {
            GuestRequest gr = this.GroupsGuestPeopleDataGrid.SelectedItem as GuestRequest;
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
