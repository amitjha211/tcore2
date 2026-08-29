using apk;
using apk.db;
using apk.sql;
using apk.table;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using tcore2;

namespace apk
{


    public static partial class clsAPK_DBF
    {
        public static clsConnectionModel db_getCon(clsAPK _apk
            , string sCon = "con_main")
        {
            clsConnectionModel objCon = clsAPKF.getCon(_apk, sCon);
            if (objCon == null)
                throw new Exception($"Connection not found [{sCon}]");
            return objCon;
        }

        private static Exception_Core getSQLException(string sMessage, string sSQL)
        {
            StringBuilder sbError = new StringBuilder();
            sbError.AppendLine("SQL Error : ");
            sbError.AppendLine(sMessage);
            sbError.AppendLine($"SQL : {sSQL}");
            var ex = new Exception_Core(sbError.ToString());
            return ex;
        }
        public static clsTableModel db_create_table(clsAPK _apk
            , string sTable
            , string sCon = "con_main")
        {

            clsConnectionModel objCon = clsAPKF.getCon(_apk, sCon);
            clsTableModel objTable = new clsTableModel();

            DataTable tTableInfo = q_getData(_apk, $"select * from sys_objects where app = '{_apk.appName}' and db_object_name = '{sTable}' ", "con_schema");

            if (tTableInfo.Rows.Count>0)
            {
                DataRow r = tTableInfo.Rows[0];

                objTable.TableName = r["db_object_name"].ToString();
                objTable.IDName = r["pk_field"].ToString();
                objTable.ViewName = r["table_view_name"].ToString();
            }
            else
            {
                objTable.TableName = sTable;
                objTable.ViewName = sTable;
                objTable.IDName = "id";
            }

            clsTableF.init(objTable, objCon);

            return objTable;

        }

        //[DebuggerStepThrough]
        public static DataTable q_getData(clsAPK _apk, clsSQLModel objSQL, string sCon = "con_main")
        {

            clsCommand cmd = clsSQLF.getCommandSelect(objSQL);

            try
            {
                DataTable t = q_getData(_apk, cmd, sCon);
                return t;
            }
            catch (Exception ex)
            {
                throw getSQLException(ex.Message, cmd.sql);
            }


            return null; ;
        }



        //[DebuggerStepThrough]
        public static DataTable q_getData(clsAPK _apk, clsCommand cmd, string sCon = "con_main")
        {


            clsConnectionModel objCon = null;

            try
            {
                objCon = clsAPKF.getCon(_apk, sCon);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            try
            {

                DataTable t = clsDB.getData(cmd, objCon);
                return t;
            }
            catch (Exception ex)
            {
                throw getSQLException(ex.Message, cmd.sql);
            }

            return null; ;
        }


        [DebuggerStepThrough]
        public static DataTable q_getData(clsAPK _apk, string q, string sCon = "con_main")
        {
            clsConnectionModel objCon = null;

            try
            {
                objCon = clsAPKF.getCon(_apk, sCon);
            }
            catch (Exception_Core ex)
            {
                throw ex;
            }


            try
            {
                DataTable t = clsDB.getData(q, objCon);
                return t;
            }
            catch (Exception_Core ex)
            {
                throw getSQLException(ex.Message, q);
            }

            return null;
        }
    }
}
