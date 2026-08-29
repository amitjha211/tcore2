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

namespace apk
{
    public static partial class clsAPKF_FS_F
    {

        public static string f_getFullOSPath(clsAPK _apk, string sVPath)
        {

            string _root = "";
            string _vpath = "";
            string _fullPath = "";

            if (sVPath.StartsWith("@/common"))
            {
                _root = _apk.appFolder + "/" + "common";
                _vpath = sVPath.Substring("@/common".Length);
            }
            else if (sVPath.StartsWith("@/"))
            {
                _root = _apk.appFolder + "/" + _apk.appName + "/";
                _vpath = sVPath.Substring(2);
            }
            else
            {
                _root = "";
            }

            return _root + _vpath;
        }

        
        public static List<clsColMap> f_getColMap(clsAPK _apk, string sFile)
        {
            string sXml = f_getStr(_apk, sFile);
            return clsAPK_ObjectF.createColMap(sXml);
        }

        public static JArray f_getJArray(clsAPK mAPK, string sPath)
        {

            string sJSON = f_getStr(mAPK, sPath);
            JArray _arr = JArray.Parse(sJSON);
            return _arr;
        }


        public static XmlDocument f_getXml(clsAPK mAPK
        , string sPath)
        {

            string _Path = f_getFullOSPath(mAPK, sPath);
            string sData = File.ReadAllText(_Path);

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(sData);

            return xDoc;
        }

        public static XmlDocument f_getXml(clsAPK _apk
            , XmlNode xNode)
        {

            string sXml = f_getStr(_apk, xNode);
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(sXml);

            return xDoc;
        }

        [DebuggerStepThrough]
        public static string f_getStr(clsAPK mAPK
            , string sPath
            , params object[] args)
        {

            string sData = f_getStr(mAPK, sPath);
            return string.Format(sData, args);
        }

        

        [DebuggerStepThrough]
        public static byte[] f_getBytes(clsAPK _apk
            , string sPath)
        {
            if (!f_exists(_apk, sPath))
                throw new Exception($"File [{sPath}] not found !");

            string _Path = f_getFullOSPath(_apk, sPath);
            byte[] bData = File.ReadAllBytes(_Path);

            return bData;
        }


        //[DebuggerStepThrough]
        public static string f_getStr(clsAPK _apk
            , string sPath)
        {
            
            if (!f_exists(_apk, sPath))
                throw new Exception_APK($"File [{sPath}] not found !");

            string _Path = f_getFullOSPath(_apk, sPath);
            string sData = File.ReadAllText(_Path);
            return sData;
        }


        [DebuggerStepThrough]
        public static string f_getStr(clsAPK _apk, XmlNode xNode)
        {
            string sVPath = xNode.getXmlAttributeValue("src");

            string sData = "";

            if (!string.IsNullOrWhiteSpace(sVPath))
            {
                try
                {
                    sData = f_getStr(_apk, sVPath);
                }
                catch (Exception_APK ex)
                {
                    throw ex;
                }
            }
            else
            {
                sData = xNode.InnerXml;
            }

            return sData;
        }

        internal static string refine_VPath(string sPathOrObjectName)
        {
            if (sPathOrObjectName.StartsWith("@"))
                return sPathOrObjectName;

            string sPath = "";

            if (sPathOrObjectName.Contains("/"))
            {
                return sPathOrObjectName;
            }
            else
            {
                return $"main/{sPathOrObjectName}";
            }
            
            return "";
        }




        
        public static bool f_exists(clsAPK mAPK
        , string sPath)
        {

            string _Path = f_getFullOSPath(mAPK, sPath);
            return System.IO.File.Exists(_Path);
        }

        public static string[] f_getListFiles(clsAPK _apk, string sVPathDirectory)
        {
            string sOSPath_Directory = f_getFullOSPath(_apk, sVPathDirectory);

            string[] lstOSFiles = System.IO.Directory.GetFiles(sOSPath_Directory);
            List<string> lstReturn = new List<string>();

            foreach (string sFile in lstOSFiles)
            {
                lstReturn.Add(Path.GetFileName(sFile));
            }
            
            return lstReturn.ToArray();
        }

        public static string[] f_getListDirectories(clsAPK _apk, string sVPathDirectory)
        {
            string sOSPath_Directory = f_getFullOSPath(_apk, sVPathDirectory);

            string[] lstOSFiles = System.IO.Directory.GetDirectories(sOSPath_Directory);
            List<string> lstReturn = new List<string>();

            foreach (string sFile in lstOSFiles)
            {
                lstReturn.Add(Path.GetFileName(sFile));
            }

            return lstReturn.ToArray();
        }

    }
}
