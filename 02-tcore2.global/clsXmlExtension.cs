using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace tcore2.global
{
    public static class clsXmlExtension
    {

        public static string getXmlText(this System.Xml.XmlNode node, string xPath = "")
        {
            var node1 = string.IsNullOrWhiteSpace(xPath) ? node : node.SelectSingleNode(xPath);

            if (node1 == null)
                return "";
            else
                return node1.InnerXml;
        }

        public static string getXmlAttributeValue(this System.Xml.XmlNode node, string sAttriName)
        {
            if (node == null) return "";
            if (node.Attributes[sAttriName] == null) return "";
            return node.Attributes[sAttriName].InnerText;
        }

        public static bool hasAttribute(this System.Xml.XmlNode node, string sAttriName)
        {
            if (node == null) return false;
            if (node.Attributes[sAttriName] == null) return false;

            XmlAttribute objAttri = node.Attributes[sAttriName];

            if(objAttri != null) 
                return true;

            return false;
        }


    }
}
