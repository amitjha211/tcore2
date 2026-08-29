using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcore2;

namespace System;

[DebuggerStepThrough]
public class clsCommand : clsCommandBase
{
    public List<clsParam> prm = null;
    public string sql { get; set; }
    public clsCommand()
    {
        base.setObject(new List<clsParam>());
        prm = (List<clsParam>)getObject();
    }

    public override bool Contains(string Key)
    {
        var obj = prm.Find(p => p.name == Key);
        return obj == null ? false : true;
    }
    
    public override object get(int iIndex) => prm[iIndex].value;


    
    public override object get(string sKey)
    {
        clsParam p = prm.FirstOrDefault(p => p.name == sKey);
        if (p == null) throw new Exception_Core($"Key [{sKey}] not found !");
        return p.value;
    }
    public override int getCount() => prm.Count;
    public override string getKey(int iCol) => prm[iCol].name;

    public override void set(string sKey, object objValue)
    {

        clsParam p = prm.FirstOrDefault(p => p.name == sKey);
        if (p == null)
        {
            p = new clsParam();
            p.name = sKey;
            prm.Add(p);
        }

        p.value = objValue;

    }

    public override void set(int iCol, object objValue) => prm[iCol].value = objValue;

}
