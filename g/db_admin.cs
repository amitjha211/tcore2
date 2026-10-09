using apk;
using apk.db;
using g.progress;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static apk.admin.apk_admin;
using static tcore2.global.clsGlobal;
using static g.db;
using static g.progress.progress_f;


namespace g
{
    public class db_admin
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

        public static void db_backup_xml(clsConnectionModel _con,string sPath,progress_model _p)
        {
            
            string q = "select * from information_schema.tables where TABLE_TYPE = 'BASE TABLE'";
            DataTable tTableList = get_data(_con, q);
            DataSet ds = new DataSet();

            foreach (DataRow rTableInfo in tTableList.Rows)
            {

                string sTableName = rTableInfo.getString("TABLE_NAME");
                progress_start(_p, sTableName,sTableName);
                DataTable t = get_data(_con, $"select * from {sTableName}");
                t.TableName = sTableName;
                ds.Tables.Add(t);
                progress_end(_p);
            }

            progress_start(_p, "saving xml", "saving xml");
            ds.WriteXml(sPath, XmlWriteMode.WriteSchema);
            progress_end(_p);
        }

    }
}
