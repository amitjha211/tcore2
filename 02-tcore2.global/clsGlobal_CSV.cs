using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using static g.csv;

namespace tcore2.global
{

    
    public partial class clsGlobal
    {
        
        [DebuggerStepThrough]
        public static DataTable readCSVFromURL(string sUrl)  => csv_read_url(sUrl);


        [DebuggerStepThrough]
        public static DataTable readCSVFromFile(string sFile) => csv_read_file(sFile);
        
        
    }
}
