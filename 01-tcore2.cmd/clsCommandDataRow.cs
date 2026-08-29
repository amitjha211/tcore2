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
    public class clsCommandDataRow : clsCommandBase
    {

        private DataRow _row;

        public override void setObject(object obj)
        {
            base.setObject(obj);

            if (obj is DataRowView)
            {
                _row = ((DataRowView)obj).Row;
            }
            _row = (DataRow)obj;
        }

        public clsCommandDataRow(DataRow r)
        {
            this.setObject(r);
        }

        public override bool Contains(string Key) => _row.Table.Columns.Contains(Key);

        
        public override object get(int iIndex) => _row[iIndex];


        
        public override object get(string sKey)
        {
            
            return _row[sKey];
        }

        
        public override int getCount() => _row.Table.Columns.Count;
        
        
        public override string getKey(int iCol) => _row.Table.Columns[iCol].ColumnName;

        
        public override void set(string sKey, object objValue) => _row[sKey] = objValue;

        
        public override void set(int iCol, object objValue) => _row[iCol] = objValue;
    }

}
