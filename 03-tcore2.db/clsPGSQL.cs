using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

using Npgsql;
using tcore2;

namespace apk.db
{
    internal class clsPGSQL : iDBCon
    {

        private static NpgsqlCommand getSQLCmd(NpgsqlConnection cnn
            , iCommand cmd, string q)
        {


            NpgsqlCommand sqlcmd = cnn.CreateCommand();
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
        public clsPGSQL(string sConnectionString)
        {
            _connectionString = sConnectionString;
        }


        public DataTable getData(string q, iCommand cmd)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                NpgsqlCommand sqlcmd = getSQLCmd(conn, cmd, q);
                NpgsqlDataAdapter ad = new NpgsqlDataAdapter(sqlcmd);

                try
                {
                    DataTable t = new DataTable();

                    ad.Fill(t);
                    return t;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int exec(string q, iCommand cmd)
        {
            int iReturn = 0;

            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                NpgsqlCommand sqlcmd = getSQLCmd(conn, cmd, q);
                sqlcmd.Connection = conn;

                try
                {
                    iReturn = sqlcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                    iReturn = -1;
                }
            }

            return iReturn;
        }
        public object execScalar(string q, iCommand cmd)
        {
            object ret = DBNull.Value;

            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                NpgsqlCommand sqlcmd = getSQLCmd(conn, cmd, q);
                sqlcmd.Connection = conn;
                try
                {
                    ret = sqlcmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return ret;
        }
    }
}
