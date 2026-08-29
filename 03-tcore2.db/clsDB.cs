using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using tcore2;
using static tcore2.global.clsGlobal;
using System.Diagnostics;
using tcore2.global;


namespace apk.db
{
    [DebuggerStepThrough]
    public static class clsDB
    {
        public static clsConnectionModel createConnection(string sConnectionString, string sDBType)
        {   
            clsConnectionModel objCon = new clsConnectionModel();
            objCon.ConnectionString = sConnectionString;
            objCon.DBType = sDBType;
    
            try
            {
                init(objCon);
                return objCon;
            }
            catch (Exception ex)
            {
                throw new Exception_Core(ex.Message);
            }
            
            return null;
        }

        public static void init(clsConnectionModel con)
        {
            switch (con.DBType.ToLower())
            {
                case "mssql":
                    con.objCon = new clsMSSQL(con.ConnectionString);
                    break;
                case "sqlite":
                    con.objCon = new clsSQLite(con.ConnectionString);
                    break;
                case "pgsql":
                    con.objCon = new clsPGSQL(con.ConnectionString);
                    break;
            }
        }


        public static DataTable getData(string q, clsConnectionModel con)
        {
            clsCommand cmd = new clsCommand();
            cmd.sql = q;

            try
            {
                return con.objCon.getData(cmd.sql, cmd);
            }
            catch (Exception ex)
            {
                throw new Exception_Core(ex.Message);
            }
        }

        public static DataTable getData(clsCommand cmd, clsConnectionModel con)
        {

            try
            {
                DataTable t  =  con.objCon.getData(cmd.sql, cmd);
                return t;
            }
            catch (Exception ex)
            {
                throw new Exception_Core(ex.Message);
            }

            return null;
        }

        public static object executeScalar(string q, clsConnectionModel con)
        {
            clsCommand cmd = new clsCommand();
            cmd.sql = q;

            try
            {
                object ret = con.objCon.execScalar(cmd.sql, cmd);
                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception_Core(ex.Message);
            }
            return null;
        }

        public static object executeScalar(clsCommand cmd, clsConnectionModel con)
        {
            try
            {
                object ret = con.objCon.execScalar(cmd.sql, cmd);
                return ret;
            }
            catch(Exception ex)
            {
                throw new Exception_Core(ex.Message);
            }
        }
        public static object executeScalar(string q, iCommand cmd, clsConnectionModel con)
        {
            try
            {
                object ret =  con.objCon.execScalar(q, cmd);
                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception_Core(ex.Message);
            }
        }

        public static int executeNonQuery(string sql, clsConnectionModel con)
        {
            clsCommand cmd = new clsCommand();
            cmd.sql = sql;
            try
            {
                int ret = con.objCon.exec(sql, cmd);
                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception_Core(ex.Message);
            }
        }

        public static int executeNonQuery(clsCommand cmd, clsConnectionModel con)
        {
            try
            {
                int ret = con.objCon.exec(cmd.sql, cmd);
                return ret;
            }
            catch(Exception ex)
            {
                throw new Exception_Core(ex.Message);
            }
        }

        public static int executeNonQuery(string q, iCommand cmd, clsConnectionModel con)
        {
            try
            {
                int ret = con.objCon.exec(q, cmd);
                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception_Core(ex.Message);
            }
            return 0;
        }

    }
}
