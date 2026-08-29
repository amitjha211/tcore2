using apk.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace g
{
    public class exception_sql : exception_g
    {
        public clsCommand cmd;
        public clsConnectionModel objCon = null;
        public exception_sql(string sMsg) : base(sMsg)
        {
        }
    }

}
