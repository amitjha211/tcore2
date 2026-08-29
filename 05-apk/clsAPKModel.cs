using apk.table;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using tcore2.global;

namespace apk;


public class clsAPK
{
    public string appFolder = "";
    public string appName = "";

    public Action<clsTableEvent> onTableDML = null;
    internal clsCommand env = new clsCommand();
}

public class clsAPK_Module
{
    public object envObjects = null;
    public clsAPK _apk;
    public XmlNode xDoc = null;

    public string VPath = "";
    public string AbsPath = "";

    public Action<string, clsAPK_Module_Event> e = null;

    public DataTable t = new DataTable();
    internal clsCommand env = new clsCommand();
}
public class clsAPK_Module_Event
{
    public clsAPK_Module _m;
    public object obj;
}


