using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using tcore2;
using System.Diagnostics;

namespace apk.db
{
    internal class clsMSSQL : iDBCon
    {

        private static SqlCommand getSQLCmd(SqlConnection cnn
            , iCommand cmd, string q)
        {

            SqlCommand sqlcmd = cnn.CreateCommand();
            sqlcmd.CommandText = q;

            for (int i = 0; i < cmd.getCount(); i++)
            {
                string sName = cmd.getKey(i);
                object objValue = cmd.get(i);
                sqlcmd.Parameters.AddWithValue(sName, objValue);
            }

            return sqlcmd;
        }

        private string _connectionString = "";
        public clsMSSQL(string sConnectionString)
        {
            _connectionString = sConnectionString;
        }


        [DebuggerStepThrough]
        public DataTable getData(string q, iCommand cmd)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand sqlcmd = getSQLCmd(conn, cmd, q);
                SqlDataAdapter ad = new SqlDataAdapter(sqlcmd);

                try
                {
                    DataTable t = new DataTable();

                    ad.Fill(t);
                    return t;

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        [DebuggerStepThrough]
        public int exec(string q, iCommand cmd)
        {
            int iReturn = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand sqlcmd = getSQLCmd(conn, cmd, q);
                sqlcmd.Connection = conn;

                try
                {
                    iReturn = sqlcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception_Core( ex.Message);
                    iReturn = -1;
                }
            }

            return iReturn;
        }

        [DebuggerStepThrough]
        public object execScalar(string q, iCommand cmd)
        {
            object ret = DBNull.Value;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand sqlcmd = getSQLCmd(conn, cmd, q);
                sqlcmd.Connection = conn;
                ret = sqlcmd.ExecuteScalar();
            }
            return ret;
        }
    }
}
