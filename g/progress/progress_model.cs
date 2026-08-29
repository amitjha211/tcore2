using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace g.progress
{
    public class progress_model
    {
        public string currentStatus = "";
        public DataTable tProgress = null;
        public string progressName = "";
        public string progressTitle = "";
        public decimal per = 0;
        
        public Action<object, string> onAction;
    }

}
