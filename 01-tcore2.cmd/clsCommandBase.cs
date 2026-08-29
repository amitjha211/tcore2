using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;


namespace tcore2
{
    [DebuggerStepThrough]
    public abstract class clsCommandBase : iCommand
    {

        private object _obj = null;

        public object getObject()
        {
            return _obj;
        }

        public virtual void setObject(object obj)
        {
            _obj = obj;
        }
        public abstract bool Contains(string Key);
        public abstract object get(int iIndex);
        public abstract object get(string sKey);
        public abstract int getCount() ;
        public abstract string getKey(int iCol);
        public abstract void set(string sKey, object objValue);
        public abstract void set(int iCol, object objValue);

        public object this[string sKey]
        {
            get {
                return get(sKey);
            }
            set { 
                set(sKey, value);
            }
        }
        public object this[int iKey]
        {
            get
            {
                return get(iKey);
            }
            set
            {
                set(iKey, value);
            }
        }
    }
}
