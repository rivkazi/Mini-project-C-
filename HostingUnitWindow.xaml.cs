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

using System.Net;
using System.ComponentModel;
using System.Xml.Linq;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for HostingUnitWindow.xaml
    /// </summary>
    public partial class HostingUnitWindow : Window
    {
        public HostingUnitWindow()
        {
            loadBanksWorker = new BackgroundWorker();
            loadBanksWorker.DoWork += LoadBanks_DoWork;
            loadBanksWorker.RunWorkerCompleted += LoadBanks_completeWork;
            loadBanksWorker.RunWorkerAsync();//calling to doWork for loading the file

            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        bool isfinish = false;
        BackgroundWorker loadBanksWorker;
        const string xmlLocalPath = @"atm.xml";
        WebClient wc = new WebClient();
        private void LoadBanks_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string xmlServerPath = @"http://www.jct.ac.il/~coshri/atm.xml";
                wc.DownloadFile(xmlServerPath, xmlLocalPath);
                
                if(wc == null)
                    throw new Exception();
            }
            catch(Exception x)
            {
                string xmlServerPath = @"http://www.boi.org.il/he/BankingSupervision/BanksAndBranchLocations/Lists/BoiBankBranchesDocs/atm.xml";
                wc.DownloadFile(xmlServerPath, xmlLocalPath);
            }
            finally
            {
                wc.Dispose();
            }
        }

        private void LoadDataBankBranch()
        {
            try
            {
                BankBranchRoot = XElement.Load(BankBranchPath);
            }
            catch
            {
                throw new Exception("File upload problem");
            }
        }

        BankBranch ConvertBankBranch(XElement element)
        {
            return new BankBranch()
            {
                MyBankNumber = int.Parse(element.Element("קוד_בנק").Value),
                MyBankName = element.Element("שם_בנק").Value,
                MyBranchNumber = int.Parse(element.Element("קוד_סניף").Value),
                MyBranchAddress = element.Element("כתובת_ה-ATM").Value,
                MyBranchCity = element.Element("ישוב").Value
            };
        }

        XElement BankBranchRoot;
        string BankBranchPath;
        IEnumerable<BankBranch> BankList;
        private void LoadBanks_completeWork(object sender, RunWorkerCompletedEventArgs e)
        {
            BankBranchPath = @"atm.xml";
            BankBranchRoot = XElement.Load(BankBranchPath);

            BankList = from item in BankBranchRoot.Elements()
                       let a = ConvertBankBranch(item)
                       select a;
            isfinish = true;
            this.btnAdd.Visibility = Visibility.Visible; 
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (isfinish)
            {
                new AddHostingUnitWindow(BankList).ShowDialog();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new LogInWindow().ShowDialog();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
