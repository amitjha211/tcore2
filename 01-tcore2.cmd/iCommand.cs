using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcore2
{

    public interface iCommand
    {
        public void setObject(object obj);
        public object getObject();
        public object get(int iIndex);
        public object get(string sKey);
        public void set(string sKey, object objValue);
        public void set(int iCol, object objValue);
        public int getCount();
        public string getKey(int iCol);
        public bool Contains(string Key);
    }

}
