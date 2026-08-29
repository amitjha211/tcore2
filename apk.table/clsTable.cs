using System.Data;
using System.Text;
using tcore2;
using tcore2.global;
using apk.sql;
using static tcore2.global.clsGlobal;
using apk.db;
using System.Reflection;

namespace apk.table
{
    public class clsTableEvent
    {
        public string Table = "";
        public string DMLCode = "";
        public int ID = 0;
        public iCommand row;
    }

    public class clsTableModel
    {
        public clsAPK _apk;
        public clsConnectionModel _con = null;
        public string TableName { get; set; }
        public string IDName { get; set; }
        public bool IsIdentity { get; set; }
        public DataTable emptyTable { get; set; }
        public DataTable emptyViewTable { get; set; }

        public string spUpdate { get; set; }
        public string spInsert { get; set; }

        public string ViewName { get; set; }


    }

    public static class clsTableF
    {


        public static clsTableModel createTableModel_old(clsAPK _apk
            , string sTable
            , clsConnectionModel objCon)
        {
            clsTableModel objTable = new clsTableModel();
            objTable.TableName = sTable;
            objTable.IDName = "id";

            init(objTable, objCon);
            return objTable;
        }


        public static bool isDuplicate(clsTableModel objTable, clsConnectionModel objCon, string sField, DataRow r)
        {
            int iID = r.getInt("id");
            clsSQLModel objSQL = new clsSQLModel();
            clsSQLF.init(objSQL, "select * from " + objTable.TableName + " where " + objTable.IDName + " <> " + iID);
            clsSQLF.addFilter(objSQL, sField, r[sField]);

            clsCommand cmdGet = clsSQLF.getCommandSelect(objSQL);
            DataTable t = clsDB.getData(cmdGet, objCon);

            if (t.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        
        private static DataTable createEmptyTable(string sTableOrView,clsConnectionModel con)
        {
            string q = "";

            if (con.DBType == "mssql")
                q = $"select top 0 * from {sTableOrView}";
            else if (con.DBType == "pgsql" || con.DBType == "sqlite")
                q = $"select * from {sTableOrView} limit 0";
            else
                q = $"select top 0 * from {sTableOrView}";

            DataTable t = clsDB.getData(q, con);
             
            return t;
        }
        public static void init(clsTableModel model, clsConnectionModel con)
        {

            string q = "";
            string sViewName = getViewName(model);
            model.emptyViewTable = createEmptyTable(sViewName,con);
            model.emptyTable = createEmptyTable(model.TableName,con);


            generateDMLScript(model, con);
        }

        //public static void getData(clsTableModel model, clsConnectionModel con)
        //{
        //    DataTable t = clsDB.getData($"select top 0 * from {model.TableName} ", con);
        //    model.emptyTable = t;
        //    generateDMLScript(model, con);
        //}

        public static string getViewName(clsTableModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.ViewName)) return model.ViewName;

            return model.TableName;

        }

        public static DataTable getData(clsTableModel model, clsConnectionModel con, int iID)
        {
            string sView = getViewName(model);
            DataTable t = clsDB.getData($"select top 1 * from {sView} where {model.IDName} = {iID}", con);
            return t;

        }


        public static void generateDMLScript(clsTableModel model, clsConnectionModel con)
        {
            model.spInsert = generateInsertScript(model, con);
            model.spUpdate = generateUpdateScript(model, con);
        }

        public static string generateUpdateScript(clsTableModel model
            , clsConnectionModel con)
        {

            StringBuilder sbSQL = new StringBuilder();

            List<string> list = new List<string>();

            bool addComm = false;

            sbSQL.AppendLine($"update {model.TableName} set ");

            foreach (DataColumn col in model.emptyTable.Columns)
            {
                if (col.ColumnName.ToLower() != model.IDName.ToLower())
                {
                    if (addComm == true) sbSQL.Append(",");
                    sbSQL.AppendLine($"{col.ColumnName} = @{col.ColumnName} ");
                    addComm = true;
                }
            }

            sbSQL.AppendLine($" where {model.IDName} = @{model.IDName} ");

            return sbSQL.ToString();
        }

        public static string generateInsertScript(clsTableModel model
            , clsConnectionModel con
            , bool is_id_autoGen = true)
        {

            StringBuilder sbSQL = new StringBuilder();
            List<string> lstColumns = new List<string>();
            List<string> lstParams = new List<string>();

            foreach (DataColumn col in model.emptyTable.Columns)
            {
                if (is_id_autoGen == true && col.ColumnName.ToLower() == model.IDName.ToLower())
                    continue;

                lstColumns.Add(col.ColumnName.ToLower());
                lstParams.Add("@" + col.ColumnName.ToLower());

            }

            sbSQL.AppendLine($"insert into {model.TableName}");
            sbSQL.AppendLine($"(");
            sbSQL.AppendLine(string.Join(',', lstColumns.ToArray()));
            sbSQL.AppendLine($")");

            sbSQL.AppendLine($" values (");
            sbSQL.AppendLine(string.Join(',', lstParams.ToArray()));
            sbSQL.AppendLine($")");

            if (is_id_autoGen)
            {
                if (con.DBType == "mssql")
                    sbSQL.AppendLine("select scope_identity()");
                if (con.DBType == "pgsql")
                    sbSQL.AppendLine($"returning {model.IDName}");
                else if (con.DBType == "sqlite")
                {
                    sbSQL.Append(";");
                    sbSQL.AppendLine("");
                    sbSQL.AppendLine("select last_insert_rowid();");
                }
            }

            return sbSQL.ToString();
        }

        public static void save(clsTableModel model
            , clsConnectionModel con
            , DataTable t)
        {
            foreach (DataRow r in t.Rows)
            {
                save(model, con, r);
            }
        }

        public static void insert(clsTableModel model, clsConnectionModel con, DataTable t)
        {
            foreach (DataRow r in t.Rows)
            {
                insert(model, con, r);
            }
        }
        public static void insert(clsTableModel model, clsConnectionModel con, DataRow r)
        {
            clsCommand cmd = r.getCMD();
            StringBuilder sbSQL = new StringBuilder();

            sbSQL.AppendLine($"SET IDENTITY_INSERT {model.TableName} ON;");
            sbSQL.AppendLine(generateInsertScript(model, con, false));
            sbSQL.AppendLine($"SET IDENTITY_INSERT {model.TableName} OFF;");

            cmd.sql = sbSQL.ToString();
            clsDB.executeNonQuery(cmd, con);
        }


        public static void save(clsTableModel model
            , clsConnectionModel con
            , DataRow r)
        {

            clsCommand cmd = r.getCMD();
            int iID = r.getInt(model.IDName);

            if (iID == 0)
            {
                cmd.sql = model.spInsert;
                object iTmpID = clsDB.executeScalar(cmd, con);
                iID = parseInt(iTmpID);
                r[model.IDName] = iID;
            }
            else
            {
                cmd.sql = model.spUpdate;
                clsDB.executeNonQuery(cmd, con);
            }
        }


        public static void save(clsTableModel model
            , clsConnectionModel con
            , iCommand r)
        {

            string q = "";
            //clsCommand cmd = new clsCommand();
            int iID = r.getInt(model.IDName);

            if (iID == 0)
            {
                q = model.spInsert;
                object iTmpID = clsDB.executeScalar(q, r, con);
                iID = parseInt(iTmpID);
                r.set(model.IDName, iID);
            }
            else
            {
                q = model.spUpdate;
                clsDB.executeNonQuery(q, r, con);
            }
        }

        public static clsMessageModel deleteRow(clsTableModel objTable
            , clsConnectionModel objCon
            , DataRow r)
        {
            int iID = r.getInt(objTable.IDName);
            string q = "delete from " + objTable.TableName + " where " + objTable.IDName + " = " + iID;
            try
            {
                clsDB.executeNonQuery(q, objCon);
                return msg();
            }
            catch (Exception ex)
            {
                return msg(ex.Message);
            }
            return msg();
        }

        public static string getNextVoucher(clsTableModel objTable, string sFieldVoucher, string sWhereRaw, clsConnectionModel objCon)
        {
            clsSQLModel oSQL = clsSQLF.createSQLModel(objTable._apk, $"select isNull(max({sFieldVoucher}),'')  as val from {objTable.TableName} where 1=1 ");

            if (!string.IsNullOrWhiteSpace(sWhereRaw))
                clsSQLF.addRawFilter(oSQL, sWhereRaw);

            clsCommand cmd = clsSQLF.getCommandSelect(oSQL);
            string sVal = (string)clsDB.executeScalar(cmd, objCon);
            sVal = getNextVoucherNo(sVal);
            return sVal;
        }

    }
}
