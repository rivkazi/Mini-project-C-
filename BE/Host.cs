using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Host
    {
        private int HostKey;
        private string PrivateName;
        private string FamilyName;
        private int FhoneNumber;
        private string MailAddress;
        private BankBranch BankBranchDetails;
        private int BankAccountNumber;
        private bool CollectionClearance;

        public int MyHostKey { get => HostKey; set => HostKey = value; }
        public string MyPrivateName { get => PrivateName; set => PrivateName = value; }
        public string MyFamilyName { get => FamilyName; set => FamilyName = value; }
        public int MyFhoneNumber { get => FhoneNumber; set => FhoneNumber = value; }
        public string MyMailAddress { get => MailAddress; set => MailAddress = value; }
        public BankBranch MyBankBranchDetails { get => BankBranchDetails; set => BankBranchDetails = value; }
        public int MyBankAccountNumber { get => BankAccountNumber; set => BankAccountNumber = value; }
        public bool MyCollectionClearance { get => CollectionClearance; set => CollectionClearance = value; }

        public override string ToString()
        {
            string result = "";
            result = "Host's I.D: " + MyHostKey + "\nHost's Private Name: " + MyPrivateName + "\nHost's Family Name: " + MyFamilyName +
                     "\nPhone Number: 0" + MyFhoneNumber + "\nMail Adress: " + MyMailAddress + "\nBranch Detailes: " + MyBankBranchDetails +
                     "\nBank Account's Number: " + MyBankAccountNumber + "\nCollection Clearance: " + MyCollectionClearance;
            return result;
        }
    }
}
