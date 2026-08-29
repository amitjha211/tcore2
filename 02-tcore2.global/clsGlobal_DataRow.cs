using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using g;

namespace tcore2.global
{

    public static partial class clsGlobal
    {


        [DebuggerStepThrough]
        public static clsCommandBase create_iCommandRow(DataRow r)
        {
            clsCommandDataRow rReturn = new clsCommandDataRow(null);
            return rReturn;
        }


        [DebuggerStepThrough]
        public static IEnumerable<clsCommandBase> getRows(this DataTable dt)
        {
            clsCommandDataRow r = new clsCommandDataRow(null);
            foreach (DataRow row in dt.Rows)
            {
                r.setObject(row);
                yield return r;
            }
        }


        [DebuggerStepThrough]
        public static List<T> ConvertDataTable<T>(this DataTable dt) => ds.dt_getList<T>(dt);
        

        [DebuggerStepThrough]
        public static int getInt(this DataRow r, int iCol)
        {
            int iVal = 0;
            string sVal = "";

            try
            {
                sVal = r[iCol].ToString();
            }
            catch (Exception ex)
            {
                throw new Exception_Core(ex.Message);
            }

            int.TryParse(sVal, out iVal);


            return iVal;

        }

        [DebuggerStepThrough]
        public static int getInt(this DataRow r, string sField)
        {
            int iVal = 0;

            string sVal = "";

            try
            {
                sVal = r[sField].ToString();
            }
            catch (Exception ex)
            {
                throw new Exception_Core(ex.Message);
            }

            int.TryParse(sVal, out iVal);

            return iVal;

        }
        
        [DebuggerStepThrough]
        public static bool getBool(this DataRow r, string sField)
        {
            bool ret = false;

            try
            {
                object objVal = r[sField];
                if (objVal == DBNull.Value) return false;
                if ((bool)objVal == true) return true;
            }
            catch (Exception ex)
            {
                throw new Exception_Core(ex.Message);
            }


            return ret;
        }


        [DebuggerStepThrough]
        public static void copyFrom(this DataRow r, JObject jnRow)
        {

            JArray jnArray = new JArray();
            jnArray.Add(jnRow);

            DataTable tSource = jnArray.ToObject<DataTable>();

            copyFrom(r, tSource.Rows[0]);
        }

        [DebuggerStepThrough]
        public static void copyFrom(this DataRow r, DataRow rSource)
        {
            DataTable tSource = rSource.Table;
            DataTable tDest = r.Table;

            ///////////////////////////////////////////////

            foreach (DataColumn col in tDest.Columns)
            {
                string sColumnName = col.ColumnName;

                if (tSource.Columns.Contains(sColumnName))
                {
                    r[sColumnName] = rSource[sColumnName];
                }
            }
        }

        
        [DebuggerStepThrough]
        public static decimal getDecimal(this DataRow r, int iCol)
        {
            decimal iVal = 0;
            string sVal = "";

            try
            {
                sVal = r[iCol].ToString();
                decimal.TryParse(sVal, out iVal);
            }
            catch (Exception ex)
            {
                throw new Exception_Core(ex.Message);
            }

            return iVal;

        }

        [DebuggerStepThrough]
        public static decimal getDecimal(this DataRow r, string sField)
        {
            decimal iVal = 0;

            string sVal = "";

            try
            {
                sVal = r[sField].ToString();
            }
            catch (Exception ex)
            {
                throw new Exception_Core(ex.Message);
            }


            decimal.TryParse(sVal, out iVal);

            return iVal;
        }

        [DebuggerStepThrough]
        public static string getStringDateFormat(this DataRow r
            , string sField, string sDateFormat)
        {
            object dt = r[sField];
            if (dt == DBNull.Value) return "";
            return ((DateTime)dt).ToString(sDateFormat);
        }
        
        [DebuggerStepThrough]
        public static string getStringDateFormat(this DataRow r
            , int iCol, string sDateFormat)
        {

            object dt = r[iCol];
            if (dt == DBNull.Value || dt == null) return "";
            return ((DateTime)dt).ToString(sDateFormat);
        }

        [DebuggerStepThrough]
        public static string getString(this DataRow r, string sField)
        {

            return r[sField].ToString();
        }


        [DebuggerStepThrough]
        public static string getString(this DataRow r, int iCol)
        {
            return r[iCol].ToString();
        }

        /// <summary>
        /// Data Row View
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>

        [DebuggerStepThrough]
        public static int getInt(this DataRowView r, int iCol)
        {
            int iVal = 0;
            string sVal = r[iCol].ToString();
            int.TryParse(sVal, out iVal);
            return iVal;
        }

        [DebuggerStepThrough]
        public static int getInt(this DataRowView r, string sField)
        {
            int iVal = 0;
            string sVal = r[sField].ToString();
            int.TryParse(sVal, out iVal);
            return iVal;
        }

        [DebuggerStepThrough]
        public static decimal getDecimal(this DataRowView r, int iCol)
        {
            decimal iVal = 0;
            string sVal = r[iCol].ToString();
            decimal.TryParse(sVal, out iVal);
            return iVal;

        }

        [DebuggerStepThrough]
        public static decimal getDecimal(this DataRowView r, string sField)
        {
            decimal iVal = 0;


            string sVal = r[sField].ToString();

            decimal.TryParse(sVal, out iVal);


            return iVal;
        }



        [DebuggerStepThrough]
        public static string getString(this DataRowView r, string sField)
        {

            return r[sField].ToString();

        }


        [DebuggerStepThrough]
        public static clsCommand getCMD(this DataRow r, params string[] cols)
        {
            clsCommand cmd = new clsCommand();



            foreach (string sCol in cols)
            {
                cmd.set(sCol, r[sCol]);
            }

            return cmd;
        }

        [DebuggerStepThrough]
        public static clsCommand getCMD(this DataRow r)
        {
            clsCommand cmd = new clsCommand();

            foreach (DataColumn col in r.Table.Columns)
            {
                cmd.set(col.ColumnName, r[col.ColumnName]);
            }

            return cmd;
        }

        [DebuggerStepThrough]
        public static string getString(this DataRowView r, int iCol)
        {
            return r[iCol].ToString();
        }


        [DebuggerStepThrough]
        public static T GetItem<T>(this DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        [DebuggerStepThrough]
        public static DataTable convertToDataTable<T>(this List<T> list
            , string _tableName)
        {
            DataTable dt = new DataTable(_tableName);

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    row[info.Name] = info.GetValue(t, null) ?? DBNull.Value;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
        
        [DebuggerStepThrough]
        public static void setValue(this DataTable t
            , string sField
            , object val
            , string sFilter)
        {
            DataRow[] _rows = t.Select(sFilter);
            foreach (DataRow r in _rows)
            {
                r[sField] = val;
            }
        }

        [DebuggerStepThrough]
        public static void insertFrom(this DataTable t, DataRow[] _rows)
        {
            foreach (DataRow rSource in _rows)
            {
                DataRow r = t.NewRow();
                r.copyFrom(rSource);

                t.Rows.Add(r);
            }
        }

        [DebuggerStepThrough]
        public static JObject getJObject(this DataRow r, params string[] sCols)
        {

            JObject jn = new JObject();

            if (sCols.Length > 0)
                foreach (string sCol in sCols) jn[sCol] = new JValue(r[sCol]);
            else
            {
                foreach (DataColumn col in r.Table.Columns)
                    jn[col.ColumnName] = new JValue(r[col.ColumnName]);
            }

            return jn;
        }

        [DebuggerStepThrough]
        public static JObject getJObject_ExcludeColumns(this DataRow r, params string[] sIgnoreColumns)
        {
            JObject jn = new JObject();

            List<string> arr = new List<string>();
            foreach (DataColumn col in r.Table.Columns)
            {
                if (!sIgnoreColumns.Contains(col.ColumnName))
                    jn[col.ColumnName] = new JValue(r[col.ColumnName]);
            }

            return jn;
        }

        [DebuggerStepThrough]
        public static JArray getJArray(this DataTable t)
        {
            return JArray.FromObject(t);
        }

    }
}
