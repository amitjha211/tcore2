using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;
using System.Data;
using tcore2;


namespace apk.db
{

    internal class clsSQLite : iDBCon
    {
        private static SqliteCommand getSQLCmd(SqliteConnection cnn
            , iCommand cmd, string sql)
        {

            SqliteCommand sqlcmd = cnn.CreateCommand();
            sqlcmd.CommandText = sql;

            //foreach (iParam p in cmd.prm)
            for (int i = 0; i < cmd.getCount(); i++)
            {
                string sKey = cmd.getKey(i);
                object objValue = cmd.get(i);
                sqlcmd.Parameters.AddWithValue(sKey, objValue);
            }

            return sqlcmd;
        }

        private static DataTable createTable(SqliteDataReader dr)
        {

            DataTable t = new DataTable();

            DataTable tSchema = dr.GetSchemaTable();

            foreach (DataRow rSchema in tSchema.Rows)
            {
                Type fieldType = rSchema["dataType"] as Type;
                string sColumnName = rSchema["ColumnName"].ToString();
                string DBDataTypeName = rSchema["DataTypeName"].ToString();

                if (DBDataTypeName == "bit")
                    fieldType = typeof(bool);

                t.Columns.Add(sColumnName, fieldType);
            }

            return t;

        }
        private string _connectionString = "";
        public clsSQLite(string sConnectionString)
        {
            _connectionString = sConnectionString;
        }

        public DataTable getData(string q, iCommand cmd)
        {

            using (SqliteConnection conn = new SqliteConnection(_connectionString))
            {
                conn.Open();

                SqliteCommand sqlcmd = getSQLCmd(conn, cmd, q);

                DataTable t = null;

                using (SqliteDataReader dr = sqlcmd.ExecuteReader())
                {
                    do
                    {
                        t = createTable(dr);

                        t.BeginLoadData();
                        t.Load(dr);
                        t.EndLoadData();
                    } while (!dr.IsClosed && dr.NextResult());
                }
                return t;
            }


        }

        public int exec(string q, iCommand cmd)
        {
            int iReturn = 0;

            using (SqliteConnection conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                SqliteCommand sqlcmd = getSQLCmd(conn, cmd, q);
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

            using (SqliteConnection conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                SqliteCommand sqlcmd = getSQLCmd(conn, cmd, q);
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
