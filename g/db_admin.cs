using apk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static apk.admin.apk_admin;

namespace tcore2.g
{
    internal class db_admin
    {

        private static string getTmpPath(string sFileName)
        {
            return "d:/tmp/apk_tmp/" + sFileName;
        }
        public void copyDB(string sAppName
            , string sEnv
            , string sDBSource
            , string sDBDest)
        {
            clsAPK _apkSource = apk_create("TJ", "trade_tmp", "dev");
            string sDSPath = getTmpPath("ds.xml");
        }

    }
}
