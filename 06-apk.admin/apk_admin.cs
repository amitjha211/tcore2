using apk.db;
using System.Data;
using System.Xml;
using tcore2;
using tcore2.global;
using static apk.clsAPKF_FS_F;

namespace apk.admin
{
    public class apk_admin
    {
        private static string get_apk_folder()
        {
            return Environment.GetEnvironmentVariable("apk_path");
        }

        public static void con_init(clsConnectionModel con)
        {
            switch (con.DBType.ToLower())
            {
                case "mssql":
                    con.objCon = new clsMSSQL(con.ConnectionString);
                    break;
                case "sqlite":
                    con.objCon = new clsSQLite(con.ConnectionString);
                    break;
                case "pgsql":
                    con.objCon = new clsPGSQL(con.ConnectionString);
                    break;
            }
        }
        public static clsAPK_Module apk_module_create(clsAPK mAPK, string sVPath)
        {
            string sXml = f_getStr(mAPK, sVPath);
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(sXml);

            clsAPK_Module _m = new clsAPK_Module();
            _m._apk = mAPK;
            _m.xDoc = xDoc.DocumentElement;
            _m.VPath = sVPath;
            _m.AbsPath = f_getFullOSPath(mAPK, sVPath);

            //Init Module


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

            return _m;
        }

        private static clsConnectionModel apk_con_create(clsAPK _apk
            , string sConnectionString
            , string sDBType)
        {

            

            clsConnectionModel objCon = new clsConnectionModel();
            objCon.ConnectionString = sConnectionString;
            objCon.DBType = sDBType;

            try
            {
                con_init(objCon);
                return objCon;
            }
            catch (Exception ex)
            {
                throw new Exception_Core(ex.Message);
            }
            
            return null;

        }

        public static clsConnectionModel apk_con_create(clsAPK _apk
        , string sDBName)
        {

            string sDBType = clsAPKF.getEnv(_apk, "db_type");
            string sServer = clsAPKF.getEnv(_apk, "db_server");
            string sUserID = clsAPKF.getEnv(_apk, "db_userid");
            string sPwd = clsAPKF.getEnv(_apk, "db_pwd");

            string sConnectionString = string.Format("Data Source={0};initial catalog={1};user id={2};pwd={3};TrustServerCertificate=True;", sServer, sDBName, sUserID, sPwd);
            clsConnectionModel objCon = clsDB.createConnection(sConnectionString, sDBType);
            return objCon;
        }


        public static clsAPK apk_create(string sAppName
            , string sDBName
            , string sEnv)
        {

            clsAPK _apk = new clsAPK();
            _apk.appFolder = get_apk_folder();
            _apk.appName = sAppName;
            clsAPKF.setEnv(_apk, "env", sEnv);

            setDBConfig(_apk);

            clsAPKF.setEnv(_apk, "db_name", sDBName);

            apk_init_db(_apk, "master", "con_master");
            apk_init_db(_apk, $"schema_db", "con_schema");
            apk_init_db(_apk, sDBName, "con_main");


            return _apk;

        }


        public static void apk_init_db(clsAPK _apk
            , string sDBName
            , string sConName)
        {
            clsConnectionModel objCon = apk_con_create(_apk, sDBName);
            clsAPKF.setEnvObject(_apk, sConName, objCon);
        }


        private static void setDBConfig(clsAPK _apk)
        {

            string sEnv = clsAPKF.getEnv(_apk, "env");

            clsAPK_Module _m_app = clsAPKF.createModule(_apk, "@/app.m.xml");
            clsCommand cmd = clsAPKF.getCmd(_m_app, $"env/env.{sEnv}");

            string sDB_Type = cmd.getString("db_type");
            string sDB_Server = cmd.getString("db_server");
            string sDB_UserID = cmd.getString("db_userid");
            string sDB_Pwd = cmd.getString("db_pwd");

            clsAPKF.setEnv(_apk, "db_type", sDB_Type);
            clsAPKF.setEnv(_apk, "db_server", sDB_Server);
            clsAPKF.setEnv(_apk, "db_userid", sDB_UserID);
            clsAPKF.setEnv(_apk, "db_pwd", sDB_Pwd);

        }

        private static void set_apk_env(clsAPK _apk, clsCommand cmd)
        {
            for (int i = 0; i < cmd.prm.Count; i++)
            {
                string sKey = cmd.getKey(i);
                string sValue = cmd.getString(i);

                clsAPKF.setEnv(_apk, sKey, sValue);
            }
        }

    }

}
