using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BE
{
    public class HostingUnit
    {
        private long HostingUnitKey;
        private Host Owner;
        private string HostingUnitName;
        private bool[,] Diary;
        private Area Area;

        public long MyHostingUnitKey{ get => HostingUnitKey; set => HostingUnitKey = value; }
        public Host MyOwner { get => Owner; set => Owner = value; }
        public string MyHostingUnitName { get => HostingUnitName; set => HostingUnitName = value; }
        [XmlIgnore]
        public bool[,] MyDiary { get => Diary; set => Diary = value; }//[days=31, months=12]
        public Area MyArea { get => Area; set => Area = value; }

        [XmlArray("HUDiary")]
        public bool[] HUDiaryto
        {
            get { return Diary.Flatten(); }
            set { Diary = value.Expand(31); } //5 is the number of roes in the matrix
        }

        public bool isSame(HostingUnit hu1, HostingUnit hu2)
        {
            if (hu1.MyOwner == hu2.MyOwner && hu1.MyHostingUnitName == hu2.MyHostingUnitName
                && hu1.MyDiary == hu2.MyDiary && hu1.MyArea == hu2.MyArea)
                return true;
            return false;
        }

        public override string ToString()
        {
            string result = "";
            result = "Hosting Unit's Number: " + MyHostingUnitKey + "\nHosting Unit's owner: " + MyOwner.MyPrivateName +" " 
                   + MyOwner.MyFamilyName + "\nHosting Unit's Name: " + MyHostingUnitName + "\nHosting Unit's Area: " + MyArea;
            return result;
        }
    }
}
 