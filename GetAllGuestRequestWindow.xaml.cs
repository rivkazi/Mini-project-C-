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
    /// Interaction logic for GetAllGuestRequestWindow.xaml
    /// </summary>
    public partial class GetAllGuestRequestWindow : Window
    {
        IBL myBL;
        public GetAllGuestRequestWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            myBL = BL.FactotyBL.GetBL();
            this.GuestRequestDataGrid.ItemsSource = myBL.GetAllGuestRequests();
            this.comBoxStatus.ItemsSource = Enum.GetValues(typeof(RequestStatus));
            this.ComBoxType.ItemsSource = Enum.GetValues(typeof(Type));
            this.ComBoxArea.ItemsSource = Enum.GetValues(typeof(Area)); 
        }

        private void comBoxStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Clear.Visibility = Visibility.Visible;
            filter();
        }

        private void ComBoxType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Clear.Visibility = Visibility.Visible;
            filter();
        }

        private void ComBoxArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Clear.Visibility = Visibility.Visible;
            filter();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            comBoxStatus.SelectedItem = null;
            ComBoxType.SelectedItem = null;
            ComBoxArea.SelectedItem = null;
            this.Clear.Visibility = Visibility.Hidden;
        }



        List<GuestRequest> filtered = new List<GuestRequest>();
        // refreshing the list accorrding to user's filtering chooses
        private void filter()
        {
            try
            {
                if (comBoxStatus.SelectedItem != null && ComBoxType.SelectedItem != null && ComBoxArea.SelectedItem != null)
                {
                    var v = from item in myBL.GetAllGuestRequests()//filter by all three
                            let r = (RequestStatus)comBoxStatus.SelectedItem
                            let t = (Type)ComBoxType.SelectedItem
                            let a = (Area)ComBoxArea.SelectedItem
                            where item.MyStatus == r && item.MyType == t && item.MyArea == a
                            select item;
                    filtered = v.ToList();
                    this.GuestRequestDataGrid.ItemsSource = filtered;
                    return;
                }

                if (comBoxStatus.SelectedItem != null && ComBoxType.SelectedItem != null && ComBoxArea.SelectedItem == null)
                {
                    var v = from item in myBL.GetAllGuestRequests()//filter by status & type
                            let r = (RequestStatus)comBoxStatus.SelectedItem
                            let t = (Type)ComBoxType.SelectedItem
                            where item.MyStatus == r && item.MyType == t 
                            select item;
                    filtered = v.ToList();
                    this.GuestRequestDataGrid.ItemsSource = filtered;
                    return;
                }

                if (comBoxStatus.SelectedItem != null && ComBoxType.SelectedItem == null && ComBoxArea.SelectedItem != null)
                {
                    var v = from item in myBL.GetAllGuestRequests()//filter by status & area
                            let r = (RequestStatus)comBoxStatus.SelectedItem
                            let a = (Area)ComBoxArea.SelectedItem
                            where item.MyStatus == r && item.MyArea == a
                            select item;
                    filtered = v.ToList();
                    this.GuestRequestDataGrid.ItemsSource = filtered;
                    return;
                }

                if (comBoxStatus.SelectedItem == null && ComBoxType.SelectedItem != null && ComBoxArea.SelectedItem != null)
                {
                    var v = from item in myBL.GetAllGuestRequests()//filter by type & area
                            let t = (Type)ComBoxType.SelectedItem
                            let a = (Area)ComBoxArea.SelectedItem
                            where item.MyType == t && item.MyArea == a
                            select item;
                    filtered = v.ToList();
                    this.GuestRequestDataGrid.ItemsSource = filtered;
                    return;
                }

                if (comBoxStatus.SelectedItem != null && ComBoxType.SelectedItem == null && ComBoxArea.SelectedItem == null)//filter by status of the request
                {
                    var v = from item in myBL.GetAllGuestRequests()
                            let x = (RequestStatus)comBoxStatus.SelectedItem
                            where item.MyStatus == x
                            select item;
                    filtered = v.ToList();
                    this.GuestRequestDataGrid.ItemsSource = filtered;
                    return;
                }

                if (ComBoxType.SelectedItem != null)//filter by the wanted type of the unit requested
                {
                    var v = from item in myBL.GetAllGuestRequests()
                            let x = (Type)ComBoxType.SelectedItem
                            where item.MyType == x
                            select item;
                    filtered = v.ToList();
                    this.GuestRequestDataGrid.ItemsSource = filtered;
                    return;
                }

                if (ComBoxArea.SelectedItem != null)//filter by the wanted area of the request
                {
                    var v = from item in myBL.GetAllGuestRequests()
                            let x = (Area)ComBoxArea.SelectedItem
                            where item.MyArea == x
                            select item;
                    filtered = v.ToList();
                    this.GuestRequestDataGrid.ItemsSource = filtered;
                    return;
                }

                this.GuestRequestDataGrid.ItemsSource = myBL.GetAllGuestRequests();// if no choose has been made- call the original list
                return;
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void details_Click(object sender, RoutedEventArgs e)
        {
            GuestRequest gr = this.GuestRequestDataGrid.SelectedItem as GuestRequest;
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

