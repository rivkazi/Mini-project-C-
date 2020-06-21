using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class FactotyBL
    {
        /// <summary>
        /// create an IBL object
        /// </summary>
        /// <returns></returns>
        public static IBL GetBL()
        {
            return BL_imp.getMyBL();
        }
    }
}
