using apk.db;
using apk.sql;
using apk.table;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using tcore2.global;
using tcore2.progress;
using static apk.clsAPKF_FS_F;
using static apk.clsAPK_DBF;

namespace apk.dts;

public class clsAPK_csvF
{

    public static void init(clsAPK_Module _m, iLog objLog)
    {
        clsAPKF.setEnvObject(_m, "objLog", objLog);
        ///
        string sTable = f_getStr(_m, "table-name");
        clsTableModel objTable = db_create_table(_m._apk, "trTrade");
        clsAPKF.setEnvObject(_m, "objTable", objTable);

        //-------------------------------------

        DataTable tDestination = objTable.emptyTable.Clone();
        clsAPKF.setEnvObject(_m, "tDestination", tDestination);
    }

    public static void loadCSV(clsAPK_Module _m, string sCSVFile)
    {
        DataTable tSource = clsGlobal.readCSVFromFile(sCSVFile);
        clsAPKF.setEnvObject(_m, "tSource", tSource);
    }


    public static void save_data(clsAPK_Module _m)
    {
        string sTable = f_getStr(_m, "table-name");
        clsTableModel objTable = clsAPKF.getEnvObject(_m, "objTable") as clsTableModel;
    }
    public static void transferDataFromSourceToDestination(clsAPK_Module _m
            , bool saveData)
    {
        clsConnectionModel objCon = clsAPKF.getCon(_m._apk);
        iLog objLog = clsAPKF.getEnvObject(_m, "objLog") as iLog;

        DataTable tDestination = clsAPKF.getEnvObject(_m, "tDestination") as DataTable;
        DataTable tSource = clsAPKF.getEnvObject(_m, "tSource") as DataTable;

        string[] sUniqueColumns = f_getStr(_m, "unique-columns").Split(',');

        List<clsColMap> lstMap = clsAPKF.getColMap(_m, "col-map");

        string sTable = f_getStr(_m, "table-name");
        clsTableModel objTable = clsAPKF.getEnvObject(_m, "objTable") as clsTableModel;

        Action<DataRow> modifyRow = clsAPKF.getEnvObject(_m, "modifyRow") as Action<DataRow>;

        clsMapF.setColIndexes(lstMap, tSource, tDestination);

        int iCount = tSource.Rows.Count;
        int iRow = 0;

        objLog.progressInit("upload started...", iCount);


        foreach (DataRow rSource in tSource.Rows)
        {
            DataRow rNew = tDestination.NewRow();
            clsMapF.fillRowByIndex(lstMap, rSource, rNew);
            update_id(_m, objCon, rNew, sTable, sUniqueColumns);
            tDestination.Rows.Add(rNew);

            if (modifyRow != null) modifyRow(rNew);

            if (saveData)
            {
                clsTableF.save(objTable, objCon, rNew);
            }
            iRow++;
            objLog.progress(iCount, iRow);
        }
        objLog.progressEnd("Upload Done");
    }


    public static void update_id(clsAPK_Module _m
        , clsConnectionModel objCon
        , DataRow rDest
        , string sTable
        , string[] sUniqueColumns)
    {

        clsSQLModel objSQL = clsSQLF.createSQLModel(_m._apk, $"select * from {sTable} where 1=1 ");
        foreach (string sField in sUniqueColumns)
        {
            clsSQLF.addFilter(objSQL, sField, rDest[sField]);
        }
        clsCommand cmd = clsSQLF.getCommandSelect(objSQL);
        DataTable t = clsDB.getData(cmd, objCon);
        if (t.Rows.Count > 0)
        {
            rDest["id"] = t.Rows[0]["id"];
        }
    }
}

