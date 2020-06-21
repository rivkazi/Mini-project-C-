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
    /// Interaction logic for GetAllOrdersWindow.xaml
    /// </summary>
    public partial class GetAllOrdersWindow : Window
    {
        IBL MyBL;
        public GetAllOrdersWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MyBL = BL.FactotyBL.GetBL();
            this.OrderDataGrid.ItemsSource = MyBL.GetAllOrders();
            this.comBoxStatus.ItemsSource = Enum.GetValues(typeof(OrderStatus));
        }

        private void comBoxStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Clear.Visibility = Visibility.Visible;
            Filter();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            comBoxStatus.SelectedItem = null;
            this.Clear.Visibility = Visibility.Hidden;
        }

        List<Order> filtered = new List<Order>();
        // refreshing the list accorrding to user's filtering chooses
        private void Filter()
        {
            try
            {
                if(comBoxStatus.SelectedItem != null)
                {
                    var v = from item in MyBL.GetAllOrders()
                            let s = (OrderStatus)comBoxStatus.SelectedItem
                            where item.MyStatus == s
                            select item;
                    filtered = v.ToList();
                    this.OrderDataGrid.ItemsSource = filtered;
                    return;
                }
                this.OrderDataGrid.ItemsSource = MyBL.GetAllOrders();// if no choose has been made- call the original list
                return;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void AllDetails_Click(object sender, RoutedEventArgs e)
        {
            Order or = this.OrderDataGrid.SelectedItem as Order;
            if(or != null)
            {
                try
                {
                    MessageBox.Show($"Datails Of Order: \n{or}", "DETAILS", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
