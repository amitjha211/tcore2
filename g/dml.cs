using apk.db;
using apk.sql;
using apk.table;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcore2.global;
using static g.db;

using static tcore2.global.clsGlobal;

namespace g
{
    public class dml
    {

        public static clsTableModel dml_create_table(clsConnectionModel objCon
            , string sTable
            , string sID = "id")
        {
            clsTableModel objTable = new clsTableModel();
            objTable.TableName = sTable;
            objTable.IDName = sID;
            objTable._con = objCon;
            clsTableF.init(objTable, objCon);

            return objTable;
        }


        public string dml_get_script_insert()
        {
            return null;
        }


        public static void dml_save(clsTableModel model
         , DataRow r)
        {
            dml_save(model, model._con, r);
        }

        public static void dml_save(clsTableModel model, clsConnectionModel con, DataRow r)
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


        public static void dml_update_id(clsConnectionModel objCon, DataTable tDest, string sTable, string sIDName, params string[] sColumns)
        {
            foreach (DataRow r in tDest.Rows)
            {
                dml_update_id(objCon, r, sTable, sIDName, sColumns);
            }
        }

        public static void dml_update_id(clsConnectionModel objCon, DataRow rDest, string sTable, string sIDName, params string[] sColumns)
        {

            string sOrderID = rDest.getString("order_id");
            clsSQLModel objSQL = clsSQLF.createSQLModel($"select {sIDName} from {sTable} where 1=1");

            foreach (string sColumn in sColumns)
            {
                clsSQLF.addFilter(objSQL, sColumn, rDest[sColumn]);
            }
            clsCommand cmd = clsSQLF.getCommandSelect(objSQL);
            DataTable t = get_data(objCon, cmd);

            if (t.Rows.Count > 0)
            {
                rDest[sIDName] = t.Rows[0][sIDName];
            }
        }
    }



}
