using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class FactoryDal
    {
        /// <summary>
        /// create an Idal object
        /// </summary>
        /// <returns></returns>
        public static Idal GetDAL() 
        {
            //return Dal_imp.getMyDal();
            return Dal_XML_imp.getDal_XML();
        }

    }
}
