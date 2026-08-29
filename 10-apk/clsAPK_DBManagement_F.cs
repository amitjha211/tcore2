using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Xml;
using tcore2.console;
using apk.table;
using tcore2.global;
using static tcore2.global.clsGlobal;
using static apk.clsAPKF_FS_F;
using apk.db;
using System.Xml.Schema;
using tcore2.progress;

namespace apk
{
    public static class clsAPK_DBManagement_F
    {

        public static XmlDocument getDB_XMLDocument(clsAPK _apk)
        {
            string sXml = f_getStr(_apk, "@/db/db.xml");
            XmlDocument _xDoc = new XmlDocument();
            _xDoc.LoadXml(sXml);
            return _xDoc;
        }

        public static bool isDBExists(clsAPK _apk) => true;

    }
}
