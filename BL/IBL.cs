using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace BL
{
    public interface IBL
    {
        #region Guest Request methods
        /// <summary>
        /// Get a Guest Request 
        /// </summary>
        /// <param name="_GuestRequest"></param>
        /// <returns></returns>
        GuestRequest GetGuestRequest(long _GuestRequestKey);

        /// <summary>
        /// add a Guest Request to the DataBase
        /// </summary>
        /// <param name="_guestRequest"></param>
        void AddGuestRequest(GuestRequest _guestRequest);

        /// <summary>
        /// Update relevant properties of The Guest Reuest
        /// </summary>
        /// <param name="_guestRequest"></param>
        void SetGuestRequest(GuestRequest _guestRequest, RequestStatus status);
        #endregion

        #region Hosting Unit methods
        /// Get a Hosting Unit
        /// </summary>
        /// <param name="_HostingUnitKey"></param>
        /// <returns></returns>
        HostingUnit GetHostingUnit(long _HostingUnitKey);

        /// <summary>
        /// add a Hosting Unit to the DataBase
        /// </summary>
        /// <param name="_hostingUnit"></param>
        void AddHostingUnit(HostingUnit _hostingUnit);

        /// <summary>
        /// delete a Hosting Unit from the DataBase
        /// </summary>
        /// <param name="_HostingUnitKey"></param>
        void DeleteHostingUnit(HostingUnit _HostingUnit);

        /// <summary>
        /// Update relevant properties of The Hosting unit
        /// </summary>
        /// <param name="_hostingUnit"></param>
        void SetHostingUnit(HostingUnit _hostingUnit);
        #endregion

        #region Order methods
        /// Get a Order
        /// </summary>
        /// <param name="_HostingUnitKey"></param>
        /// <returns></returns>
        Order GetOrder(long _orderKey);
        
        /// <summary>
        /// add an order to the DataBase
        /// </summary>
        /// <param name="_order"></param>
        void AddOrder(Order _order);

        /// <summary>
        /// Update relevant properties of Order
        /// </summary>
        /// <param name="_order"></param>
        void SetOrder(Order _order, OrderStatus status);
        #endregion

        #region getting all the lists for all kinds
        /// <summary>
        /// Get all Hosting units
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<HostingUnit> GetAllHostingUnits();

        /// <summary>
        /// Get all Hosting Guest Request
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<GuestRequest> GetAllGuestRequests();

        /// <summary>
        /// Get all orders
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<Order> GetAllOrders();

        ///// <summary>
        ///// Get all Bank Branches
        ///// </summary>
        ///// <param name="predicate"></param>
        ///// <returns></returns>
        //List<BankBranch> GetAllBankBranch();
        #endregion

        /// <summary>
        /// this function returns all hosting units available in specific dates
        /// </summary>
        /// <param name="entryDate"></param>
        /// <param name="numOfDays"></param>
        /// <returns></returns>
        List<HostingUnit> available(DateTime entryDate, int numOfDays);

        /// <summary>
        /// this function returns the difference of amount of days betweem two dates
        /// </summary>
        /// <param name="DateTimeA"></param>
        /// <param name="DateTimeB"></param>
        /// <returns></returns>
        int daysAmount(DateTime DateTimeA, DateTime DateTimeB);

        /// <summary>
        /// this function returns a list of orderes that their num of days past from their creation
        /// bigger or even to a given number
        /// </summary>
        /// <param name="numDays"></param>
        /// <returns></returns>
        List<Order> daysPast(int numDays);

        /// <summary>
        /// returns all guest request fits a condition
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        List<GuestRequest> filterBy(Func<GuestRequest, bool> filter);

        /// <summary>
        /// this function returns the num of orders sent to a specific guest request
        /// </summary>
        /// <param name="guestRequest"></param>
        /// <returns></returns>
        int numOfOrdersSent(GuestRequest guestRequest);

        /// <summary>
        /// this function returns the num of orderes successfuly done with a specific hosting unit
        /// </summary>
        /// <param name="hostingUnit"></param>
        /// <returns></returns>
        int numOfOrdersDone(HostingUnit hostingUnit);

        #region grouping
        /// <summary>
        /// grouping the guests requests by the areas
        /// </summary>
        /// <param name="_area"></param>
        /// <returns></returns>
        IEnumerable<IGrouping<Area, GuestRequest>> GuestRequestGroupsByAreas();

        /// <summary>
        /// grouping the guests requests by the num of people
        /// </summary>
        /// <returns></returns>
        IEnumerable<IGrouping<int, GuestRequest>> GuestRequesGroupstByNumOfPeople();

        /// <summary>
        /// grouping the host by the num of hosting unit
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        IEnumerable<IGrouping<int, Host>> HostsGroupsByNumOfHostingUnits();

        /// <summary>
        /// grouping the hosting units by their areas
        /// </summary>
        /// <returns></returns>
        IEnumerable<IGrouping<Area, HostingUnit>> HostingUnitGroupsByAreas();
        #endregion
    }
}
