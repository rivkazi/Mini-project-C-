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
    /// Interaction logic for MoreOptionsWindow.xaml
    /// </summary>
    public partial class MoreOptionsWindow : Window
    {
        IBL myBL = BL.FactotyBL.GetBL();
        public MoreOptionsWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void btnGRlst_Click(object sender, RoutedEventArgs e)
        {
            new GetAllGuestRequestWindow().ShowDialog();
        }

        private void btnHUlst_Click(object sender, RoutedEventArgs e)
        {
            new GetAllHostingUnitsWindow().ShowDialog();
        }

        private void btnORlst_Click(object sender, RoutedEventArgs e)
        {
            new GetAllOrdersWindow().ShowDialog();
        }

        private void BtnGuestByArea_Click(object sender, RoutedEventArgs e)
        {
            GroupGuestByAreasUserControl uc = new GroupGuestByAreasUserControl();
            this.page.Content = uc;
        }

        private void btnGuestByPeople_Click(object sender, RoutedEventArgs e)
        {
            GroupGuestByPeopleUserControl uc = new GroupGuestByPeopleUserControl();
            this.page.Content = uc;
        }

        private void btnHostingByArea_Click(object sender, RoutedEventArgs e)
        {
            GroupHostingByAreaUserControl uc = new GroupHostingByAreaUserControl();
            this.page.Content = uc;
        }

        private void btnHostByHosting_Click(object sender, RoutedEventArgs e)
        {
            GroupHostByHostingUserControl uc = new GroupHostByHostingUserControl();
            this.page.Content = uc;
        } 
    }
}
