using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class BankBranch
    {
        private int BankNumber;
        private string BankName;
        private int BranchNumber;
        private string BranchAddress;
        private string BranchCity;

        public int MyBankNumber { get => BankNumber; set => BankNumber = value; }
        public string MyBankName { get => BankName; set => BankName = value; }
        public int MyBranchNumber { get => BranchNumber; set => BranchNumber = value; }
        public string MyBranchAddress { get => BranchAddress; set => BranchAddress = value; }
        public string MyBranchCity { get => BranchCity; set => BranchCity = value; }

        public override string ToString()
        {
            string result = "";
            result = "Bank Number: " + MyBankNumber + "\nBank Name: " + MyBankName + "\nBranch Number: " + MyBranchNumber +
                     "\nBranch Adress: " + MyBranchAddress + "\nBranch City: " + MyBranchCity;
            return result;
        }

    }
}
