using apk;
using apk.db;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcore2;

namespace g
{
    public static class db
    {

        [DebuggerStepThrough]
        public static DataTable get_data(clsConnectionModel con, string q)
        {
            clsCommand cmd = new clsCommand();
            cmd.sql = q;
            return get_data(con, cmd);
        }


        


        [DebuggerStepThrough]
        public static DataTable get_data(clsConnectionModel con, clsCommand cmd)
        {

            try
            {
                return con.objCon.getData(cmd.sql, cmd);
            }
            catch (Exception ex)
            {
                exception_sql _ex = new exception_sql(ex.Message);
                _ex.cmd = cmd;
                _ex.objCon = con;

                throw _ex;
            }
        }



        [DebuggerStepThrough]
        public static int exec(clsConnectionModel con, string q)
        {
            clsCommand cmd = new clsCommand();
            cmd.sql = q;
            return exec(con, cmd);
        }

        [DebuggerStepThrough]
        public static int exec(clsConnectionModel con, clsCommand cmd)
        {
            try
            {
                int ret = con.objCon.exec(cmd.sql, cmd);
                return ret;
            }
            catch (Exception ex)
            {
                exception_sql _ex = new exception_sql(ex.Message);
                _ex.cmd = cmd;
                _ex.objCon = con;
                throw _ex;
            }
        }


        [DebuggerStepThrough]
        public static object exec_scalar(clsConnectionModel con, string q)
        {
            clsCommand cmd = new clsCommand();
            cmd.sql = q;
            return exec(con, cmd);
        }

        [DebuggerStepThrough]
        public static object exec_scalar(clsConnectionModel con, clsCommand cmd)
        {
            try
            {
                object ret = con.objCon.execScalar(cmd.sql, cmd);
                return ret;
            }
            catch (Exception ex)
            {
                exception_sql _ex = new exception_sql(ex.Message);
                _ex.cmd = cmd;
                _ex.objCon = con;
                throw _ex;
            }
        }



        [DebuggerStepThrough]
        public static clsConnectionModel getCon(clsAPK _apk, string sCon = "con_main")
        {
            clsConnectionModel objCon = clsAPKF.getEnvObject(_apk, sCon) as clsConnectionModel;
            if (objCon == null) throw new exception_g($"Connection not {sCon} found ");
            return objCon;
        }


    }
}
