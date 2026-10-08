using Newtonsoft.Json.Linq;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using tcore2.global;
using Microsoft.Win32.SafeHandles;
using apk.exception;
using apk.dts;
using g;

namespace apk
{
    public static partial class clsAPKF_FS_F
    {

        [DebuggerStepThrough]
        public static string f_getStr(clsAPK_Module m, string sPathOrObjectName)
        {
            if (sPathOrObjectName.StartsWith("@"))
                return f_getStr(m._apk, sPathOrObjectName);


            string _path = refine_VPath(sPathOrObjectName);
            DataRow[] _rows = m.t.Select($" path = '{_path}' ");

            if (_rows != null && _rows.Length > 0)
            {
                return _rows[0]["content"].ToString();
            }
            else
                throw new Exception_APK($"Key [{sPathOrObjectName}] not found in module !");


            return "";
        }

        public static bool f_exists(clsAPK_Module _m, string sPath)
        {
            string _path = refine_VPath(sPath);
            DataRow[] _rows = _m.t.Select($" path = '{_path}' ");
            return (_rows.Length > 0) ? true : false;
        }

        public static string f_getType(clsAPK_Module _m, string sName)
        {
            DataRow[] _rows = _m.t.Select($"path = '{sName}' ");
            if (_rows.Length > 0)
                return _rows[0].getString("type");
            else
            {
                throw new Exception_APK("module path [{sName}] not found !");
            }
        }

        public static List<clsColMap> f_getColMap(clsAPK_Module _m
        , string sName)
        {
            string sXml = f_getStr(_m, sName);
            return clsAPK_ObjectF.createColMap(sXml);
        }


        [DebuggerStepThrough]
        public static JArray f_getJArray(clsAPK_Module _m, string sPath)
        {
            string sJSON = null;

            try
            {
                sJSON = f_getStr(_m, sPath);
                JArray _arr = JArray.Parse(sJSON);
                return _arr;
            }
            catch (exception_g ex)
            {
                throw new exception_g(ex.Message);
            }

            return null;

        }

        public static XmlDocument f_getXml(clsAPK_Module _m
                , string sPath)
        {

            string sData = f_getStr(_m, sPath);

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(sData);

            return xDoc;
        }

        [DebuggerStepThrough]
        public static string f_getStr(clsAPK_Module _m
            , string sPath
            , params object[] args)
        {
            string sData = f_getStr(_m, sPath);
            return string.Format(sData, args);
        }

    }
}
