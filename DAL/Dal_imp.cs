using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DS;
using BE;

namespace DAL
{
    public class Dal_imp :Idal
    {
        private static Idal instance = null;

        /// <summary>
        /// returns an Idal object
        /// </summary>
        /// <returns></returns>
        public static Idal getMyDal() //singelton method
        {
            if (instance == null)
                instance = new Dal_imp(); //create new object
            return instance; //doesnt create new object if its already created
        }

        #region Guest Request methods
        /// <summary>
        /// Get a Guest Request 
        /// </summary>
        /// <param name="_GuestRequest"></param>
        /// <returns></returns>
        public GuestRequest GetGuestRequest(long _GuestRequestKey)
        {
            GuestRequest guestRequest = DataSource.listGuestRequests.FirstOrDefault(gr => gr.MyGuestRequestKey == _GuestRequestKey);
            if (guestRequest == null)
                throw new ItemDoesntExists(_GuestRequestKey, "this guest request doesnt exists");
            return guestRequest;
        }

        /// <summary>
        /// Add a Guest Request to the list
        /// </summary>
        /// <param name="_guestRequest"></param>
        public void AddGuestRequest(GuestRequest _guestRequest)
        {
            List<GuestRequest> gr = DataSource.listGuestRequests;
            if (gr.Count() == 0)
            {
                ++BE.Configuration.GuestRequestKey;
                _guestRequest.MyGuestRequestKey = BE.Configuration.GuestRequestKey;
                _guestRequest.MyStatus = RequestStatus.Open;
                DataSource.listGuestRequests.Add(_guestRequest.Clone());
            }
            else
            {
                IEnumerable<GuestRequest> g1 = from item in gr
                                               where item.isSame(item, _guestRequest) == true
                                               select item;

                if (g1.ToList().Count() != 0)
                    throw new itemAlreadyExists(_guestRequest, "this guest request already exists");
                _guestRequest.MyGuestRequestKey = (++BE.Configuration.GuestRequestKey);
                DataSource.listGuestRequests.Add(_guestRequest.Clone());
            }
        }

        /// <summary>
        ///  Update relevant properties of The Guest Reuest
        /// </summary>
        /// <param name="_guestRequest"></param>
        public void SetGuestRequest(GuestRequest _guestRequest, RequestStatus status)
        {
            IEnumerable<GuestRequest> g1 = from GuestRequest gr in DataSource.listGuestRequests
                                           where gr.MyGuestRequestKey == _guestRequest.MyGuestRequestKey
                                           select gr;
            if (g1.ToList().Count() == 0)
                throw new ItemDoesntExists(_guestRequest, "this guest request doesnt exists");
            int indexOfGuest = DataSource.listGuestRequests.FindIndex(gs => gs.MyGuestRequestKey == _guestRequest.MyGuestRequestKey);
            _guestRequest.MyStatus = status;
            DataSource.listGuestRequests[indexOfGuest] = _guestRequest.Clone();
        }
        #endregion

        #region Hosting Unit methods
        /// <summary>
        /// Get a Hosting Unit
        /// </summary>
        /// <param name="HostingUnitKey"></param>
        /// <returns></returns>
        public HostingUnit GetHostingUnit(long _HostingUnitKey)
        {
            HostingUnit hostingUnit= DataSource.listHostingUnits.FirstOrDefault(hu => hu.MyHostingUnitKey == _HostingUnitKey);
            if (hostingUnit == null)
                throw new ItemDoesntExists(_HostingUnitKey, "this hosting unit doesnt exists");
            return hostingUnit;
        }

        /// <summary>
        /// Add a hosting unit to the list
        /// </summary>
        /// <param name="_hostingUnit"></param>
        public void AddHostingUnit(HostingUnit _hostingUnit)
        {
            List<HostingUnit> hu = DataSource.listHostingUnits;
            if (hu.Count() == 0)
            {
                _hostingUnit.MyHostingUnitKey = (++BE.Configuration.HostingUnitKey);
                DataSource.listHostingUnits.Add(_hostingUnit.Clone());
            }
            else
            {
                IEnumerable<HostingUnit> h1 = from hosting in hu
                                              where hosting.isSame(hosting, _hostingUnit)
                                              select hosting;
                if (h1.ToList().Count() != 0)
                    throw new itemAlreadyExists(_hostingUnit, "this hosting unit already exists");
                _hostingUnit.MyHostingUnitKey = (++BE.Configuration.HostingUnitKey);
                DataSource.listHostingUnits.Add(_hostingUnit.Clone());
            }   
        }

        /// <summary>
        /// Delete a Hosting Unit
        /// </summary>
        /// <param name="_HostingUnitKey"></param>
        public void DeleteHostingUnit(HostingUnit _HostingUnit)
        {
            bool flag = false;
            foreach(var v in DataSource.listHostingUnits)
            {
                if(v.MyHostingUnitKey == _HostingUnit.MyHostingUnitKey)
                {
                    DataSource.listHostingUnits.Remove(v);
                    flag = true;
                    break;
                }
            }
            if (!flag)
                throw new ItemDoesntExists(_HostingUnit, "this hosting unit doesnt exists");
        }

        /// <summary>
        /// Update relevant properties of The hosting unit
        /// </summary>
        /// <param name="_hostingUnit"></param>
        public void SetHostingUnit(HostingUnit _hostingUnit)
        {
            HostingUnit hosting = GetHostingUnit(_hostingUnit.MyHostingUnitKey);
            if(hosting == null)
                throw new ItemDoesntExists(_hostingUnit, "this hosting unit doesnt exists");
            int indexOfHostingUnit = DataSource.listHostingUnits.FindIndex(hu => hu.isSame(hu, hosting));
            DataSource.listHostingUnits[indexOfHostingUnit] = _hostingUnit;
        }
        #endregion

        #region Order methods
        /// <summary>
        /// Get a Order 
        /// </summary>
        /// <param name="_GuestRequest"></param>
        /// <returns></returns>
        public Order GetOrder(long _OrdertKey)
        {
            Order order = DataSource.listOrders.FirstOrDefault(gr => gr.MyOrderKey == _OrdertKey);
            if (order == null)
                throw new ItemDoesntExists(_OrdertKey, "this order doesnt exists");
            return order;
        }

        /// <summary>
        /// Add Order to the  list
        /// </summary>
        /// <param name="_order"></param>
        public void AddOrder(Order _order)
        {
            List<Order> or = DataSource.listOrders;
            if (or.Count() == 0)
            {
                _order.MyOrderKey = (++BE.Configuration.OrderKey);
                DataSource.listOrders.Add(_order.Clone());
            }
            else
            {
                IEnumerable<Order> o1 = from o in or
                                        where o.isSame(o, _order) == true
                                        select o;
                if (o1.ToList().Count() != 0)
                    throw new itemAlreadyExists(_order, "this order already exists");
                _order.MyOrderKey = (++BE.Configuration.OrderKey);
                _order.MyStatus = OrderStatus.Email_Sent;
                DataSource.listOrders.Add(_order);
            }
        }

        /// <summary>
        /// Update relevant properties of The order
        /// </summary>
        /// <param name="_order"></param>
        public void SetOrder(Order _order, OrderStatus status)
        {
            Order o = GetOrder(_order.MyOrderKey);
            if (o == null)
                throw new ItemDoesntExists(_order, "this order doesnt found ");
            int indexOfOrder = DataSource.listOrders.FindIndex(order => order.isSame(order, o));
            _order.MyStatus = status;
            DataSource.listOrders[indexOfOrder] = _order;
        }

        public void OrdersExpired() { }
        #endregion

        #region getting all the lists from all kinds
        /// <summary>
        ///  Get all Hosting Units
        /// </summary>
        /// <returns></returns>
        public List<HostingUnit> GetAllHostingUnits()
        {
            var v = from item in DataSource.listHostingUnits
                    select item.Clone();

            return v.ToList();
        }

        /// <summary>
        ///  Get all Guest Requests
        /// </summary>
        /// <returns></returns>
        public List<GuestRequest> GetAllGuestRequests()
        {
            List<GuestRequest> g = DataSource.listGuestRequests;
            var v = from item in g
                    select item.Clone();

            return v.ToList();
        }

        /// <summary>
        ///  Get all Orders
        /// </summary>
        /// <returns></returns>
        public List<Order> GetAllOrders()
        {
            var v = from item in DataSource.listOrders
                    select item.Clone();

            return v.ToList();

        }

        /// <summary>
        ///  Get all Bank Branches
        /// </summary>
        /// <returns></returns>
        //public List<BankBranch> GetAllBankBranch()
        //{
        //    List<BankBranch> branches = new List<BankBranch>();
        //    branches.Add(
        //        new BankBranch() { MyBankNumber = 123456, MyBankName = "Yahav", MyBranchNumber = 319, MyBranchAddress = "Rotchild 15", MyBranchCity = "Tel-Aviv" });
        //    branches.Add(
        //         new BankBranch() { MyBankNumber = 132456, MyBankName = "Leumi", MyBranchNumber = 342, MyBranchAddress = "Ezra 3", MyBranchCity = "Jerusalem" });
        //    branches.Add(
        //        new BankBranch() { MyBankNumber = 123546, MyBankName = "Diskont", MyBranchNumber = 103, MyBranchAddress = "Hagibor 22", MyBranchCity = "Lod" });
        //    branches.Add(
        //        new BankBranch() { MyBankNumber = 123465, MyBankName = "Poalim", MyBranchNumber = 556, MyBranchAddress = "Pinkas 7", MyBranchCity = "Rehovot" });
        //    branches.Add(
        //        new BankBranch() { MyBankNumber = 213456, MyBankName = "Mizrachi", MyBranchNumber = 281, MyBranchAddress = "Shimon 2", MyBranchCity = "Ramla" });
        //    return branches;
        //}
        #endregion
    }
}


