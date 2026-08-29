using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using tcore2.global;
using apk.sql;
using apk.dts;

namespace apk
{
    public class clsAPK_ObjectF
    {
        public static clsSQLModel createSQLModel(clsAPK _apk, string q)
        {

            clsSQLModel m = new clsSQLModel();
            string[] sQueries = q.Split('\n', '\r');

            StringBuilder sbTmp = new StringBuilder();

            foreach (string sQuery in sQueries)
            {
                string sKey = sQuery.Trim().ToLower();

                switch (sKey)
                {
                    case "--select":
                        m.selectQuery.AppendLine(sbTmp.ToString());
                        sbTmp.Clear();
                        break;
                    case "--where":
                        m._where.AppendLine(sbTmp.ToString());
                        sbTmp.Clear();
                        break;
                    case "--orderby":
                        m._orderBy.AppendLine(sbTmp.ToString());
                        sbTmp.Clear();
                        break;
                    case "--groupby":
                        m._groupBy.AppendLine(sbTmp.ToString());
                        sbTmp.Clear();
                        break;
                    default:
                        sbTmp.AppendLine(sQuery);
                        break;
                }
            }
            return m;
        }
        public static clsCommand createCmd(string sXml)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(sXml);
            XmlNodeList xNodeList = xDoc.SelectNodes("//cmd/param");

            clsCommand cmd = new clsCommand();

            foreach (XmlNode xNode in xNodeList)
            {
                string sKey = xNode.getXmlAttributeValue("key");
                string sValue = xNode.InnerXml;
                cmd.set(sKey, sValue);
            }

            return cmd;

        }

        public static object createAssemblyObject(string _asm)
        {
            string asmName = "";
            string asmClassPath = "";

            string[] sSplitted = _asm.Split(';');
            if (sSplitted.Length == 2)
            {
                asmName = sSplitted[0];
                asmClassPath = sSplitted[1];
            }

            return createAssemblyObject(asmName, asmClassPath);

        }
        public static List<clsColMap> createColMap(string sXml)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(sXml);

            XmlNodeList _list = xDoc.SelectNodes("//col-map/cols/col");
            List<clsColMap> _lst = new List<clsColMap>();

            foreach (XmlNode _nodeCol in _list)
            {
                string sSource = _nodeCol.getXmlAttributeValue("source");
                string sDest = _nodeCol.getXmlAttributeValue("dest");
                string sValue = _nodeCol.getXmlAttributeValue("value");

                clsColMap _map = new clsColMap();
                _map.Source = sSource;
                _map.Dest = sDest;
                _map.defaultValue = sValue;

                _lst.Add(_map);
            }

            return _lst;
        }

        public static object createAssemblyObject(string asmName, string asmClassPath)
        {
            object _obj = Activator.CreateInstance(asmName, asmClassPath).Unwrap() as object;
            return _obj;
        }

    }
}
