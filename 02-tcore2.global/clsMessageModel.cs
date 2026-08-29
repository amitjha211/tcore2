using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcore2.global
{

    public class clsMessageModel
    {
        public bool success = false;
        public string Message  { get; set; }
        public string MessageType { get; set; }
        public object Data { get; set; }    
        public string Field { get; set; }

    }

}
