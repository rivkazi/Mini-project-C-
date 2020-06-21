using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class ItemDoesntExists: Exception
    {
        public object myOb { get; private set; }
        public ItemDoesntExists(object ob, string st) : base(st)
        {
            myOb = ob;
        }

        override public string ToString()
        {
            return "the " + myOb.GetType().ToString() + "doesnt exist";
        }

    }
}
