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
    /// Interaction logic for AddGuestRequestWindow.xaml
    /// </summary>
    public partial class AddGuestRequestWindow : Window
    {
        IBL myBL = BL.FactotyBL.GetBL();
        GuestRequest gr;

        public AddGuestRequestWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            gr = new GuestRequest();
            this.GuestRequestDeatails.DataContext = gr;
            this.comBoxMyStatus.ItemsSource = Enum.GetValues(typeof(RequestStatus));
            comBoxMyStatus.SelectedIndex = 0;
            this.comBoxMyArea.ItemsSource = Enum.GetValues(typeof(Area));
            comBoxMyArea.SelectedIndex = 0;
            this.comBoxMyType.ItemsSource = Enum.GetValues(typeof(Type));
            comBoxMyType.SelectedIndex = 0;
            this.comBoxMyPool.ItemsSource = Enum.GetValues(typeof(Interest));
            comBoxMyPool.SelectedIndex = 0;
            this.comBoxMyJacuzzi.ItemsSource = Enum.GetValues(typeof(Interest));
            comBoxMyJacuzzi.SelectedIndex = 0;
            this.comBoxMyGarden.ItemsSource = Enum.GetValues(typeof(Interest));
            comBoxMyGarden.SelectedIndex = 0;
            this.comBoxMyChildrensAttractions.ItemsSource = Enum.GetValues(typeof(Interest));
            comBoxMyChildrensAttractions.SelectedIndex = 0;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource guestRequestViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("guestRequestViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // guestRequestViewSource.Source = [generic data source]
        }

        private void comBoxMyArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.comBoxMyArea.SelectedIndex == 0)
            {
                this.comBoxMySubArea.ItemsSource = Enum.GetValues(typeof(North));
                comBoxMySubArea.SelectedIndex = 0;
            }
            if (this.comBoxMyArea.SelectedIndex == 1)
            {
                this.comBoxMySubArea.ItemsSource = Enum.GetValues(typeof(Center));
                comBoxMySubArea.SelectedIndex = 0;
            }
            if (this.comBoxMyArea.SelectedIndex == 2)
            {
                this.comBoxMySubArea.ItemsSource = Enum.GetValues(typeof(South));
                comBoxMySubArea.SelectedIndex = 0;
            }
        }

        private void datePMyEntryDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            gr.MyEntryDate = Convert.ToDateTime(datePMyEntryDate.SelectedDate);
        }
        private void datePMyReleaseDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            gr.MyReleaseDate = Convert.ToDateTime(datePMyReleaseDate.SelectedDate);
        }

        private void txtBoxMyPrivateName_GotFocus(object sender, RoutedEventArgs e)
        {
            PrivateNameExp.Content = "";
            txtBoxMyPrivateName.BorderBrush = Brushes.Black;
        }
        private void txtBoxMyPrivateName_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Tools.stringCheck(txtBoxMyPrivateName.Text))
                {
                    gr.MyPrivateName = txtBoxMyPrivateName.Text;
                }
                else
                    throw new UnPossibleSelection(gr, "Name Must Include Letters Only!");
            }
            catch (Exception exp)
            {
                txtBoxMyPrivateName.BorderBrush = Brushes.Red;
                PrivateNameExp.Content = exp.Message;
            }
        }

        private void txtBoxMyFamilyName_GotFocus(object sender, RoutedEventArgs e)
        {
            FamilyNameExp.Content = "";
            txtBoxMyFamilyName.BorderBrush = Brushes.Black;
        }
        private void txtBoxMyFamilyName_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Tools.stringCheck(txtBoxMyFamilyName.Text) == true)
                {
                    gr.MyFamilyName = txtBoxMyFamilyName.Text;
                }
                else
                    throw new UnPossibleSelection(gr, "Name Must Include Letters Only!");
            }
            catch(Exception exp)
            {
                txtBoxMyFamilyName.BorderBrush = Brushes.Red;
                FamilyNameExp.Content = exp.Message;
            }
        }

        private void txtBoxMyMailAdress_GotFocus(object sender, RoutedEventArgs e)
        {
            MailAdressExp.Content = "";
            txtBoxMyMailAdress.BorderBrush = Brushes.Black;
        }
        private void txtBoxMyMailAdress_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Tools.emailCheck(txtBoxMyMailAdress.Text) == true)
                {
                    gr.MyMailAdress = txtBoxMyMailAdress.Text;
                }
                else
                    throw new UnPossibleSelection(gr, "This Email Pattern Doesn't Exceptable!");
            }
            catch (Exception exp)
            {
                txtBoxMyMailAdress.BorderBrush = Brushes.Red;
                MailAdressExp.Content = exp.Message;
            }
        }

        private void txtBoxMyAdults_GotFocus(object sender, RoutedEventArgs e)
        {
            AdultsExp.Content = "";
            txtBoxMyAdults.BorderBrush = Brushes.Black;
        }
        private void txtBoxMyAdults_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Tools.numberCheck(txtBoxMyAdults.Text) == true)
                {
                    gr.MyAdults = int.Parse(txtBoxMyAdults.Text);
                }
                else
                    throw new UnPossibleSelection(gr, "Number Must Include Digits Only!");

            }
            catch (Exception exp)
            {
                txtBoxMyAdults.BorderBrush = Brushes.Red;
                AdultsExp.Content = exp.Message;
            }
        }

        private void txtBoxMyChildren_GotFocus(object sender, RoutedEventArgs e)
        {
            ChildrenExp.Content = "";
            txtBoxMyChildren.BorderBrush = Brushes.Black;
        }
        private void txtBoxMyChildren_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Tools.numberCheck(txtBoxMyAdults.Text) == true)
                {
                    gr.MyChildren = int.Parse(txtBoxMyChildren.Text);
                }
                else
                    throw new UnPossibleSelection(gr, "Number Must Include Digits Only!");
            }
            catch (Exception exp)
            {
                txtBoxMyChildren.BorderBrush = Brushes.Red;
                ChildrenExp.Content = exp.Message;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DateTime d = DateTime.Now;
            gr.MyRegistrationDate = d;
            try
            {
                if ((string)PrivateNameExp.Content != "" || (string)FamilyNameExp.Content != "" || (string)MailAdressExp.Content != "" ||
                    (string)AdultsExp.Content != "" || (string)ChildrenExp.Content != "") //there is exception for one of the fields
                    return; //still cant be added, fixing is needed

                if ((string)txtBoxMyPrivateName.Text == "" || (string)txtBoxMyFamilyName.Text == "" || (string)txtBoxMyMailAdress.Text == "" ||
                    (string)txtBoxMyAdults.Text == "" || (string)txtBoxMyChildren.Text == "")
                {
                    MessageBox.Show("Please Fill All The Fields");
                    return;
                }

                gr.MyStatus = (RequestStatus)comBoxMyStatus.SelectedItem;
                gr.MyArea = (Area)comBoxMyArea.SelectedItem;
                if(gr.MyArea == Area.North)
                {
                    gr.MySubArea = ((North)comBoxMySubArea.SelectedItem).ToString();
                }
                if (gr.MyArea == Area.Center)
                {
                    gr.MySubArea = ((Center)comBoxMySubArea.SelectedItem).ToString();
                }
                if (gr.MyArea == Area.South)
                {
                    gr.MySubArea = ((South)comBoxMySubArea.SelectedItem).ToString();
                }

                gr.MyType = (Type)comBoxMyType.SelectedItem;
                gr.MyPool = (Interest)comBoxMyPool.SelectedItem;
                gr.MyJacuzzi = (Interest)comBoxMyJacuzzi.SelectedItem;
                gr.MyGarden = (Interest)comBoxMyGarden.SelectedItem;
                gr.MyChildrensAttractions = (Interest)comBoxMyChildrensAttractions.SelectedItem;

                myBL.AddGuestRequest(gr); // all the fields are Ok so it can be added now
                MessageBox.Show(gr.MyPrivateName + ", Your Guest Request added Successfuly", "SUCCESSFULL", MessageBoxButton.OK);

                //reputting default valuest in the tools
                txtBoxMyPrivateName.Text = "";
                txtBoxMyFamilyName.Text = "";
                txtBoxMyMailAdress.Text = "";
                comBoxMyStatus.SelectedIndex = 0; 
                datePMyEntryDate.SelectedDate = DateTime.Today;
                datePMyReleaseDate.SelectedDate = DateTime.Today;
                comBoxMyArea.SelectedIndex = 0;
                comBoxMySubArea.SelectedIndex = 0;
                comBoxMyType.SelectedIndex = 0;
                txtBoxMyAdults.Text = "";
                txtBoxMyChildren.Text = "";
                comBoxMyPool.SelectedIndex = 0;
                comBoxMyJacuzzi.SelectedIndex = 0;
                comBoxMyGarden.SelectedIndex = 0;
                comBoxMyChildrensAttractions.SelectedIndex = 0;

                gr = new GuestRequest();
                this.GuestRequestDeatails.DataContext = gr;
                this.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
