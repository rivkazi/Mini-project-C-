using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using BE;
using DS;

namespace DAL
{
    class Dal_XML_imp : Idal
    {
        XElement ConfigurationRoot;
        XElement GuestRequestRoot;
        XElement OrderRoot;

        string ConfigurationPath = @"ConfigurationXml.xml";
        string GuestRequestPath = @"GuestRequestXml.xml";
        string HostingUnitPath = @"HostingUnitXml.xml";
        string OrderPath = @"OrderXml.xml";

        private static Dal_XML_imp instance = null;

        public static Dal_XML_imp getDal_XML()//singelton
        {
            if (instance == null)
                instance = new Dal_XML_imp();
            return instance;
        }
        


        private Dal_XML_imp()
        {
            if (!File.Exists(ConfigurationPath))
            {
                ConfigurationRoot = new XElement("Configuration");
                ConfigurationRoot.Add(new XElement("KeyGuestRequest", 10000000));
                ConfigurationRoot.Add(new XElement("KeyHostingUnit", 10000000));
                ConfigurationRoot.Add(new XElement("KeyOrder", 10000000));
                ConfigurationRoot.Add(new XElement("ManagerPassword", 1111));
                ConfigurationRoot.Add(new XElement("HostPassword", 1234));
                ConfigurationRoot.Add(new XElement("Fee", 10));
                ConfigurationRoot.Add(new XElement("DaysForExpire", 30));

                ConfigurationRoot.Save(ConfigurationPath);
            }
            else
                ConfigurationRoot = XElement.Load(ConfigurationPath);

            if (!File.Exists(GuestRequestPath))
                CreateFileGuestRequest();
            else
                LoadDataGuestRequest();

            if (!File.Exists(OrderPath))
                CreateFileOrder();
            else
                LoadDataOrder();

            if (!File.Exists(HostingUnitPath))
            {
                FileStream fileHostingUnit = new FileStream(HostingUnitPath, FileMode.Create);
                fileHostingUnit.Close();
            }
            else
                DataSource.listHostingUnits = LoadFromXML<HostingUnit>(HostingUnitPath);
            SaveToXML(DataSource.listHostingUnits, HostingUnitPath);
        }
        

        #region ConfigurationXML
        private void LoadDataConfiguration()
        {
            try
            {
                ConfigurationRoot = XElement.Load(ConfigurationPath);
            }
            catch
            {
                throw new Exception("File upload problem");
            }
        }
        #endregion

        #region GuestRequestXML
        private void CreateFileGuestRequest()
        {
            GuestRequestRoot = new XElement("GuestRequests");
            GuestRequestRoot.Save(GuestRequestPath);
        }

        private void LoadDataGuestRequest()
        {
            try
            {
                GuestRequestRoot = XElement.Load(GuestRequestPath);
            }
            catch
            {
                throw new Exception("File upload problem");
            }
        }

        public void SaveGuestRequestListToXML(List<GuestRequest> gr)
        {
            //GuestRequestRoot = new XElement("GuestRequest");
            //XElement GuestRequests = new XElement("GuestRequests");
            foreach (GuestRequest item in gr)
            { 
                XElement Key = new XElement("Key", item.MyGuestRequestKey);
                XElement PrivateName = new XElement("PrivateName", item.MyPrivateName);
                XElement FamilyName = new XElement("FamilyName", item.MyFamilyName);
                XElement MailAdress = new XElement("MailAdress", item.MyMailAdress);
                XElement Status = new XElement("Status", item.MyStatus.ToString());
                XElement RegistrationDate = new XElement("RegistrationDate", item.MyRegistrationDate);
                XElement EntryDate = new XElement("EntryDate", item.MyEntryDate);
                XElement ReleaseDate = new XElement("ReleaseDate", item.MyReleaseDate);
                XElement Area = new XElement("Area", item.MyArea.ToString());
                XElement SubArea = new XElement("SubArea", item.MySubArea.ToString());
                XElement Type = new XElement("Type", item.MyType.ToString());
                XElement Adults = new XElement("Adults", item.MyAdults);
                XElement Children = new XElement("Children", item.MyChildren);
                XElement Pool = new XElement("Pool", item.MyPool.ToString());
                XElement Jacuzzi = new XElement("Jacuzzi", item.MyJacuzzi.ToString());
                XElement Garden = new XElement("Garden", item.MyGarden.ToString());
                XElement ChildrensAttractions = new XElement("ChildrensAttractions", item.MyChildrensAttractions.ToString());
                XElement GuestRequest = new XElement("GuestRequest", Key, PrivateName, FamilyName, MailAdress, Status, RegistrationDate, EntryDate,
                                                     ReleaseDate, Area, SubArea, Type, Adults, Children, Pool, Jacuzzi, Garden, ChildrensAttractions);

                GuestRequestRoot.Add(GuestRequest);
            }
            GuestRequestRoot.Save(GuestRequestPath);
        }

        public List<GuestRequest> GetGuestRequestListFromXMl()
        {
            LoadDataGuestRequest();
            List<GuestRequest> gr;
            try
            {
                gr = (from item in GuestRequestRoot.Elements()
                            select new GuestRequest()
                            {
                                MyGuestRequestKey = Convert.ToInt32(item.Element("Key").Value),
                                MyPrivateName = item.Element("PrivateName").Value,
                                MyFamilyName = item.Element("FamilyName").Value,
                                MyMailAdress = item.Element("MailAdress").Value,
                                MyStatus = (RequestStatus)Enum.Parse(typeof(RequestStatus),item.Element("Status").Value),
                                MyRegistrationDate = DateTime.Parse(item.Element("RegistrationDate").Value),
                                MyEntryDate = DateTime.Parse(item.Element("EntryDate").Value),
                                MyReleaseDate = DateTime.Parse(item.Element("ReleaseDate").Value),
                                MyArea = (Area)Enum.Parse(typeof(Area), item.Element("Area").Value),
                                MySubArea = item.Element("SubArea").Value,
                                MyType= (Type)Enum.Parse(typeof(Type), item.Element("Type").Value),
                                MyAdults = Convert.ToInt32(item.Element("Adults").Value),
                                MyChildren = Convert.ToInt32(item.Element("Children").Value),
                                MyPool = (Interest)Enum.Parse(typeof(Interest), item.Element("Pool").Value),
                                MyJacuzzi = (Interest)Enum.Parse(typeof(Interest), item.Element("Jacuzzi").Value),
                                MyGarden = (Interest)Enum.Parse(typeof(Interest), item.Element("Garden").Value),
                                MyChildrensAttractions = (Interest)Enum.Parse(typeof(Interest), item.Element("ChildrensAttractions").Value),
                            }).ToList();
            }
            catch
            {
                gr = null;
            }
            return gr;
        }
        #endregion

        #region HostingUnitXML
        public static void SaveToXML<T>(List<T> list, string path)
        {
            FileStream file = new FileStream(path, FileMode.Create);
            XmlSerializer xmlSer = new XmlSerializer(list.GetType());//source.GetType());
            xmlSer.Serialize(file, list);
            file.Close();
        }

        public static List<T> LoadFromXML<T>(string path)
        {
            List<T> list = new List<T>();
            FileStream file = new FileStream(path, FileMode.Open);
            XmlSerializer xmlSer = new XmlSerializer(typeof(List<T>));
            if (file.Length != 0)
                list = (List<T>)xmlSer.Deserialize(file);
            file.Close();
            return list;
        }
        #endregion

        #region OrderXML
        private void CreateFileOrder()
        {
            OrderRoot = new XElement("Orders");
            OrderRoot.Save(OrderPath);
        }

        private void LoadDataOrder()
        {
            try
            {
                OrderRoot = XElement.Load(OrderPath);
            }
            catch
            {
                throw new Exception("File upload problem");
            }
        }

        public void SaveOrderListToXML(List<Order> orders)
        {
            foreach (Order item in orders)
            {
                XElement Key = new XElement("Key", item.MyOrderKey);
                XElement GuestRequestKey = new XElement("GuestRequestKey", item.MyGuestRequestKey);
                XElement HostingUnitKey = new XElement("HostingUnitKey", item.MyHostingUnitKey);
                XElement Status = new XElement("Status", item.MyStatus.ToString());
                XElement CreateDate = new XElement("CreateDate", item.MyCreateDate);
                XElement CommissionSum = new XElement("CommissionSum", item.MyCommissionSum);
                XElement Order = new XElement("Order", Key, GuestRequestKey, HostingUnitKey, Status, CreateDate , CommissionSum);

                OrderRoot.Add(Order);
            }
            OrderRoot.Save(OrderPath);
        }

        public List<Order> GetOrderListFromXMl()
        {
            LoadDataOrder();
            List<Order> or;
            try
            {
                or = (from item in OrderRoot.Elements()
                      select new Order()
                      {
                          MyOrderKey = Convert.ToInt32(item.Element("Key").Value),
                          MyGuestRequestKey = Convert.ToInt32(item.Element("GuestRequestKey").Value),
                          MyHostingUnitKey = Convert.ToInt32(item.Element("HostingUnitKey").Value),
                          MyStatus = (OrderStatus)Enum.Parse(typeof(OrderStatus), item.Element("Status").Value),
                          MyCreateDate = DateTime.Parse(item.Element("CreateDate").Value),
                          MyCommissionSum = Convert.ToInt32(item.Element("CommissionSum").Value),
                      }).ToList();
            }
            catch
            {
                or = null;
            }
            return or;
        }
        #endregion

        //methods
        #region Guest Request methods
        /// <summary>
        /// Get a Guest Request 
        /// </summary>
        /// <param name="_GuestRequest"></param>
        /// <returns></returns>
        public GuestRequest GetGuestRequest(long _GuestRequestKey)
        {
            LoadDataGuestRequest();
            GuestRequest gr;
            try
            {
                gr = (from item in GuestRequestRoot.Elements()
                      where Convert.ToInt32(item.Element("Key").Value) == _GuestRequestKey
                      select new GuestRequest()
                      {
                          MyGuestRequestKey = Convert.ToInt32(item.Element("Key").Value),
                          MyPrivateName = item.Element("PrivateName").Value,
                          MyFamilyName = item.Element("FamilyName").Value,
                          MyMailAdress = item.Element("MailAdress").Value,
                          MyStatus = (RequestStatus)Enum.Parse(typeof(RequestStatus), item.Element("Status").Value),
                          MyRegistrationDate = DateTime.Parse(item.Element("RegistrationDate").Value),
                          MyEntryDate = DateTime.Parse(item.Element("EntryDate").Value),
                          MyReleaseDate = DateTime.Parse(item.Element("ReleaseDate").Value),
                          MyArea = (Area)Enum.Parse(typeof(Area), item.Element("Area").Value),
                          MySubArea = item.Element("SubArea").Value,
                          MyType = (Type)Enum.Parse(typeof(Type), item.Element("Type").Value),
                          MyAdults = Convert.ToInt32(item.Element("Adults").Value),
                          MyChildren = Convert.ToInt32(item.Element("Children").Value),
                          MyPool = (Interest)Enum.Parse(typeof(Interest), item.Element("Pool").Value),
                          MyJacuzzi = (Interest)Enum.Parse(typeof(Interest), item.Element("Jacuzzi").Value),
                          MyGarden = (Interest)Enum.Parse(typeof(Interest), item.Element("Garden").Value),
                          MyChildrensAttractions = (Interest)Enum.Parse(typeof(Interest), item.Element("ChildrensAttractions").Value),
                      }).FirstOrDefault();
            }
            catch
            {
                gr = null;
            }
            return gr;
        }

        /// <summary>
        /// add a Guest Request to the DataBase
        /// </summary>
        /// <param name="_guestRequest"></param>
        public void AddGuestRequest(GuestRequest _guestRequest)
        {
            XElement GuestRequestElement = (from item in GuestRequestRoot.Elements()
                                            where Convert.ToInt32(item.Element("Key").Value) == _guestRequest.MyGuestRequestKey
                                            select item).FirstOrDefault();
            if (GuestRequestElement != null)
                throw new itemAlreadyExists(_guestRequest, "this guest request already exists");
            long key = Convert.ToInt64(ConfigurationRoot.Element("KeyGuestRequest").Value);
            ++key;
            ConfigurationRoot.Element("KeyGuestRequest").Value = key.ToString();
            ConfigurationRoot.Save(ConfigurationPath);
            _guestRequest.MyStatus = RequestStatus.Open;
            XElement Key = new XElement("Key", key);
            XElement PrivateName = new XElement("PrivateName", _guestRequest.MyPrivateName);
            XElement FamilyName = new XElement("FamilyName", _guestRequest.MyFamilyName);
            XElement MailAdress = new XElement("MailAdress", _guestRequest.MyMailAdress);
            XElement Status = new XElement("Status", _guestRequest.MyStatus.ToString());
            XElement RegistrationDate = new XElement("RegistrationDate", _guestRequest.MyRegistrationDate);
            XElement EntryDate = new XElement("EntryDate", _guestRequest.MyEntryDate);
            XElement ReleaseDate = new XElement("ReleaseDate", _guestRequest.MyReleaseDate);
            XElement Area = new XElement("Area", _guestRequest.MyArea.ToString());
            XElement SubArea = new XElement("SubArea", _guestRequest.MySubArea.ToString());
            XElement Type = new XElement("Type", _guestRequest.MyType.ToString());
            XElement Adults = new XElement("Adults", _guestRequest.MyAdults);
            XElement Children = new XElement("Children", _guestRequest.MyChildren);
            XElement Pool = new XElement("Pool", _guestRequest.MyPool.ToString());
            XElement Jacuzzi = new XElement("Jacuzzi", _guestRequest.MyJacuzzi.ToString());
            XElement Garden = new XElement("Garden", _guestRequest.MyGarden.ToString());
            XElement ChildrensAttractions = new XElement("ChildrensAttractions", _guestRequest.MyChildrensAttractions.ToString());
            GuestRequestRoot.Add(new XElement("GuestRequest", Key, PrivateName, FamilyName, MailAdress, Status, RegistrationDate, EntryDate,
                                                     ReleaseDate, Area, SubArea, Type, Adults, Children, Pool, Jacuzzi, Garden, ChildrensAttractions));
            GuestRequestRoot.Save(GuestRequestPath);
        }

        /// <summary>
        /// Update relevant properties of The Guest Reuest
        /// </summary>
        /// <param name="_guestRequest"></param>
        public void SetGuestRequest(GuestRequest _guestRequest, RequestStatus _status)
        {
            XElement GuestRequestElement = (from item in GuestRequestRoot.Elements()
                                            where Convert.ToInt32(item.Element("Key").Value) == _guestRequest.MyGuestRequestKey
                                            select item).FirstOrDefault();
            if (GuestRequestElement == null)
                throw new ItemDoesntExists(_guestRequest, "this guest request doesnt exists");
            GuestRequestElement.Element("Status").Value = _status.ToString();
            GuestRequestRoot.Save(GuestRequestPath);
        }
        #endregion

        #region Hosting unit methods
        /// <summary>
        /// Get a Hosting Unit
        /// </summary>
        /// <param name="HostingUnitKey"></param>
        /// <returns></returns>
        public HostingUnit GetHostingUnit(long _HostingUnitKey)
        {
            string path = @"HostingUnitXml.xml";
            List<HostingUnit> hu = LoadFromXML<HostingUnit>(path);
            HostingUnit h = hu.FirstOrDefault(item => item.MyHostingUnitKey == _HostingUnitKey);
            if (h == null)
                throw new ItemDoesntExists(_HostingUnitKey, "this hosting unit doesnt exists");
            else
                return h;
        }

        /// <summary>
        /// add a Hosting Unit to the DataBase
        /// </summary>
        /// <param name="_hostingUnit"></param>
        public void AddHostingUnit(HostingUnit _hostingUnit)
        {
            string path = @"HostingUnitXml.xml";
            List<HostingUnit> hu = LoadFromXML<HostingUnit>(path);
            IEnumerable<HostingUnit> h1 = from item in hu
                                          where item.isSame(item, _hostingUnit)
                                          select item;
            if (h1.ToList().Count() != 0)
                throw new itemAlreadyExists(_hostingUnit, "this hosting unit already exists");
            else
            {
                long key = Convert.ToInt64(ConfigurationRoot.Element("KeyHostingUnit").Value);
                ++key;
                ConfigurationRoot.Element("KeyHostingUnit").Value = key.ToString();
                ConfigurationRoot.Save(ConfigurationPath);
                _hostingUnit.MyHostingUnitKey = key;
                hu.Add(_hostingUnit);
                SaveToXML(hu, path);
            }
        }

        /// <summary>
        /// delete a Hosting Unit from the DataBase
        /// </summary>
        /// <param name="_HostingUnitKey"></param>
        public void DeleteHostingUnit(HostingUnit _HostingUnit)
        {
            string path = @"HostingUnitXml.xml";
            List<HostingUnit> hu = LoadFromXML<HostingUnit>(path);
            HostingUnit h = GetHostingUnit(_HostingUnit.MyHostingUnitKey);
            if (h == null)
                throw new ItemDoesntExists(_HostingUnit, "this hosting unit doesnt exists");
            else
            {
                foreach (HostingUnit item in hu)
                {
                    if (item.MyHostingUnitKey == h.MyHostingUnitKey)
                    {
                        hu.Remove(item);
                        SaveToXML(hu, path);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Update relevant properties of The Hosting unit
        /// </summary>
        /// <param name="_hostingUnit"></param>
        public void SetHostingUnit(HostingUnit _hostingUnit)
        {
            string path = @"HostingUnitXml.xml";
            List<HostingUnit> hu = LoadFromXML<HostingUnit>(path);
            HostingUnit h = GetHostingUnit(_hostingUnit.MyHostingUnitKey);
            if (h == null)
                throw new ItemDoesntExists(_hostingUnit, "this hosting unit doesnt exists");
            else
            {
                int indexOfHostingUnit = hu.FindIndex(item => item.MyHostingUnitKey == _hostingUnit.MyHostingUnitKey);
                hu[indexOfHostingUnit] = _hostingUnit;
                SaveToXML(hu, path);
            }
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
            LoadDataOrder();
            Order or;
            try
            {
                or = (from item in OrderRoot.Elements()
                      where Convert.ToInt32(item.Element("Key").Value) == _OrdertKey
                      select new Order()
                      {
                          MyOrderKey = Convert.ToInt32(item.Element("Key").Value),
                          MyGuestRequestKey = Convert.ToInt32(item.Element("GuestRequestKey").Value),
                          MyHostingUnitKey = Convert.ToInt32(item.Element("HostingUnitKey").Value),
                          MyStatus = (OrderStatus)Enum.Parse(typeof(OrderStatus), item.Element("Status").Value),
                          MyCreateDate = DateTime.Parse(item.Element("CreateDate").Value),
                          MyCommissionSum = Convert.ToInt32(item.Element("CommissionSum").Value),
                      }).FirstOrDefault();
            }
            catch
            {
                or = null;
            }
            return or;
        }

        /// <summary>
        /// add an order to the DataBase
        /// </summary>
        /// <param name="_order"></param>
        public void AddOrder(Order _order)
        {
            XElement OrderElement = (from item in OrderRoot.Elements()
                                            where Convert.ToInt64(item.Element("Key").Value) == _order.MyOrderKey
                                            select item).FirstOrDefault();
            if (OrderElement != null)
                throw new itemAlreadyExists(_order, "this Order already exists");
            long key = Convert.ToInt64(ConfigurationRoot.Element("KeyOrder").Value);
            ++key;
            ConfigurationRoot.Element("KeyOrder").Value = key.ToString();
            ConfigurationRoot.Save(ConfigurationPath);
            _order.MyStatus = OrderStatus.Email_Sent;
            XElement Key = new XElement("Key", key);
            XElement GuestRequestKey = new XElement("GuestRequestKey", _order.MyGuestRequestKey);
            XElement HostingUnitKey = new XElement("HostingUnitKey", _order.MyHostingUnitKey);
            XElement Status = new XElement("Status", _order.MyStatus.ToString());
            XElement CreateDate = new XElement("CreateDate", _order.MyCreateDate);
            XElement CommissionSum = new XElement("CommissionSum", _order.MyCommissionSum);
            XElement Order = new XElement("GuestRequest", Key, GuestRequestKey, HostingUnitKey, Status, CreateDate, CommissionSum);
            OrderRoot.Add(Order);
            OrderRoot.Save(OrderPath);
        }

        /// <summary>
        /// Update relevant properties of Order
        /// </summary>
        /// <param name="_order"></param>
        public void SetOrder(Order _order, OrderStatus _status)
        {
            XElement OrderElement = (from item in OrderRoot.Elements()
                                     where Convert.ToInt64(item.Element("Key").Value) == _order.MyOrderKey
                                     select item).FirstOrDefault();
            if (OrderElement == null)
                throw new ItemDoesntExists(_order, "this order doesnt exists");
            OrderElement.Element("Status").Value = _status.ToString();
            OrderElement.Element("CommissionSum").Value = _order.MyCommissionSum.ToString();

            OrderRoot.Save(OrderPath);
        }

        /// <summary>
        /// this function change the status for expired orders
        /// </summary>
        public void OrdersExpired()
        {
            List<Order> orders = GetAllOrders();
            if (orders.Count() == 0)
                return;
            foreach (Order item in orders)
            {
                if ((DateTime.Today - item.MyCreateDate).Days > Configuration.NumDayForExpires)
                {
                    SetOrder(item, OrderStatus.Close_By_MissAwnser);
                }
            }
        }
        #endregion

        #region getting all the lists from all kinds
        /// <summary>
        /// Get all Hosting units
        /// </summary>
        /// <returns></returns>
        public List<HostingUnit> GetAllHostingUnits()
        {
            string path = @"HostingUnitXml.xml";
            List<HostingUnit> list = LoadFromXML<HostingUnit>(path);
            return list;
        }

        /// <summary>
        /// Get all Guest Request
        /// </summary>
        /// <returns></returns>
        public List<GuestRequest> GetAllGuestRequests()
        {
            List<GuestRequest> gr = GetGuestRequestListFromXMl();
            return gr;
        }

        /// <summary>
        /// Get all orders
        /// </summary>
        /// <returns></returns>
        public List<Order> GetAllOrders()
        {
            List<Order> or = GetOrderListFromXMl();
            return or;
        }
        #endregion
    }
}
