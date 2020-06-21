using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class itemAlreadyExists : Exception
    {
        public object myOb { get; private set; }
        public itemAlreadyExists(object ob, string st) : base(st)
        {
            myOb = ob;
        }

        override public string ToString()
        {
            return "the " + myOb.GetType().ToString() + "already exists";
        }
    }
}
