using Microsoft.Data.SqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcore2.global
{
    public partial class clsGlobal
    {
        public static T JSON_DeserializeObject<T>(string sJSON)
        {
            object objRet = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(sJSON);
            return (T)objRet;
        }

        public static string JSON_SerializeObject(object obj)
        {
            string sRet = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return sRet;
        }

    }
}
