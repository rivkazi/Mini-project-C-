using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading; 
using System.Windows;
using System.Windows.Controls;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        string MyID;
        IBL myBL;
        HostingUnit unit;
        ListBoxItem newItem;
        GuestRequest gr;
        HostingUnit hu;
        Order or;
        List<HostingUnit> HostingUnits;
        List<HostingUnit> MyHostingUnits;
        List<Order> Orders = new List<Order>();
        List<GuestRequest> LoadGR = new List<GuestRequest>();
        List<Order> MyOrders = new List<Order>();

        //bool isSend = false;
        //private Thread sendMail;

        public OrderWindow(string _HostID)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MyID = _HostID;
            gr = null;
            hu = null;
            or = null;
            myBL = BL.FactotyBL.GetBL();
            foreach(GuestRequest item in myBL.GetAllGuestRequests())//only if ther order is open
            {
                if (item.MyStatus == RequestStatus.Open)
                    LoadGR.Add((GuestRequest)item);
            }
            this.GuestRequestDataGrid.ItemsSource = LoadGR;
            this.comBoxStatus.ItemsSource = Enum.GetValues(typeof(RequestStatus));
            this.ComBoxType.ItemsSource = Enum.GetValues(typeof(Type));
            this.ComBoxArea.ItemsSource = Enum.GetValues(typeof(Area));

            HostingUnits = myBL.GetAllHostingUnits();
            Orders = myBL.GetAllOrders();

            this.comBoxChoosing.DisplayMemberPath = "MyHostingUnitName";
            this.comBoxChoosing.SelectedValuePath = "MyHostingUnitName";

            MyHostingUnits = new List<HostingUnit>();
            foreach (HostingUnit item in HostingUnits)
            {
                if (item.MyOwner.MyHostKey == int.Parse(MyID))
                    MyHostingUnits.Add(item);
            }
            this.comBoxChoosing.ItemsSource = MyHostingUnits;
            refreshWindow();
        }

        private void refreshWindow()
        {
            Orders = myBL.GetAllOrders();
            MyOrders = new List<Order>();
            lstBox.Items.Clear();
            foreach (Order item1 in Orders)
            {
                unit = myBL.GetHostingUnit(item1.MyHostingUnitKey);
                if (unit.MyOwner.MyHostKey == int.Parse(MyID) && item1.MyStatus == OrderStatus.Email_Sent)
                    MyOrders.Add(item1);
            }
            if (MyOrders != null)
            {
                foreach (Order item2 in MyOrders)//create listBox of orders
                {
                    HostingUnit h1 = myBL.GetHostingUnit(item2.MyHostingUnitKey);
                    string str = "OrderKey: " + item2.MyOrderKey + "  Guest Request Number: " + item2.MyGuestRequestKey +
                                 "  Hosting Unit Name: " + h1.MyHostingUnitName + "  Status: " + item2.MyStatus +
                                 "  Creation Date: " + item2.MyCreateDate;    
                    //if(item2.MyStatus == OrderStatus.Close_By_Awnser)
                    //    str += "  Commission Sum: " + item2.MyCommissionSum;

                    newItem = new ListBoxItem();
                    newItem.Content = str;
                    lstBox.Items.Add(newItem);
                }
            }
        }

        private void refreshRequests()
        {
            try
            {
                var v = from GuestRequest in myBL.GetAllGuestRequests()
                        where isUsed(GuestRequest.MyGuestRequestKey) == false
                        select GuestRequest;
                v.ToList();
                this.GuestRequestDataGrid.ItemsSource = v;
            }
            catch
            {
                this.Close();
            }
        }

        public bool isUsed(long guestRequestKey)
        {
            var v = from item in myBL.GetAllOrders()
                    let x = guestRequestKey
                    where item.MyGuestRequestKey == x
                    select item;
            if (v.ToList().Count() == 0)
                return false;
            return true;
        }

        #region stack panel
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
#endregion

        private void details_Click(object sender, RoutedEventArgs e)
        {
            GuestRequest gr = this.GuestRequestDataGrid.SelectedItem as GuestRequest;
            if (gr != null)
            {
                try
                {
                    MessageBox.Show($"Datails of Guest Request: \n{gr}", "DETAILS", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        #region Update Order
        private void lstBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.btnUpdate.Visibility = Visibility.Visible;
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem lItem = (ListBoxItem)lstBox.SelectedItem;
            string or = ((ListBoxItem)lstBox.SelectedItem).Content.ToString();
            string key = or.Substring(10, 8);
            Order order1 = myBL.GetOrder(int.Parse(key));
            gr = myBL.GetGuestRequest(order1.MyGuestRequestKey);
            hu = myBL.GetHostingUnit(order1.MyHostingUnitKey);
            //DateTime d = DateTime.Now;
            //if ((d - order1.MyCreateDate).Days > Configuration.NumDayForExpires)
            //{
            //    myBL.SetOrder(order1, OrderStatus.Close_By_MissAwnser);
            //    this.btnUpdate.Visibility = Visibility.Hidden;
            //    string name = hu.MyOwner.MyPrivateName;
            //    refreshWindow();
            //    MessageBox.Show(hu.MyOwner.MyPrivateName + ", Your Order Updated Successfuly", "SUCCESSFULL", MessageBoxButton.OK);  
            //}
            //else
            //{
                try
                {
                    myBL.SetOrder(order1, OrderStatus.Close_By_Awnser);
                    this.btnUpdate.Visibility = Visibility.Hidden;
                    string name = hu.MyOwner.MyPrivateName;
                    refreshWindow();
                    MessageBox.Show(name + ", Your Order Updated Successfuly", "SUCCESSFULL", MessageBoxButton.OK);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            //}
        }
        #endregion

        #region Add Order
        private void comBoxChoosing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.comBoxChoosing.SelectedItem is HostingUnit)
            {
                this.hu = ((HostingUnit)this.comBoxChoosing.SelectedItem);
            }
            if (gr != null && hu != null)
                this.btnAdd.Visibility = Visibility.Visible;
        }

        private void select_Click(object sender, RoutedEventArgs e)
        {
            this.gr = this.GuestRequestDataGrid.SelectedItem as GuestRequest;
            if (gr != null && hu != null)
                this.btnAdd.Visibility = Visibility.Visible;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            or = new Order();
            or.MyGuestRequestKey = gr.MyGuestRequestKey;
            or.MyHostingUnitKey = hu.MyHostingUnitKey;
            or.MyCreateDate = DateTime.Now;
            try
            {
                myBL.AddOrder(or);
                MailMessage mail = new MailMessage();

                mail.To.Add(gr.MyMailAdress);
                mail.From = new MailAddress("vacationer4you@gmail.com");
                mail.Subject = "Request Has Been Recieved";

                mail.Body = "<b> <p style='font-size:25px'>" + "Hello, " + gr.MyPrivateName + " " + gr.MyFamilyName +
                            "<b> <p style='color:red; font-size:25px'>My Hosting Unit Found Match To Your Request Details!" +
                            "<b> <p style='font-size:18px'> Your Vacation Dates Are Between: " + gr.MyEntryDate.ToString("dd/MM/yyyy") + " to " + gr.MyReleaseDate.ToString("dd/MM/yyyy") +
                            "<b> <p style='font-size:18px'>" + "My " + hu.MyHostingUnitName + " Hosting Unit Location is in " + hu.MyArea +
                            "<b> <p style='font-size:20px'> If your'e interested in my suggesting, Please contact me" +
                            "<b> <p style='font-size:18px'> My phone number is 0" + hu.MyOwner.MyFhoneNumber +
                            "<b> <p style='font-size:18px'> MyEmail Adress is " + hu.MyOwner.MyMailAddress +
                            "<b> <p style='color:red; 'font-size:18px'> I would love to have you as my guest" +
                            "<b> <p style='font-size:16px'>" + hu.MyOwner.MyPrivateName + " " + hu.MyOwner.MyFamilyName + " ,Your host" +
                            "<b> <p style='color:red; font-size:20px'>" + "Vacation4you"
                            ;

                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("vacationer4you@gmail.com", "Z0549219058");
                smtp.EnableSsl = true;
                try
                {
                    smtp.Send(mail);
                }
                catch (Exception ex1)
                {
                    throw ex1;
                }

                this.refreshRequests();//delete from the stackPanel
                this.refreshWindow();//adding to listBox
                gr = null;
                string name = hu.MyOwner.MyPrivateName;
                hu = null;
                or = null;
                this.comBoxChoosing.SelectedItem = null;
                this.GuestRequestDataGrid.SelectedItem = null;

                this.btnAdd.Visibility = Visibility.Hidden;
                MessageBox.Show(name + ", Your Order Added Successfuly", "SUCCESSFULL", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        ////Auxiliary function of send Email
        //void sd()
        //{
        //    while (isSend)
        //    {
        //        //MailMessage יצירת אובייקט
        //        MailMessage mail = new MailMessage();
        //        // כתובת הנמען)ניתן להוסיף יותר מאחד(
        //        mail.To.Add(gr.MyMailAdress);
        //        // הכתובת ממנה נשלח המייל
        //        mail.From = new MailAddress("zehorit10@gmail.com");
        //        // נושא ההודע ה
        //        mail.Subject = "מערכת אתר הזמנות נופשים";
        //        //( HTML תוכן ההודעה )נניח שתוכן ההודעה בפורמט
        //        mail.Body = "סוף סוך זה עובד";
        //        // HTML הגדרה שתוכן ההודעה בפורמט
        //        mail.IsBodyHtml = true;
        //        // Smtp יצירת עצם מסוג
        //        SmtpClient smtp = new SmtpClient();
        //        // gmail הגדרת השרת של
        //        smtp.Host = "smtp.gmail.com";
        //        // gmail הגדרת פרטי הכניסה )שם משתמש וסיסמה( לחשבון ה
        //        smtp.Credentials = new System.Net.NetworkCredential("vacationer4you@gmail.com", "Z0549219058");
        //        // SSL ע"פ דרישת השר, חובה לאפשר במקרה זה
        //        smtp.EnableSsl = true;
        //        try
        //        {
        //            // שליחת ההודע ה
        //            smtp.Send(mail);
        //        }
        //        catch (Exception ex1)
        //        {
        //            //- ש-ינ-ו-י טיפול בשגיאות ותפיסת חריגות
        //            //throw new Exception(ex1 + "");
        //            //txtMessage.Text = ex1.ToString();
        //            MessageBox.Show(ex1.ToString());

        //        }
        //        isSend = false;
        //    }
        //}
        #endregion

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
