using apk;
using apk.db;
using apk.exception;
using apk.sql;
using apk.table;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using tcore2;
using tcore2.global;
using static apk.clsAPKF_FS_F;

namespace apk
{
    public static partial class clsAPK_DBF
    {


        public static DataTable db_get_data(string sCon
            , clsAPK_Module _m
            , string sqlName
            , params object[] args)
        {
            string sSQL = null;

            try
            {
                sSQL = f_getStr(_m, sqlName, args);
                DataTable t = q_getData(_m._apk, sSQL, sCon);
                return t;
            }
            catch (Exception_Core ex)
            {
                throw ex;
            }

            return null;
        }


        //private static Exception getSQLException(string sMessage, string sSQL)
        //{
        //    StringBuilder sbError = new StringBuilder();
        //    sbError.AppendLine("SQL Error : ");
        //    sbError.AppendLine(sMessage);
        //    sbError.AppendLine($"SQL : {sSQL}");
        //    var ex = new Exception(sbError.ToString());

        //    return ex;
        //}

        

        [DebuggerStepThrough]
        public static DataTable db_get_data(clsAPK_Module _m, string sqlName, params object[] args)
        {
            string sSQL = null;

            try
            {
                sSQL = f_getStr(_m, sqlName, args);
            }
            catch (Exception ex)
            {
                throw new Exception_APK(ex.Message);
            }

            DataTable t = null;

            try
            {
                t = q_getData(_m._apk, sSQL);
            }
            catch (Exception ex)
            {
                throw new Exception_APK(ex.Message);
            }

               return t;
        }

        public static clsSQLModel db_getsql(clsAPK_Module _m, string sName, params object[] args)
        {

            string q = null;
            try
            {
                q = f_getStr(_m, sName, args);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return clsAPK_ObjectF.createSQLModel(_m._apk, q);
        }


        private static List<string> sql_list_by_go(string s)
        {

            List<string> sqlList = new List<string>();
            StringBuilder sbCurrent = new StringBuilder();

            using (TextReader sr = new StringReader(s))
            {

                string sLine = "";
                while ((sLine = sr.ReadLine()) != null)
                {
                    if (sLine.Trim().ToLower() == "go")
                    {
                        sqlList.Add(sbCurrent.ToString());
                        sbCurrent.Clear();
                    }
                    else
                    {
                        sbCurrent.AppendLine(sLine);
                    }
                }
            }

            return sqlList;
        }

        public static void db_executeFile(clsAPK _apk
            , string sFile
            , string sCon = "con_main")
        {

            string _sql_scripts = f_getStr(_apk, sFile);
            List<string> _lst_script = sql_list_by_go(_sql_scripts);

            clsConnectionModel objCon = clsAPKF.getCon(_apk, sCon);

            foreach (string q in _lst_script)
            {
                clsCommand cmd = new clsCommand();
                cmd.sql = q;
                db_exec(_apk, cmd, objCon);
            }
        }


        public static int db_exec(clsAPK _apk
                , clsCommand cmd
                , string sCon = "con_main")
        {
            clsConnectionModel objCon = clsAPKF.getCon(_apk, sCon);
            return db_exec(_apk, cmd, objCon);
        }


        [DebuggerStepThrough]
        public static int db_exec(clsAPK _apk
                , string q
                , string sCon = "con_main")
        {
            clsConnectionModel objCon = clsAPKF.getCon(_apk, sCon);
            clsCommand cmd = new clsCommand();
            cmd.sql = q;
            try
            {
                return db_exec(_apk, cmd, objCon);
            }
            catch (Exception_APK ex)
            {
                throw new Exception_APK(ex.Message);
            }
        }


        public static int db_exec(clsAPK _apk
            , string q
            , clsConnectionModel objCon)
        {
            clsCommand cmd = new clsCommand();
            cmd.sql = q;
            return db_exec(_apk, cmd, objCon);
        }

        [DebuggerStepThrough]
        public static int executeNonQuery(clsCommand cmd, clsConnectionModel con)
        {
            try
            {
                int ret = con.objCon.exec(cmd.sql, cmd);
                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception_APK(ex.Message);
            }
        }


        [DebuggerStepThrough]
        public static int db_exec_rnd(clsAPK _apk
            , clsCommand cmd
            , clsConnectionModel objCon)
        {
             
            try
            {
                return executeNonQuery(cmd, objCon);
            }
            catch (Exception_APK ex)
            {
                throw new Exception_APK(ex.Message);
            }
        }


        [DebuggerStepThrough]
        public static int db_exec(clsAPK _apk
            , clsCommand cmd
            , clsConnectionModel objCon)
        {
            try
            {
                return executeNonQuery(cmd, objCon);
            }
            catch (Exception_APK ex)
            {
                throw new Exception_APK(ex.Message);
            }
        }

        public static clsSQLModel db_getsql(clsAPK _apk
            , string sqlFile
            , params object[] args)
        {
            string sSQL = f_getStr(_apk, sqlFile, args);
            return clsAPK_ObjectF.createSQLModel(_apk, sSQL);
        }

        public static void db_save_data(clsAPK _apk
            , string sTable
            , DataTable t
            , string sCon = "con_main", bool executeTrigger = true)
        {
            foreach (DataRow r in t.Rows)
            {
                db_save_data(_apk, sTable, r, sCon, executeTrigger);
            }
        }


        public static void db_save_data(clsAPK _apk
        , string sTable
        , DataRow r
        , string sCon = "con_main"
        , bool executeTrigger = true)
        {
            clsTableModel objTable = db_create_table(_apk, sTable, sCon);
            db_save_data(_apk, objTable, r, sCon, executeTrigger);
        }

        public static void db_save_data(clsAPK _apk
        , clsTableModel objTable
        , DataRow r
        , string sCon = "con_main"
        , bool executeTrigger = true)
        {


            clsConnectionModel objCon = clsAPKF.getCon(_apk, sCon);


            int iID = r.getInt("id");
            clsTableF.save(objTable, objCon, r);


            if (executeTrigger
                && _apk.onTableDML != null)
            {

                clsTableEvent e = new clsTableEvent();
                e.Table = objTable.TableName;
                e.ID = r.getInt("id");
                e.row = r.getCMD();

                if (iID == 0)
                    e.DMLCode = "INSERT";
                else
                    e.DMLCode = "UPDATE";

                _apk.onTableDML(e);
            }
        }


    }
}
