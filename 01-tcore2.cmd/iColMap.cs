using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcore2
{
    public interface iColMap
    {
        public int iSource { get; set; }
        public int iDest { get; set; }
        public string Source { get; set; }
        public string Dest { get; set; }
        public object defaultValue { get; set; }
    }
}
