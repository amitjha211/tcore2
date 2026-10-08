using g;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcore2
{

    [DebuggerStepThrough]
    public class clsCommandJObject : clsCommandBase
    {

        private JObject _row;
        public override void setObject(object obj)
        {
            base.setObject(obj);
            _row = (JObject)obj;
        }

        public clsCommandJObject(JObject r)
        {
            this.setObject(r);
        }

        public override bool Contains(string Key) => _row.ContainsKey(Key);

        
        public override object get(int iIndex) => _row[iIndex];


        
        public override object get(string sKey)
        {
            
            return _row[sKey];
        }

        
        public override int getCount() => _row.Count;
        

        public override string getKey(int iCol) => throw new exception_g("not applicable");
        public override void set(int iCol, object objValue) => throw new exception_g("not applicable");

        public override void set(string sKey, object objValue) => _row[sKey] = new JValue(objValue);
        
    }

}
