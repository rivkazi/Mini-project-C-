using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;
using DAL;
using System.Threading;

namespace BL
{
    public class BL_imp: IBL
    {
        public static bool flag = true;
        private static IBL instance = null;
        DAL.Idal d = DAL.FactoryDal.GetDAL();
        internal BL_imp()
        {
            Thread t1 = new Thread(Expired);
            t1.Start();
        }

        /// <summary>
        /// returns an IBL object
        /// </summary>
        /// <returns></returns>
        public static IBL getMyBL() //singelton method
        {
            if (instance == null)
                instance = new BL_imp(); //create new object
            return instance; //doesnt create new object if its already created
        }

        public void Expired()
        {
            while(flag)
            {
                d.OrdersExpired();
                Thread.Sleep(86400000);//sleep for a whole day
            }
        }

        #region Guest Request methods
        /// <summary>
        /// Get a Guest Request 
        /// </summary>
        /// <param name="_GuestRequest"></param>
        /// <returns></returns>
        public GuestRequest GetGuestRequest(long _GuestRequestKey)
        {
            return d.GetGuestRequest(_GuestRequestKey);
        }

        /// <summary>
        /// add a Guest Request to the DataBase
        /// </summary>
        /// <param name="_guestRequest"></param>
        public void AddGuestRequest(GuestRequest _guestRequest)
        {
            int days = (_guestRequest.MyReleaseDate - _guestRequest.MyEntryDate).Days;
            if (days < 1)
                throw new UnPossibleSelection(_guestRequest, "unpossible selection");
            d.AddGuestRequest(_guestRequest.Clone()); //sending it to the lower level for adding the Guest Request
        }

        /// <summary>
        /// Update relevant properties of The Guest Reuest
        /// </summary>
        /// <param name="_guestRequest"></param>
        public void SetGuestRequest(GuestRequest _guestRequest, RequestStatus status)
        {
            try
            {
                d.SetGuestRequest(_guestRequest.Clone(), status);
            }
            catch (Exception ex)
            {
                throw new Exception(ex + "");
            }
        }
        #endregion

        #region Hosting Unit methods
        /// Get a Hosting Unit
        /// </summary>
        /// <param name="_HostingUnitKey"></param>
        /// <returns></returns>
        public HostingUnit GetHostingUnit(long _HostingUnitKey)
        {
            return d.GetHostingUnit(_HostingUnitKey);
        }

        /// <summary>
        /// add a Hosting Unit to the DataBase
        /// </summary>
        /// <param name="_hostingUnit"></param>
        public void AddHostingUnit(HostingUnit _hostingUnit)
        {
            d.AddHostingUnit(_hostingUnit.Clone());
        }

        /// <summary>
        /// delete a Hosting Unit from the DataBase
        /// </summary>
        /// <param name="_HostingUnitKey"></param>
        public void DeleteHostingUnit(HostingUnit _HostingUnit)
        {
            if (OpenGuestRequestRelated(_HostingUnit.MyHostingUnitKey) == true || OpenOrdersRelated(_HostingUnit.MyHostingUnitKey) == true)
                throw new UnPossibleSelection(_HostingUnit, "unpossible deletion cause there are request releated");
            d.DeleteHostingUnit(_HostingUnit);
        }

        /// <summary>
        /// returns rather there are open guest request releted to hosing unit or not
        /// </summary>
        /// <param name="HostingUnitKey"></param>
        /// <returns></returns>
        public bool OpenGuestRequestRelated(long HostingUnitKey)
        {
            bool flag = false;//there is no open guest request related
            IEnumerable<Order> o1 = from Order order in d.GetAllOrders()//אוסף את כל ההזמנות שמקשרות בקשות ליחידת אירוח זו
                                    where order.MyHostingUnitKey == HostingUnitKey
                                    select order;
            GuestRequest g;
            foreach (var order in o1)
            {
                g = GetGuestRequest(order.MyGuestRequestKey);
                if (g.MyStatus == RequestStatus.Open)
                {
                    flag = true;
                    break;
                }
            }
            if (flag == true)
                return true;
            return false;
        }


        /// <summary>
        /// returns rather there are done orders releted to hosing unit or not
        /// </summary>
        /// <param name="HostingUnitKey"></param>
        /// <returns></returns>
        public bool OpenOrdersRelated(long HostingUnitKey)
        {
            bool flag = false;//there is no open guest request related
            IEnumerable<Order> o1 = from Order order in d.GetAllOrders()//אוסף את כל ההזמנות שמקשרות בקשות ליחידת אירוח זו
                                    where order.MyHostingUnitKey == HostingUnitKey
                                    select order;
            foreach (var order in o1)
            {
                if (order.MyStatus  == OrderStatus.Close_By_Awnser)
                {
                    flag = true;
                    break;
                }
            }
            if (flag == true)
                return true;
            return false;
        }

        /// <summary>
        /// Update relevant properties of The Hosting unit
        /// </summary>
        /// <param name="_hostingUnit"></param>
        public void SetHostingUnit(HostingUnit _hostingUnit)
        {
            List<HostingUnit> h1 = d.GetAllHostingUnits().ToList();
            HostingUnit h = h1.Find(x => _hostingUnit.MyHostingUnitKey == x.MyHostingUnitKey);
            if (_hostingUnit.MyOwner.MyCollectionClearance == false)
            {
                if (h.MyOwner.MyCollectionClearance == true && OpenGuestRequestRelated(_hostingUnit.MyHostingUnitKey))
                    throw new thereIsAnOpenRequest(_hostingUnit, "there is open guest request related");
            }
            d.SetHostingUnit(_hostingUnit);
        }
        #endregion

        #region Order methods
        /// Get a Order
        /// </summary>
        /// <param name="_HostingUnitKey"></param>
        /// <returns></returns>
        public Order GetOrder(long _orderKey)
        {
            return d.GetOrder(_orderKey);
        }

        /// <summary>
        /// add an order to the DataBase
        /// </summary>
        /// <param name="_order"></param>
        public void AddOrder(Order _order)
        {
            HostingUnit h = GetHostingUnit(_order.MyHostingUnitKey);
            Host ho = h.MyOwner;
            GuestRequest g = GetGuestRequest(_order.MyGuestRequestKey);
            bool approve = ApproveRequest(g.MyEntryDate, g.MyReleaseDate, h.MyDiary);//יוכל ליצור הזמנה רק אם התאריכים ביחידה פנויים
            if (approve == false)
                throw new UnPossibleSelection(_order, "unpossible selection cause those dates already taken");
            else
            {
                if (ho.MyCollectionClearance == true)//רק אם חתם על הרשאה לגבייה בחשבון
                {
                    d.AddOrder(_order);
                    //Console.WriteLine("mail with order details:" + _order.ToString());//כאשר סטטוס ההזמנה משתנה ל"נשלח למייל" אוטומטית המארחת שולחת מייל עם פרטי ההזמנה
                }
            }
        }

        /// <summary>
        /// Update relevant properties of Order
        /// </summary>
        /// <param name="_order"></param>
        public void SetOrder(Order _order, OrderStatus status)
        {
            IEnumerable<Order> o1 = from Order o in d.GetAllOrders()
                                    where o.isSame(o, _order) == true
                                    select o;
            if (o1.ToList().Count() != 1)
                throw new ItemDoesntExists(_order, "this order doesnt found ");

            if (_order.MyStatus == OrderStatus.Close_By_Awnser)
                throw new OrderAlreadyDone(_order, "this order already done");//אי אפשר לשנות את הסטטוס יותר לאחר שהעסקה נסגרה    

            GuestRequest g = GetGuestRequest(_order.MyGuestRequestKey);
            HostingUnit h = GetHostingUnit(_order.MyHostingUnitKey);

            if (status == OrderStatus.Close_By_Awnser)//כאשר סטטוס ההזמנה משתנה עקב סגירת עסקה
            {
                int comissionPayment = ((g.MyReleaseDate - g.MyEntryDate).Days) * Configuration.ComissionHeight;//יש לבצע חישוב עמלה בגובה של 10 שח ליום
                _order.MyCommissionSum = comissionPayment;
                AssignRequest(g.MyEntryDate, g.MyReleaseDate, h.MyDiary);//יש לסמן במטריצה את התאריכים המתאימים
                d.SetHostingUnit(h);
                d.SetOrder(_order, OrderStatus.Close_By_Awnser);
                RequestStatus s = RequestStatus.Close_By_Web;
                d.SetGuestRequest(g, s);//יש לשנות את סטטוס דרישת הלקוח בהתאם לסגירת העסקה

                IEnumerable<Order> o2 = from Order order in d.GetAllOrders() //אוסף כל ההזמנות הקשורות בדרישה זו
                                        where _order.MyGuestRequestKey == order.MyGuestRequestKey
                                        select order;
                foreach (var order in o2)
                {
                    if (order.MyStatus != OrderStatus.Close_By_Awnser)//שינוי הסטטוס של כל ההזמנות האחרות עקב סגירת עסקה
                        order.MyStatus = OrderStatus.Close_By_MissAwnser;//בקשה זו כבר נסגרה עם מארח אחר
                }
            } 
            d.SetOrder(_order.Clone(), status);
        }
        #endregion

        #region getting all the lists from all kinds
        /// <summary>
        /// Get all Hosting units
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<HostingUnit> GetAllHostingUnits()
        {
            return d.GetAllHostingUnits();
        }

        /// <summary>
        /// Get all Hosting Guest Request
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<GuestRequest> GetAllGuestRequests()
        {
            return d.GetAllGuestRequests();
        }

        /// <summary>
        /// Get all orders
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<Order> GetAllOrders()
        {
            return d.GetAllOrders();
        }

        /// <summary>
        /// Get all Bank Branches
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        //public List<BankBranch> GetAllBankBranch()
        //{
        //    return d.GetAllBankBranch();
        //}
        #endregion

        /// <summary>
        /// this function returns all hosting units available in specific dates
        /// </summary>
        /// <param name="entryDate"></param>
        /// <param name="numOfDays"></param>
        /// <returns></returns>
        public List<HostingUnit> available(DateTime entryDate, int numOfDays)
        {
            DateTime releaseDate = entryDate.AddDays(numOfDays);
            IEnumerable<HostingUnit> units = from HostingUnit hu in DataSource.listHostingUnits
                                             where ApproveRequest(entryDate, releaseDate, hu.MyDiary) == true
                                             select hu;
            List<HostingUnit> hostingUnits = new List<HostingUnit>();
            foreach(var hu in units)//makeing a list
                hostingUnits.Add(hu);
            return hostingUnits;
        }

        /// <summary>
        /// this function returns the difference of amount of days betweem two dates
        /// </summary>
        /// <param name="DateTimeA"></param>
        /// <param name="DateTimeB"></param>
        /// <returns></returns>
        public int daysAmount(DateTime DateTimeA, DateTime DateTimeB)
        {
            return (DateTimeB - DateTimeA).Days;
        }

        /// <summary>
        /// this function returns a list of orderes that their num of days past from their creation
        /// bigger or even to a given number
        /// </summary>
        /// <param name="numDays"></param>
        /// <returns></returns>
        public List<Order> daysPast(int numDays)
        {
            DateTime current = DateTime.Now;
            List<Order> ordersList = new List<Order>();
            IEnumerable<Order> orders = from Order o in d.GetAllOrders()
                                        where daysAmount(o.MyCreateDate, current) >= numDays
                                        select o;
            foreach (var o1 in orders)//makeing a list
                ordersList.Add(o1);
            return ordersList;
        }
        
        /// <summary>
        /// returns all guest request fits a condition
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<GuestRequest> filterBy(Func<GuestRequest, bool> filter)
        {
            var guestRequests = d.GetAllGuestRequests();
            return guestRequests.Where(gs => filter(gs)).ToList();
        }

        /// <summary>
        /// this function returns the num of orders sent to a specific guest request
        /// </summary>
        /// <param name="guestRequest"></param>
        /// <returns></returns>
        public int numOfOrdersSent(GuestRequest guestRequest)
        {
            int count = 0;
            IEnumerable<Order> orders = from Order o in d.GetAllOrders()
                                        where o.MyGuestRequestKey == guestRequest.MyGuestRequestKey
                                        select o;
            foreach (var order in orders)
                count++;
            return count;
        }

        /// <summary>
        /// this function returns the num of orderes successfuly done with a specific hosting unit
        /// </summary>
        /// <param name="hostingUnit"></param>
        /// <returns></returns>
        public int numOfOrdersDone(HostingUnit hostingUnit)
        {
            int count = 0;
            IEnumerable<Order> orders = from Order o in d.GetAllOrders()
                                        where GetHostingUnit(o.MyHostingUnitKey) == hostingUnit
                                        select o;
            foreach (var order in orders)
            {
                if (order.MyStatus == OrderStatus.Close_By_Awnser)
                    count++;
            }
            return count;
        }

        /// <summary>
        /// returns a list of keys of hosting units
        /// </summary>
        /// <returns></returns>
        public List<int> getAllHostingUnitKeys()
        {
            List<HostingUnit> hu = d.GetAllHostingUnits().ToList();
            var v = from item in hu
                    select new { key = item.MyHostingUnitKey };
            return (List<int>)v;
        }

        #region grouping
        /// <summary>
        /// grouping the guests requests by the areas
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IGrouping<Area, GuestRequest>> GuestRequestGroupsByAreas()
        {
            List<GuestRequest> list = d.GetAllGuestRequests();
            var v = from item in list
                    group item by item.MyArea into g
                    select g;
            return v;
        }

        /// <summary>
        /// grouping the guests requests by the num of people
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IGrouping<int,GuestRequest>> GuestRequesGroupstByNumOfPeople()
        {
            var v = from item in d.GetAllGuestRequests()
                    let num = item.MyChildren + item.MyAdults
                    group item by num into g
                    select g;

            return v;
        }

        /// <summary>
        /// this function returns the num of hosting units of a host
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        public int numOfHostingUnits(int OwnerKey)
        {
            List<HostingUnit> hu = d.GetAllHostingUnits();
            var v = from item in hu
                    where item.MyOwner.MyHostKey == OwnerKey
                    select item.Clone();
            int x = v.Count();
            return x;
        }

        private bool hostExists(Host h)
        {
            foreach(Host item in hst)
            {
                if (item == h)
                    return true;
            }
            return false;
        }
        List<Host> hst = new List<Host>();
        /// <summary>
        /// grouping the hosts by the num of hosting unit
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public IEnumerable<IGrouping<int, Host>> HostsGroupsByNumOfHostingUnits()
        {
            List<HostingUnit> h = d.GetAllHostingUnits();
            foreach (HostingUnit item in h)//create list of host
            {
                if (hostExists(item.MyOwner) != true)
                {
                    hst.Add(item.MyOwner);
                }
            }
            var v = from item in hst
                    let id = item.MyHostKey
                    let numOfUnits = numOfHostingUnits(id)
                    group item by numOfUnits into g
                    select g;
            return v;
        }

        /// <summary>
        /// grouping the hosting units by their areas
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IGrouping<Area, HostingUnit>> HostingUnitGroupsByAreas()
        {
            var v = from item in d.GetAllHostingUnits()
                    group item by item.MyArea into g
                    select g;
            return v;
        }
        #endregion

        /// <summary>
        /// returns the hosting units order by the num of orderes done with them
        /// </summary>
        /// <returns></returns>
        public List<HostingUnit> OrderByNumOfDoneOrders()
        {
            List<HostingUnit> hu = new List<HostingUnit>();
            var v = from HostingUnit in d.GetAllHostingUnits()
                    orderby(numOfOrdersDone(HostingUnit))
                    select HostingUnit;
            v.ToList();
            foreach(var item in v)
            {
                hu.Add(item);
            }
            return hu;
        }

        /// <summary>
        /// returns the sum of done orders related to host in all of his units
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        public int SumOfDoneOrders(Host h)
        {
            int sum = 0;
            var hostingUnits = from item in d.GetAllHostingUnits()
                    where item.MyOwner == h
                    select item;
            foreach(var v in hostingUnits)
            {
                sum += numOfOrdersDone(v);
            }

            return sum;
        }

        /// <summary>
        /// grouping the orders by their creation dates
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IGrouping<DateTime, Order>> OrdersGroupsByCreationDate()
        {
            IEnumerable<Order> gr = d.GetAllOrders();
            var result = from item in gr
                         group item by item.MyCreateDate into g
                         select g;

            return result;
        }

        /// <summary>
        /// grouping the guests requests by their status
        /// </summary>
        /// <returns></returns>
        public List<GuestRequest> GuestRequestsGroupsByStatus()
        {
            IEnumerable<GuestRequest> gr = d.GetAllGuestRequests();
            var result = from item in gr
                         group item by item.MyStatus into g
                         select g;

            return (List<GuestRequest>)result;
        }

        /// <summary>
        /// this function returns rather the request days are available in the hosting unit or not
        /// </summary>
        /// <param name="EntryDate"></param>
        /// <param name="ReleaseDate"></param>
        /// <param name="diary"></param>
        /// <returns></returns>
        public bool ApproveRequest(DateTime EntryDate, DateTime ReleaseDate, bool[,] diary)
        {
            int dayS, monthS;
            dayS = EntryDate.Day-1;
            monthS = EntryDate.Month-1;

            int dayF, monthF;
            dayF = ReleaseDate.Day-1;
            monthF = ReleaseDate.Month-1;

            int sumDays = 0;

            int numDays = 0;
            bool help = true;
            if (monthS == monthF)
            {
                numDays = dayF - dayS;
                if (diary[dayS, monthS] == false)
                {
                    for (int i = 0; i < numDays && help == true; i++)
                    {
                        if (diary[dayS + i, monthS] == true)
                            help = false;
                    }
                    if (help == false)
                    {
                        return false;
                    }
                }
                if (diary[dayS, monthS] == true)//the first day is taken
                    return false;
            }
            else//not the same month
            {
                if (diary[dayS, monthS] == false)
                {
                    if (monthS == 2-1)
                        sumDays = 28;
                    if (monthS == 1-1 || monthS == 3-1 || monthS == 5-1 || monthS == 7-1 || monthS == 8-1 || monthS == 10-1 || monthS == 12-1)
                        sumDays = 31;
                    else if (monthS == 1 || monthS == 3 || monthS == 5 || monthS == 8 || monthS == 10 || monthS == 12)
                        sumDays = 30;
                    for (int i = 0; i < sumDays - dayS-1 && help == true; i++)//for the first month
                    {
                        if (diary[dayS + i, monthS] == true)
                            help = false;
                    }
                    if (help == false)
                    {
                        return false;
                    }
                    else//for the second month
                    {
                        for (int i = 0; i <= dayF && help == true; i++)
                        {
                            if (diary[dayS + i, monthS + 1] == true)
                                help = false;
                        }
                        if (help == false)
                        {
                            return false;
                        }
                    }
                }
                else// the first day is taken
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// this function mark the requested days in the matrix of the hosting unit
        /// </summary>
        /// <param name="EntryDate"></param>
        /// <param name="ReleaseDate"></param>
        /// <param name="diary"></param>
        public void AssignRequest(DateTime EntryDate, DateTime ReleaseDate, bool[,] diary)
        {
            int dayS, monthS;
            dayS = EntryDate.Day - 1;
            monthS = EntryDate.Month - 1;

            int dayF, monthF;
            dayF = ReleaseDate.Day - 1;
            monthF = ReleaseDate.Month - 1;
            int help1 = 0, help2 = 0;
            int numOfDays = (ReleaseDate - EntryDate).Days;

            int sumDays = 0;
            if (monthS == 2 - 1)
                sumDays = 28;
            if (monthS == 1 - 1 || monthS == 3 - 1 || monthS == 5 - 1 || monthS == 7 - 1 || monthS == 8 - 1 || monthS == 10 - 1 || monthS == 12 - 1)
                sumDays = 31;
            else
                sumDays = 30;

            if (dayS + numOfDays < sumDays)
                for (int j = 0; j < numOfDays; j++)
                {
                    diary[EntryDate.Day + j, EntryDate.Month] = true;
                }
            else//not in the same month
            {
                help1 = sumDays - dayS;//the wanted days in the first month
                help2 = (dayF + numOfDays) - sumDays;//the wanted days in the second month
                for (int j = 0; j < help1; j++)
                {
                    diary[EntryDate.Day + j, EntryDate.Month] = true;
                }
                for (int j = 0; j < help2; j++)
                {
                    diary[EntryDate.Day + j, EntryDate.Month + 1] = true;
                }
            }
        } 
    }
}
