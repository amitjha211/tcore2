using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace g
{
    public static class csv
    {

        public static DataTable csv_read_url(string sUrl)
        {
            WebClient webClient = new WebClient();
            try
            {
                using Stream stream = webClient.OpenRead(sUrl);
                using StreamReader reader = new StreamReader(stream);
                using CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                using CsvDataReader reader2 = new CsvDataReader(csv);
                DataTable dataTable = new DataTable();
                dataTable.Load(reader2);
                foreach (DataColumn column in dataTable.Columns)
                {
                    column.ColumnName = column.ColumnName.Trim();
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                throw new exception_g(ex.Message);
            }
            finally
            {
                ((IDisposable)webClient)?.Dispose();
            }
        }

        public static DataTable csv_read_file(string sFile)
        {
            try
            {
                using StreamReader reader = new StreamReader(sFile);
                using CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                using CsvDataReader reader2 = new CsvDataReader(csv);
                DataTable dataTable = new DataTable();
                dataTable.Load(reader2);
                foreach (DataColumn column in dataTable.Columns)
                {
                    column.ColumnName = column.ColumnName.Trim();
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                throw new exception_g(ex.Message);
            }

            return null;
        }
    }
}
