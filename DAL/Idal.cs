using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;


namespace DAL
{
    public interface Idal
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

        #region Hosting unit methods
        /// <summary>
        /// Get a Hosting Unit
        /// </summary>
        /// <param name="HostingUnitKey"></param>
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
        /// <summary>
        /// Get a Order 
        /// </summary>
        /// <param name="_GuestRequest"></param>
        /// <returns></returns>
        Order GetOrder(long _OrdertKey);

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

        /// <summary>
        /// this function change the status for expired orders
        /// </summary>
        void OrdersExpired();
        #endregion

        #region getting all the lists from all kinds
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
    }
}
