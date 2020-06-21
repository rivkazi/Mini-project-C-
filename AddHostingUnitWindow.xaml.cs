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
    /// Interaction logic for AddHostingUnitWindow.xaml
    /// </summary>
    public partial class AddHostingUnitWindow : Window
    { 
        IBL myBL = BL.FactotyBL.GetBL();
        HostingUnit hu;
        Host hst;
        BankBranch b;
        bool[,] Diary = new bool[31,12];
        List<BankBranch> bankBranches;
        public List<string> BankNameList; 

        public AddHostingUnitWindow(IEnumerable<BankBranch> _bankBranches)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            hu = new HostingUnit();
            hst = new Host();
            b = new BankBranch();
            this.HostingUnitDeatails.DataContext = hu;
            this.HostingUnitDeatails.DataContext = hst;
            this.HostingUnitDeatails.DataContext = b;
            this.cmBoxMyArea.ItemsSource = Enum.GetValues(typeof(Area));
            cmBoxMyArea.SelectedIndex = 0;
            bankBranches = _bankBranches.ToList();

            BankNameList = new List<string>();
            foreach (BankBranch item in bankBranches)
            {
                string x = item.MyBankName;
                if (nameExists(x) != true)
                    BankNameList.Add(x);
            }
            BankNameList.Remove(BankNameList[18]);
            BankNameList.Remove(BankNameList[15]);
            BankNameList.Remove(BankNameList[12]);
            BankNameList.Remove(BankNameList[10]);
            BankNameList.Remove(BankNameList[7]);
            BankNameList.Remove(BankNameList[5]);
            BankNameList.Remove(BankNameList[3]);
            this.cmbBoxMyBankName.ItemsSource = BankNameList;
        }
 
        private bool nameExists(string name)
        {
            foreach (string item in BankNameList)
                if (item == name)
                    return true;
            return false;
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

        private void myCollectionClearanceCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (ChckBoxMyCollectionClearance.IsChecked == true)
                hst.MyCollectionClearance = true;
            else
                hst.MyCollectionClearance = false;
        }
        #endregion

        #region Bank Branch Details
        int getBankNumber(string bankName)
        {
            BankBranch x = bankBranches.FirstOrDefault(y => y.MyBankName == bankName);
            return x.MyBankNumber;
        }

        List<int> BranchNumber;
        string name;
        private void cmbBoxMyBankName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.HostingUnitDeatails.DataContext = b;
            name = cmbBoxMyBankName.SelectedItem.ToString();
            b.MyBankName = name;
            b.MyBankNumber = getBankNumber(name);
            this.txtBoxMyBankNumber.Text = b.MyBankNumber.ToString();

            BranchNumber = new List<int>();
            foreach (BankBranch item in bankBranches)
            {
                if (item.MyBankName == name)
                {
                    if(numExists(item.MyBranchNumber) != true)
                    BranchNumber.Add(item.MyBranchNumber);
                }
            }
            this.cmbBoxMyBranchNumber.IsEnabled = true;
            this.cmbBoxMyBranchNumber.ItemsSource = BranchNumber;
        }

        private bool numExists(int num)
        {
            foreach (int item in BranchNumber)
                if (item == num)
                    return true;
            return false;
        }

        string getBranchAdress(int branchNumber)
        {
            BankBranch x = bankBranches.FirstOrDefault(y => y.MyBranchNumber == branchNumber && y.MyBankName == name);
            return x.MyBranchAddress;
        }

        string getBranchCity(int branchNumber)
        {
            BankBranch x = bankBranches.FirstOrDefault(y => y.MyBranchNumber == branchNumber && y.MyBankName == name);
            return x.MyBranchCity;
        }

        private void cmbBoxMyBranchNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.cmbBoxMyBankName.IsEnabled = false;
            int number2 = Convert.ToInt32(cmbBoxMyBranchNumber.SelectedItem);
            b.MyBranchNumber = number2;
            b.MyBranchAddress = getBranchAdress(number2);
            b.MyBranchCity = getBranchCity(number2);
            this.txtBoxMyBranchAddress.Text = b.MyBranchAddress.ToString();
            this.txtBoxMyBranchCity.Text = b.MyBranchCity.ToString();
        }

        #endregion

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        { 
            try
            {
                if ((string)HostingNameExp.Content != "" || (string)HostKeyExp.Content != "" || (string)PrivateNameExp.Content != ""
                     || (string)FamilyNameExp.Content != "" || (string)FhoneNumExp.Content != "" || (string)MailExp.Content != ""
                     || (string)BankAccountNumExp.Content != "")//there is exception for one of the fields
                    return;//still cant be added, fixing is needed

                if ((string)txtBoxMyHostingUnitName.Text == "" || (string)txtBoxMyHostKey.Text == "" || (string)txtBoxMyPrivateName.Text == "" ||
                    (string)txtBoxMyFamilyName.Text == "" || (string)txtBoxMyFhoneNumber.Text == "" || (string)txtBoxMyMailAddress.Text == ""
                    || (string)txtBoxMyBankAccountNumber.Text == "" || (string)txtBoxMyBankNumber.Text == ""
                    || (string)txtBoxMyBranchAddress.Text == "" || (string)txtBoxMyBranchCity.Text == "")
                {
                    MessageBox.Show("Please Fill All The Fields");
                    return;
                }

                hu.MyDiary = Diary;
                hu.MyOwner = hst;
                hst.MyBankBranchDetails = b;

                hu.MyArea = (Area)cmBoxMyArea.SelectedItem;
                hu.MyOwner.MyCollectionClearance = (bool)ChckBoxMyCollectionClearance.IsChecked;

                myBL.AddHostingUnit(hu);// all the fields are Ok so it can be added now
                MessageBox.Show(hu.MyHostingUnitName + " Hosting Unit was added Successfuly" , "SUCCESSFULL", MessageBoxButton.OK);

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
                txtBoxMyBranchAddress.Text = "";
                txtBoxMyBranchCity.Text = "";

                hu = new HostingUnit();
                hst = new Host();
                b = new BankBranch();
                this.HostingUnitDeatails.DataContext = hu;
                this.HostingUnitDeatails.DataContext = hst;
                this.HostingUnitDeatails.DataContext = b;
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
