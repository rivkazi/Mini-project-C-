using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using System.Windows;

namespace BE
{
    public class GuestRequest//: DependencyObject
    {
        private long GuestRequestKey;
        private string PrivateName;
        private string FamilyName;
        private string MailAdress;
        private RequestStatus Status;
        private DateTime RegistrationDate;
        private DateTime EntryDate;
        private DateTime ReleaseDate;
        private Area Area;
        private string SubArea;
        private Type Type;
        private int Adults;
        private int Children;
        private Interest Pool;
        private Interest Jacuzzi;
        private Interest Garden;
        private Interest ChildrensAttractions;

        public long MyGuestRequestKey { get => GuestRequestKey; set => GuestRequestKey = value; }
        public string MyPrivateName { get => PrivateName; set => PrivateName = value; }
        public string MyFamilyName { get => FamilyName; set => FamilyName = value; }
        public string MyMailAdress { get => MailAdress; set => MailAdress = value; }
        public RequestStatus MyStatus { get => Status; set => Status = value; }
        public DateTime MyRegistrationDate { get => RegistrationDate; set => RegistrationDate = value; }
        public DateTime MyEntryDate { get => EntryDate; set => EntryDate = value; }
        public DateTime MyReleaseDate { get => ReleaseDate; set => ReleaseDate = value; }
        public Area MyArea { get => Area; set => Area = value; }
        public string MySubArea { get => SubArea; set => SubArea = value; }
        public Type MyType { get => Type; set => Type = value; }
        public int MyAdults { get => Adults; set => Adults = value; }
        public int MyChildren { get => Children; set => Children = value; }
        public Interest MyPool { get => Pool; set => Pool = value; }
        public Interest MyJacuzzi { get => Jacuzzi; set => Jacuzzi = value; }
        public Interest MyGarden { get => Garden; set => Garden = value; }
        public Interest MyChildrensAttractions { get => ChildrensAttractions; set => ChildrensAttractions = value; }

        /// <summary>
        /// return rather two guest request are the same
        /// </summary>
        /// <param name="gs1"></param>
        /// <param name="gs2"></param>
        /// <returns></returns>
        public bool isSame(GuestRequest gs1, GuestRequest gs2)
        {
            if (gs1.MyPrivateName == gs2.MyPrivateName && gs1.MyFamilyName == gs2.MyFamilyName && gs1.MyMailAdress == gs2.MyMailAdress &&
                gs1.MyStatus == gs2.MyStatus && gs1.MyRegistrationDate == gs2.MyRegistrationDate && gs1.MyEntryDate == gs2.MyEntryDate &&
                gs1.MyReleaseDate == gs2.MyReleaseDate && gs1.MyArea == gs2.MyArea && gs1.MySubArea == gs2.MySubArea &&
                gs1.MyType == gs2.MyType && gs1.MyAdults == gs2.MyAdults && gs1.MyChildren == gs2.MyChildren && gs1.MyPool == gs2.MyPool &&
                gs1.MyJacuzzi == gs2.MyJacuzzi && gs1.MyGarden == gs2.MyGarden && gs1.MyChildrensAttractions == gs2.MyChildrensAttractions)
                return true;
            return false;
        }

        /// <summary>
        /// to string method
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string result = "";
            result = "Request Number: " + MyGuestRequestKey + "\nPrivate Name: " + MyPrivateName + "\nFamily Name: " + MyFamilyName +
                     "\nMail Adress: " + MyMailAdress + "\nRequest's Status: " + MyStatus + "\nRegistresion Date: " + 
                     MyRegistrationDate.ToString("dd/MM/yyyy") + "\nEntry Date: " + MyEntryDate.ToString("dd/MM/yyyy") + 
                     "\nRelease Date: " + MyReleaseDate.ToString("dd/MM/yyyy") + "\nRequest Area: " + MyArea + "\nRequest Sub Area: " + MySubArea + 
                     "\nRequest Unit's Type: " + MyType + "\nAdults Number: " + MyAdults + "\nChildren Number: " + MyChildren + 
                     "\nPool's Interest: " + MyPool + "\nJacuzzi's Interest: " + MyJacuzzi + "\nGarden's Interest: " + MyGarden + 
                     "\nAttraction's Interest: " + MyChildrensAttractions;
            return result;
        }





    }
}
