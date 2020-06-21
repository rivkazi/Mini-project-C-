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
using System.Xml.Linq;
using BE;
using BL;

namespace PLWPF 
{
    /// <summary>
    /// Interaction logic for UpdateHostingUnitWindow.xaml
    /// </summary>
    public partial class UpdateHostingUnitWindow : Window
    {
        string myID;
        List<HostingUnit> HostingUnits;
        List<HostingUnit> MyHostingUnits;
        IBL myBL = BL.FactotyBL.GetBL();
        HostingUnit hu;
        HostingUnit checkHU;
        Host hst;
        BankBranch b;
        bool[,] Diary = new bool[31, 12];

        public UpdateHostingUnitWindow(string _HostID)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            myID = _HostID;
            hu = null;
            hst = null;
            b = null;
            this.cmBoxMyArea.ItemsSource = Enum.GetValues(typeof(Area));

            HostingUnits = myBL.GetAllHostingUnits();

            this.comBoxChoosing.DisplayMemberPath = "MyHostingUnitName";
            this.comBoxChoosing.SelectedValuePath = "MyHostingUnitName";

            MyHostingUnits = new List<HostingUnit>();
            foreach (HostingUnit item in HostingUnits)
            {
                if (item.MyOwner.MyHostKey == int.Parse(myID)) 
                    MyHostingUnits.Add(item);
            }
            this.comBoxChoosing.ItemsSource = MyHostingUnits;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource hostingUnitViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("hostingUnitViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // hostingUnitViewSource.Source = [generic data source]
            System.Windows.Data.CollectionViewSource bankBranchViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("bankBranchViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // bankBranchViewSource.Source = [generic data source]
        }

        private void comBoxChoosing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.comBoxChoosing.SelectedItem is HostingUnit)
            {
                this.hu = ((HostingUnit)this.comBoxChoosing.SelectedItem);
                this.checkHU = hu;
                this.hst = hu.MyOwner;
                this.b = hst.MyBankBranchDetails;
                btnUpdate.IsEnabled = true;
                setHostingUnitFields();
            }
        }

        private void setHostingUnitFields()
        {
            this.HostingUnitDeatails.DataContext = hu;
            this.HostingUnitDeatails.DataContext = hst;
            this.HostingUnitDeatails.DataContext = b;

            txtBoxMyHostingUnitKey.Text = hu.MyHostingUnitKey.ToString();
            txtBoxMyHostingUnitName.Text = hu.MyHostingUnitName;
            cmBoxMyArea.SelectedItem = hu.MyArea;

            hst = hu.MyOwner;
            txtBoxMyHostKey.Text = hst.MyHostKey.ToString();
            txtBoxMyPrivateName.Text = hst.MyPrivateName;
            txtBoxMyFamilyName.Text = hst.MyFamilyName;
            txtBoxMyFhoneNumber.Text = hst.MyFhoneNumber.ToString();
            txtBoxMyMailAddress.Text = hst.MyMailAddress;
            txtBoxMyBankAccountNumber.Text = hst.MyBankAccountNumber.ToString();
            ChckBoxMyCollectionClearance.IsChecked = hst.MyCollectionClearance;

            b = hst.MyBankBranchDetails;
            txtBoxMyBankNumber.Text = b.MyBankNumber.ToString();
            txtBoxMyBankName.Text = b.MyBankName;
            txtBoxMyBranchNumber.Text = b.MyBranchNumber.ToString();
            txtBoxMyBranchAddress.Text = b.MyBranchAddress;
            txtBoxMyBranchCity.Text = b.MyBranchCity;

            for (int i = 0; i < 31; i++)
                for (int j = 0; j < 12; j++)
                {
                    Diary[i, j] = hu.MyDiary[i, j];
                }
        }

        bool RemainTheSame()
        {
            if (txtBoxMyHostingUnitName.Text == checkHU.MyHostingUnitName && (Area)cmBoxMyArea.SelectedItem == checkHU.MyArea &&
               txtBoxMyHostKey.Text == checkHU.MyOwner.MyHostKey.ToString() && txtBoxMyPrivateName.Text == checkHU.MyOwner.MyPrivateName &&
               txtBoxMyFamilyName.Text == checkHU.MyOwner.MyFamilyName && txtBoxMyFhoneNumber.Text == checkHU.MyOwner.MyFhoneNumber.ToString() &&
               txtBoxMyMailAddress.Text == checkHU.MyOwner.MyMailAddress && txtBoxMyBankAccountNumber.Text == checkHU.MyOwner.MyBankAccountNumber.ToString() &&
               ChckBoxMyCollectionClearance.IsChecked == checkHU.MyOwner.MyCollectionClearance)
                return true;
            return false;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((string)HostingNameExp.Content != "" || (string)HostKeyExp.Content != "" || (string)PrivateNameExp.Content != ""
                     || (string)FamilyNameExp.Content != "" || (string)FhoneNumExp.Content != "" || (string)MailExp.Content != ""
                     || (string)BankAccountNumExp.Content != "")//there is exception for one of the fields
                {
                    MessageBox.Show($"Not All Files Are Correct");
                    return;//still cant be added, fixing is needed
                }

                if (RemainTheSame() == true)
                {
                    MessageBox.Show($"There Haven't Been a Change");
                    return;
                }

                for (int i = 0; i < 31; i++)
                    for (int j = 0; j < 12; j++)
                    {
                        hu.MyDiary[i, j]= Diary[i,j];
                    }

                hu.MyArea = (Area)cmBoxMyArea.SelectedItem;
                hu.MyOwner.MyCollectionClearance = (bool)ChckBoxMyCollectionClearance.IsChecked;

                myBL.SetHostingUnit(hu);
                MessageBox.Show(hu.MyHostingUnitName + " Hosting Unit Was Successfully Updated", "SUCCESSFULL", MessageBoxButton.OK);

                //reputting default valuest in the tools
                txtBoxMyHostingUnitName.Text = "";
                cmBoxMyArea.SelectedIndex = 0;

                txtBoxMyHostKey.Text = "";
                txtBoxMyPrivateName.Text = "";
                txtBoxMyFamilyName.Text = "";
                txtBoxMyFhoneNumber.Text = "";
                txtBoxMyMailAddress.Text = "";
                txtBoxMyBankAccountNumber.Text = "";
                ChckBoxMyCollectionClearance.IsChecked = false;

                txtBoxMyBankNumber.Text = "";
                txtBoxMyBankName.Text = "";
                txtBoxMyBranchNumber.Text = "";
                txtBoxMyBranchAddress.Text = "";
                txtBoxMyBranchCity.Text = "";

                this.HostingUnitDeatails.DataContext = hu = null;
                this.HostingUnitDeatails.DataContext = hst = null;
                this.HostingUnitDeatails.DataContext = b = null;

                this.comBoxChoosing.ItemsSource = myBL.GetAllHostingUnits();
                this.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }


        #region Hosting Unit details
        private void txtBoxMyHostingUnitName_GotFocus(object sender, RoutedEventArgs e)
        {
            HostingNameExp.Content = "";
            txtBoxMyHostingUnitName.BorderBrush = Brushes.Black;
        }
        private void txtBoxMyHostingUnitName_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Tools.stringCheck(txtBoxMyHostingUnitName.Text))
                {
                    hu.MyHostingUnitName = txtBoxMyHostingUnitName.Text;
                }
                else
                    throw new UnPossibleSelection(hu, "Name Must Include Letters Only!");
            }
            catch (Exception exp)
            {
                txtBoxMyHostingUnitName.BorderBrush = Brushes.Red;
                HostingNameExp.Content = exp.Message;
            }
        }
        #endregion

        #region Host Details
        private void myHostKeyTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            HostKeyExp.Content = "";
            txtBoxMyHostKey.BorderBrush = Brushes.Black;
        }
        private void myHostKeyTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Tools.checkID(txtBoxMyHostKey.Text) == true)
                {
                    hst.MyHostKey = int.Parse(txtBoxMyHostKey.Text);
                }
                else
                    throw new UnPossibleSelection(hu, "ID Must Include Digits Only, Nine Digits!");
            }
            catch (Exception exp)
            {
                txtBoxMyHostKey.BorderBrush = Brushes.Red;
                HostKeyExp.Content = exp.Message;
            }
        }

        private void myPrivateNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PrivateNameExp.Content = "";
            txtBoxMyPrivateName.BorderBrush = Brushes.Black;
        }
        private void myPrivateNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Tools.stringCheck(txtBoxMyPrivateName.Text))
                {
                    hst.MyPrivateName = txtBoxMyPrivateName.Text;
                }
                else
                    throw new UnPossibleSelection(hu, "Name Must Include Letters Only!");
            }
            catch (Exception exp)
            {
                txtBoxMyPrivateName.BorderBrush = Brushes.Red;
                PrivateNameExp.Content = exp.Message;
            }
        }

        private void myFamilyNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            FamilyNameExp.Content = "";
            txtBoxMyFamilyName.BorderBrush = Brushes.Black;
        }
        private void myFamilyNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Tools.stringCheck(txtBoxMyFamilyName.Text) == true)
                {
                    hst.MyFamilyName = txtBoxMyFamilyName.Text;
                }
                else
                    throw new UnPossibleSelection(hu, "Name Must Include Letters Only!");
            }
            catch (Exception exp)
            {
                txtBoxMyFamilyName.BorderBrush = Brushes.Red;
                FamilyNameExp.Content = exp.Message;
            }
        }

        private void myFhoneNumberTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            FhoneNumExp.Content = "";
            txtBoxMyFhoneNumber.BorderBrush = Brushes.Black;
        }

        private void myFhoneNumberTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Tools.phoneCheck(txtBoxMyFhoneNumber.Text) == true)
                {
                    hst.MyFhoneNumber = int.Parse(txtBoxMyFhoneNumber.Text);
                }
                else
                    throw new UnPossibleSelection(hst, "Phone Number Must Include Digits Only, With Perfix '05'!");
            }
            catch (Exception exp)
            {
                txtBoxMyFhoneNumber.BorderBrush = Brushes.Red;
                FhoneNumExp.Content = exp.Message;
            }
        }

        private void myMailAddressTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            MailExp.Content = "";
            txtBoxMyMailAddress.BorderBrush = Brushes.Black;
        }
        private void myMailAddressTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Tools.emailCheck(txtBoxMyMailAddress.Text) == true)
                {
                    hst.MyMailAddress = txtBoxMyMailAddress.Text;
                }
                else
                    throw new UnPossibleSelection(hst, "This Email Pattern Doesn't Exceptable!");
            }
            catch (Exception exp)
            {
                txtBoxMyMailAddress.BorderBrush = Brushes.Red;
                MailExp.Content = exp.Message;
            }
        }

        private void myBankAccountNumberTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            BankAccountNumExp.Content = "";
            txtBoxMyBankAccountNumber.BorderBrush = Brushes.Black;
        }
        private void myBankAccountNumberTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Tools.numberCheck(txtBoxMyBankAccountNumber.Text) == true)
                {
                    hst.MyBankAccountNumber = int.Parse(txtBoxMyBankAccountNumber.Text);
                }
                else
                    throw new UnPossibleSelection(hst, "Number Must Include Digits Only!");
            }
            catch (Exception exp)
            {
                txtBoxMyBankAccountNumber.BorderBrush = Brushes.Red;
                BankAccountNumExp.Content = exp.Message;
            }
        }
        #endregion
        
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}
