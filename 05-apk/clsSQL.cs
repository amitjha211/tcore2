using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcore2;
using tcore2.global;
using static tcore2.global.clsGlobal;

namespace apk.sql
{
    public class clsSQLModel
    {
        public string dbType = "mssql";
        internal string sPaging = "offset {0} rows  fetch next {1} rows only ";
        
        internal StringBuilder selectQuery = new StringBuilder();
        internal StringBuilder _where = new StringBuilder();
        internal StringBuilder _orderBy = new StringBuilder();
        internal StringBuilder _groupBy = new StringBuilder();
        internal StringBuilder _sbPaging = new StringBuilder();
        internal clsCommand cmd = new clsCommand();
    }


    public class clsSQLF2
    {

        public static clsSQLModel q_create_model(string sql)
        {
            clsSQLModel _model = new clsSQLModel();
            clsSQLF.init(_model, sql);
            return _model;
        }

        public static void q_add_filter(clsSQLModel objSQL
            , string sName
            , object value
            , string sOperator = "=")
        {
            clsParam prm = new clsParam();

            prm.name = sName.Replace('.', '_');
            prm.value = value;
            objSQL._where.AppendFormat(" and ( {0} {1} @{2} ) ", sName, sOperator, prm.name);

            objSQL.cmd.set(prm.name, value);
        }

        public static clsCommand q_get_cmd_select(clsSQLModel objSQL)
        {
            clsCommand cmd = new clsCommand();
            cmd.sql = clsSQLF.getSQL(objSQL);
            foreach (clsParam prm in objSQL.cmd.prm) cmd.set(prm.name, prm.value);

            return cmd;
        }

        public static void q_add_in(clsSQLModel objSQL, string sName, params string[] values)
        {

            List<string> lstValues = new List<string>();

            foreach (string sValue in values)
            {
                lstValues.Add(string.Format("'{0}'", sValue));
            }

            objSQL._where.AppendFormat(" and ( {0} IN ({1}) )", sName, string.Join(",", lstValues.ToArray()));
        }

        public static void q_add_raw(clsSQLModel objSQL, string sFilter)
        {
            objSQL._where.AppendFormat(" and ({0}) ", sFilter);
        }


    }
    public class clsSQLF
    {

        public static clsSQLModel createSQLModel(string sql)
        {
            clsSQLModel _model = new clsSQLModel();
            clsSQLF.init(_model, sql);
            return _model;
        }

        public static clsSQLModel createSQLModel(clsAPK _apk, string sql)
        {
            clsSQLModel _model = new clsSQLModel();
            clsSQLF.init(_model, sql);
            return _model;
        }

        public static void init(clsSQLModel objSQL
            , string sql)
        {
            setSelectSQL(objSQL, sql);
        }

        public static void clearFilter(clsSQLModel objSQL)
        {
            objSQL.cmd.prm.Clear();
            objSQL._where.Clear();
        }

        public static void setSelectSQL(clsSQLModel objSQL, string sql)
        {
            //selectQuery = new StringBuilder();
            objSQL.selectQuery.Clear();
            objSQL.selectQuery.Append(sql);

            if (sql.ToLower().Contains("where") == false)
            {
                objSQL.selectQuery.AppendLine(" where 1=1 ");
            }
        }


        public static void setOrderBy(clsSQLModel objSQL, string sOrderBy)
        {
            objSQL._orderBy = new StringBuilder(sOrderBy);
        }
        public static void setGroupBy(clsSQLModel objSQL, string sGroupBy)
        {
            objSQL._groupBy = new StringBuilder(sGroupBy);
        }

        public static void addFilter(clsSQLModel objSQL
            , string sName
            , object value
            , string sOperator = "=")
        {
            clsParam prm = new clsParam();

            prm.name = sName.Replace('.', '_');
            prm.value = value;
            objSQL._where.AppendFormat(" and ( {0} {1} @{2} ) ", sName, sOperator, prm.name);

            objSQL.cmd.set(prm.name, value);
        }


        public static void addRawFilter(clsSQLModel objSQL, string sFilter)
        {
            objSQL._where.AppendFormat(" and ({0}) ", sFilter);
        }

        public static void addIn(clsSQLModel objSQL, string sName, params string[] values)
        {

            List<string> lstValues = new List<string>();

            foreach (string sValue in values)
            {
                lstValues.Add(string.Format("'{0}'", sValue));
            }

            objSQL._where.AppendFormat(" and ( {0} IN ({1}) )", sName, string.Join(",", lstValues.ToArray()));
        }

        public static void addFilterBetween(clsSQLModel objSQL, string sName, object value1, string value2)
        {
            objSQL.cmd.set(sName, value1);
            objSQL.cmd.set(sName + "2", value2);
            objSQL._where.AppendFormat(" and ( {0} between @{0} and @{0}2 ) ", sName);
        }
        public static void addFilterBetween(clsSQLModel objSQL, string sName, DateTime value1, DateTime value2)
        {
            objSQL.cmd.set(sName, value1.ToString("dd/MMM/yyyy"));
            objSQL.cmd.set(sName + "2", value2.ToString("dd/MMM/yyyy"));
            objSQL._where.AppendFormat(" and ( {0} between @{0} and @{0}2 ) ", sName);
        }

        public static void addOrderBy(clsSQLModel objSQL, string sOrderBy)
        {
            objSQL._orderBy = new StringBuilder(sOrderBy);
        }

        public static void addGroupBy(clsSQLModel objSQL, string sGroupBy)
        {
            objSQL._orderBy = new StringBuilder(sGroupBy);
        }
        public static void addPaging(clsSQLModel objSQL, int iOffSet, int ilimit)
        {
            string sPaging = string.Format(objSQL.sPaging, iOffSet, ilimit);
            objSQL._sbPaging.AppendLine(sPaging);
        }

        public static string getSQL(clsSQLModel objSQL)
        {
            StringBuilder sbSQL = new StringBuilder();

            sbSQL.AppendLine(objSQL.selectQuery.ToString());
            sbSQL.AppendLine("");
            sbSQL.AppendLine(objSQL._where.ToString());
            sbSQL.AppendLine("");
            sbSQL.AppendLine(objSQL._groupBy.ToString());
            sbSQL.AppendLine("");
            sbSQL.AppendLine(objSQL._orderBy.ToString());
            sbSQL.AppendLine("");

            return sbSQL.ToString();
        }

        public static string getSQLPaging(clsSQLModel objSQL)
        {
            StringBuilder sbSQL = new StringBuilder();

            sbSQL.AppendLine(objSQL.selectQuery.ToString());
            sbSQL.AppendLine(objSQL._where.ToString());
            sbSQL.AppendLine(objSQL._orderBy.ToString());
            sbSQL.AppendLine(objSQL._groupBy.ToString());
            sbSQL.AppendLine(objSQL._sbPaging.ToString());
            return sbSQL.ToString();
        }

        public static clsCommand getCommandSelect(clsSQLModel objSQL)
        {
            clsCommand cmd = new clsCommand();
            cmd.sql = getSQL(objSQL);
            foreach (clsParam prm in objSQL.cmd.prm) cmd.set(prm.name, prm.value);
            
            return cmd;
        }
    }
}
