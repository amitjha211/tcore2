using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcore2;

namespace apk.db
{

    internal interface iDBCon
    {
        DataTable getData(string sql, iCommand cmd);
        int exec(string q, iCommand cmd);
        object execScalar(string q, iCommand cmd);
    }



}
