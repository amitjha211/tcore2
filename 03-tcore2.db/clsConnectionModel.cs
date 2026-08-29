using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apk.db
{
    public class clsConnectionModel
    {
        public string DBType { get; set; }
        public string ConnectionString { get; set; }
        internal iDBCon objCon { get; set; }
        public DataTable tFields { get; set; }
        public DataTable tTables { get; set; }

    }
}
