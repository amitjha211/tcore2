using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apk.dts
{
    public class clsColMap 
    {
        public int iSource { get; set; } = -1;
        public int iDest { get; set; } = -1;
        public string Source { get; set; }
        public string Dest { get; set; }
        public string defaultValue { get; set; }
    }

}
