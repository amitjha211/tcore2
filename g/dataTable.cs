using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace g
{
    public static class ds
    {

        ///ConvertDataTable
        public static List<T> dt_getList<T>( DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = dr_createClass<T>(row);
                data.Add(item);
            }
            return data;
        }

        public static T dr_createClass<T>(DataRowView r) => dr_createClass<T>(r);

        public static T dr_createClass<T>(DataRow r)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in r.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, r[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }


        public static void dr_copy(DataRow r, DataRow rSource)
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


    }
}
