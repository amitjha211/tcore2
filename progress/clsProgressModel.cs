using CsvHelper.Configuration.Attributes;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace tcore2.progress
{

    public class clsProgressModel
    {
        public string progressName { get; set; }
        public string progressLabel { get; set; }

        public string Msg { get; set; }

        public string action { get; set; }

        public int iCurrentRow = 0;
        public int iCount = 0;
        public decimal iPer = 0;
        public object Tag;
    }

}
