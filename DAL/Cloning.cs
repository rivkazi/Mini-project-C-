using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public static class Cloning
    {
        /// <summary>
        /// coping between the layers for guest request
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static GuestRequest Clone(this GuestRequest original)
        {
            GuestRequest target = new GuestRequest();
            target.MyGuestRequestKey = original.MyGuestRequestKey;
            target.MyPrivateName = original.MyPrivateName;
            target.MyFamilyName = original.MyFamilyName;
            target.MyMailAdress = original.MyMailAdress;
            target.MyStatus = original.MyStatus;
            target.MyRegistrationDate = original.MyRegistrationDate;
            target.MyEntryDate = original.MyEntryDate;
            target.MyReleaseDate = original.MyReleaseDate;
            target.MyArea = original.MyArea;
            target.MySubArea = original.MySubArea;
            target.MyType = original.MyType;
            target.MyAdults = original.MyAdults;
            target.MyChildren = original.MyChildren;
            target.MyPool = original.MyPool;
            target.MyJacuzzi = original.MyJacuzzi;
            target.MyGarden = original.MyGarden;

            return target;
        }

        /// <summary>
        /// coping between the layers for bank branch
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static BankBranch Clone(this BankBranch original)
        {
            BankBranch target = new BankBranch();
            target.MyBankNumber = original.MyBankNumber;
            target.MyBankName = original.MyBankName;
            target.MyBranchNumber = original.MyBranchNumber;
            target.MyBranchAddress = original.MyBranchAddress;
            target.MyBranchCity = original.MyBranchCity;

            return target;
        }

        /// <summary>
        /// coping between the layers for hosting unit
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static HostingUnit Clone(this HostingUnit original)
        {
            HostingUnit target = new HostingUnit();
            target.MyHostingUnitKey = original.MyHostingUnitKey;
            target.MyOwner = original.MyOwner;
            target.MyHostingUnitName = original.MyHostingUnitName;
            target.MyDiary = original.MyDiary;
            target.MyArea = original.MyArea;

            return target;
        }

        /// <summary>
        /// coping between the layers for host
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static Host Clone(this Host original)
        {
            Host target = new Host();
            target.MyHostKey = original.MyHostKey;
            target.MyPrivateName = original.MyPrivateName;
            target.MyFamilyName = original.MyFamilyName;
            target.MyFhoneNumber = original.MyFhoneNumber;
            target.MyMailAddress = original.MyMailAddress;
            target.MyBankBranchDetails = original.MyBankBranchDetails;
            target.MyBankAccountNumber = original.MyBankAccountNumber;
            target.MyCollectionClearance = original.MyCollectionClearance;

            return target;
        }

        /// <summary>
        /// coping between the layers for order
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static Order Clone(this Order original)
        {
            Order target = new Order();
            target.MyHostingUnitKey = original.MyHostingUnitKey;
            target.MyGuestRequestKey = original.MyGuestRequestKey;
            target.MyHostingUnitKey = original.MyHostingUnitKey;
            target.MyOrderKey = original.MyOrderKey;
            target.MyStatus = original.MyStatus;
            target.MyCreateDate = original.MyCreateDate;
            target.MyCommissionSum = original.MyCommissionSum;

            return target;
        }
    }
}
