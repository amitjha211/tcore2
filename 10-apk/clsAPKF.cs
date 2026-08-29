using Newtonsoft.Json.Linq;
using System.Data;
using System.Runtime.Serialization.DataContracts;
using System.Text;
using System.Xml;
using apk.db;
using apk.sql;
using apk.table;
using apk.dts;
using tcore2;
using tcore2.global;

using static apk.clsAPKF_FS_F;

namespace apk;

public class clsAPKF
{

    public static void raiseEvent(clsAPK_Module _m
        , string sEvent
        , clsAPK_Module_Event e)
    {
        if (_m.e != null)
        {
            _m.e(sEvent, e);
        }
    }




    public static DataTable getData(clsAPK_Module _m
        , clsSQLModel objSQL
        , string sCon = "con_main")
    {
        clsConnectionModel objCon = clsAPKF.getEnvObject(_m._apk, sCon) as clsConnectionModel;
        clsCommand cmd = clsSQLF.getCommandSelect(objSQL);
        DataTable t = clsDB.getData(cmd, objCon);
        return t;
    }


    public static DataTable getData(clsAPK_Module _m
        , string sqlName
        , string sCon = "con_main"
        , params object[] args)
    {
        string sSQL = f_getStr(_m, sqlName, args);
        clsConnectionModel objCon = clsAPKF.getEnvObject(_m._apk, sCon) as clsConnectionModel;
        DataTable t = clsDB.getData(sSQL, objCon);
        return t;
    }

    public static DataTable getData(clsAPK _apk
        , string sqlFile
        , string sCon = "con_main"
        , params object[] args)
    {
        clsConnectionModel objCon = clsAPKF.getEnvObject(_apk, sCon) as clsConnectionModel;
        string sSQL = f_getStr(_apk, sqlFile);
        DataTable t = clsDB.getData(sSQL, objCon);
        return t;
    }

    //public static clsTableModel getTable(clsAPK _apk
    //    , string sTable
    //    , string sCon = "con_main")
    //{

    //    clsConnectionModel objCon = clsAPKF.getCon(_apk, sCon);
    //    clsTableModel objTable = clsTableF.createTableModel(_apk, sTable,objCon);
    //    return objTable;
    //}

    public static clsCommand getCmd(clsAPK_Module _m
        , string sName)
    {
        if (!f_exists(_m, sName)) return null;

        string sXml = f_getStr(_m, sName);
        return clsAPK_ObjectF.createCmd(sXml);
    }
    public static clsCommand getCmd(clsAPK _apk, string sqlFile)
    {
        string sXml = f_getStr(_apk, sqlFile);
        return clsAPK_ObjectF.createCmd(sXml);
    }

    public static JArray getJArray(clsAPK_Module m
        , string sName)
    {
        string strArray = f_getStr(m, sName);
        return JArray.Parse(strArray);
    }

    public static JArray getJArray(clsAPK mAPK
        , string sFile)
    {
        string strArray = f_getStr(mAPK, sFile);
        return JArray.Parse(strArray);
    }

    public static clsSQLModel getSQLModel(clsAPK_Module m
        , string sName)
    {
        string q = f_getStr(m, sName);
        return clsAPK_ObjectF.createSQLModel(m._apk, q);
    }

    public static clsSQLModel getSQLModel(clsAPK mAPK, string sqlFile)
    {
        string sSQL = f_getStr(mAPK, sqlFile);
        return clsAPK_ObjectF.createSQLModel(mAPK, sSQL);
    }

    public static clsAPK_Module createModule(clsAPK mAPK, string sVPath)
    {
        string sXml = f_getStr(mAPK, sVPath);
        XmlDocument xDoc = new XmlDocument();
        xDoc.LoadXml(sXml);

        clsAPK_Module m = new clsAPK_Module();
        m._apk = mAPK;
        m.xDoc = xDoc.DocumentElement;
        m.VPath = sVPath;
        m.AbsPath = f_getFullOSPath(mAPK, sVPath);

        init_module(m);

        return m;
    }

    public static clsAPK_Module createModule(clsAPK_Module _m, string sVPath)
    {

        string sXml = f_getStr(_m, refine_VPath(sVPath));
        XmlDocument xDoc = new XmlDocument();
        xDoc.LoadXml(sXml);

        clsAPK_Module m = new clsAPK_Module();
        m._apk = _m._apk;
        m.xDoc = xDoc.DocumentElement;


        DataRow[] _rows = _m.t.Select($" path = '{refine_VPath(sVPath)}' ");
        if (_rows.Length > 0)
        {
            m.VPath = _rows[0].getString("src");
        }
        else
        {
            m.VPath = sVPath;
        }

        m.AbsPath = f_getFullOSPath(_m._apk, m.VPath);

        init_module(m);
        return m;

    }

    

    public static clsAPK_Module createModule(clsAPK mAPK, XmlNode xNode)
    {

        clsAPK_Module m = new clsAPK_Module();
        m._apk = mAPK;
        m.xDoc = xNode;

        init_module(m);

        return m;
    }


    public static void init_module(clsAPK_Module _m)
    {
        //
        foreach (clsParam p in _m._apk.env.prm)
        {
            _m.env.set(p.name, p.value);
        }

        _m.t = clsAPKF.getJArray(_m._apk, "@/common/m.json").ToObject<DataTable>();
        _m.t.Rows.Clear();
        _m.t.Columns.Add("obj", typeof(object));

        XmlNodeList xNode_Modules = _m.xDoc.SelectNodes("//modules/module");

        foreach (XmlNode xNode_Module in xNode_Modules)
        {
            XmlNodeList xNode_Objects = xNode_Module.SelectNodes("obj");
            string sModuleName = xNode_Module.getXmlAttributeValue("name");

            foreach (XmlNode xNode_Object in xNode_Objects)
            {
                string sObject_Name = xNode_Object.getXmlAttributeValue("name");
                string sSrc = xNode_Object.getXmlAttributeValue("src");
                string sType = xNode_Object.getXmlAttributeValue("type");
                string sContent = f_getStr(_m._apk, xNode_Object);
                string sObj_id = xNode_Object.getXmlAttributeValue("id");

                DataRow r = _m.t.NewRow();

                r["module_name"] = sModuleName;
                r["object_name"] = sObject_Name;
                r["path"] = $"{sModuleName}/{sObject_Name}";
                r["type"] = sType;
                r["src"] = sSrc;
                r["content"] = sContent;
                r["id"] = sObj_id;

                _m.t.Rows.Add(r);
            }
        }
    }

    public static List<clsColMap> getColMap(clsAPK_Module _m
        , string sName)
    {
        string sXml = f_getStr(_m, sName);
        return clsAPK_ObjectF.createColMap(sXml);
    }
    public static List<clsColMap> getColMap(clsAPK _apk, string sqlFile)
    {
        string sXml = f_getStr(_apk, sqlFile);
        return clsAPK_ObjectF.createColMap(sXml);
    }


    public static void setEnv(clsAPK objAPK, clsCommand cmd)
    {
        objAPK.env = cmd;
    }

    public static void setEnv(clsAPK objAPK, string sKey, string sValue)
    {
        objAPK.env.set(sKey, sValue);
    }
    public static string getEnv(clsAPK objAPK, string sKey)
    {
        return objAPK.env.getString(sKey);
    }
    public static void setEnvObject(clsAPK objAPK, string sKey, object objValue)
    {
        objAPK.env.set(sKey, objValue);
    }
    public static void setEnvObject(clsAPK_Module _m, string sKey, object objValue)
    {
        _m.env.set(sKey, objValue);
    }
    public static object getEnvObject(clsAPK objAPK
        , string sKey)
    {
        if (!objAPK.env.Contains(sKey))
            return null;
        else
            return objAPK.env.get(sKey);
    }

    public static object getEnvObject(clsAPK_Module _m
        , string sKey)
    {
        if (!_m.env.Contains(sKey))
            return null;
        else
            return _m.env.get(sKey);
    }
    public static clsConnectionModel getCon(clsAPK _apk, string sCon = "con_main")
    {
        clsConnectionModel objCon = clsAPKF.getEnvObject(_apk, sCon) as clsConnectionModel;
        if (objCon == null) throw new Exception_Core($"Connection not {sCon} found ");
        return objCon;
    }

}

