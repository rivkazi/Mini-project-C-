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
    /// Interaction logic for GroupHostByHostingUserControl.xaml
    /// </summary>
    public partial class GroupHostByHostingUserControl : UserControl
    {
        IBL myBL = BL.FactotyBL.GetBL();
        List<Host> host;
        List<int> numbers = new List<int>();
        public GroupHostByHostingUserControl()
        {
            InitializeComponent();
            host = new List<Host>();
            getGrouping();
        }

        private void getGrouping()
        {
            var v = myBL.HostsGroupsByNumOfHostingUnits();
            foreach (var item in v)
            {
                numbers.Add(item.Key);
            }
            this.comBoxNum.ItemsSource = numbers;
        }

        private void comBoxNum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            host = new List<Host>();
            int keyH = Convert.ToInt32(this.comBoxNum.SelectedItem);
            var v = myBL.HostsGroupsByNumOfHostingUnits();
            foreach (var item in v)
            {
                if (item.Key == keyH)
                {
                    foreach (var x in item)
                    {
                        if (!HostExists(x))
                        {
                            host.Add(x);
                        }
                    }
                }
            }
            this.GroupsHostHostingDataGrid.ItemsSource = host;
        }

        private bool HostExists(Host _h)
        {
            foreach (Host item in host)
            {
                if (_h.MyHostKey == item.MyHostKey)
                {
                    return true;
                }
            }
            return false;
        }

        private void btnMoreDetails_Click(object sender, RoutedEventArgs e)
        {
            Host h = this.GroupsHostHostingDataGrid.SelectedItem as Host;
            if (h != null)
            {
                try
                {
                    MessageBox.Show($"Datails Of Guest Request: \n{h}", "DETAILS", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
