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
using BL;
using BE;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for VisualOptionWindow.xaml
    /// </summary>
    public partial class VisualOptionWindow : Window
    {
        int myID;
        IBL myBL = BL.FactotyBL.GetBL();
        List<HostingUnit> MyHostingUnits = new List<HostingUnit>();
        HostingUnit hu = new HostingUnit();
        private Calendar MyCalendar;
        public VisualOptionWindow(string HostID)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            myID = Convert.ToInt32(HostID);
            foreach(HostingUnit item in myBL.GetAllHostingUnits())
            {
                if (item.MyOwner.MyHostKey == myID)
                    MyHostingUnits.Add(item);
            }
            this.comBoxChoosing.DisplayMemberPath = "MyHostingUnitName";
            this.comBoxChoosing.SelectedValuePath = "MyHostingUnitName";
            this.comBoxChoosing.ItemsSource = MyHostingUnits;
        }

        private void refreshCMBox()
        {
            MyHostingUnits = new List<HostingUnit>();
            foreach (HostingUnit item in myBL.GetAllHostingUnits())
            {
                if (item.MyOwner.MyHostKey == myID)
                    MyHostingUnits.Add(item);
            }
            this.comBoxChoosing.DisplayMemberPath = "MyHostingUnitName";
            this.comBoxChoosing.SelectedValuePath = "MyHostingUnitName";
            this.comBoxChoosing.ItemsSource = MyHostingUnits;
        }

        private void comBoxChoosing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.comBoxChoosing.SelectedItem is HostingUnit)
            {
                this.btnNumDays.Visibility = Visibility.Visible;
                this.btnPercentageDays.Visibility = Visibility.Visible;
                hu = null;
                this.hu = ((HostingUnit)this.comBoxChoosing.SelectedItem);
                this.HUdetails.DataContext = hu;
                MyCalendar = CreateCalendar();
                vbCalendar.Child = null;
                vbCalendar.Child = MyCalendar;
                SetBlackOutDates();
                refreshCMBox();
            }
        }

        //this function returns the sum of days are taken during the year
        public int GetAnnualBusyDays()
        {
            int counter = 0;
            int sumDays = 0;
            for (int i = 0; i < 12; i++)
            {
                if (i == 1)
                    sumDays = 28;
                if (i == 0 || i == 2 || i == 4 || i == 6 || i == 7 || i == 9 || i == 11)
                    sumDays = 31;
                else
                    sumDays = 30;
                for (int j = 0; j < sumDays; j++)
                {
                    if (hu.MyDiary[j , i] == true)
                        counter++;
                }
            }
            return counter;
        }

        public float GetAnnualBusyPrecentege()
        {
            int counter = GetAnnualBusyDays();
            float precent = ((float)counter / 365) * 100;
            return precent;
        }

        private void NumDays_Click(object sender, RoutedEventArgs e)
        {
            int num = GetAnnualBusyDays();
            MessageBox.Show("My Number Of Busy Days Is: " + num, "BUSY DAYS", MessageBoxButton.OK);
        }

        private void PercentageDays_Click(object sender, RoutedEventArgs e)
        {
            float num = GetAnnualBusyPrecentege();
            MessageBox.Show("My Percentage Of Busy Days Is: " + num, "BUSY DAYS PERCENTAGE", MessageBoxButton.OK);
        }

        private Calendar CreateCalendar()
        {
            Calendar MonthlyCalendar = new Calendar();
            MonthlyCalendar.Name = "MonthlyCalendar";
            MonthlyCalendar.DisplayMode = CalendarMode.Month;
            MonthlyCalendar.IsTodayHighlighted = true;
            return MonthlyCalendar;
        }

        private void SetBlackOutDates()
        {
            DateTime d = new DateTime();
            int sumDays = 0;
            for (int i = 0; i < 12; i++)
            {
                if (i == 1)
                    sumDays = 28;
                if (i == 0 || i == 2 || i == 4 || i == 6 || i == 7 || i == 9 || i == 11)
                    sumDays = 31;
                else if (i == 1 || i == 3 || i == 5 || i == 8 || i == 10 || i == 12)
                    sumDays = 30;

                for (int j = 0; j < sumDays; j++)
                {
                    if (hu.MyDiary[j, i] == true)
                    {
                        d = new DateTime(2020, i, j);
                        MyCalendar.BlackoutDates.Add(new CalendarDateRange(d));
                    }
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
