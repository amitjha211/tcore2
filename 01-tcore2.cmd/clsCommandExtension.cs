using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace tcore2
{
    [DebuggerStepThrough]
    public static class clsCommandExtension
    {
        public static void copyFrom(this iCommand rDest, iCommand rSource)
        {
            //DataTable tSource = rSource.Table;
            //DataTable tDest = r.Table;

            ///////////////////////////////////////////////

            int iCount = rDest.getCount();

            for (int iCol = 0; iCol < iCount; iCol++)
            {

                string sColumnName = rDest.getKey(iCol);

                if (rSource.Contains(sColumnName))
                {
                    object objValue = rSource.get(sColumnName);
                    rDest.set(sColumnName, objValue);
                }
            }
        }


        public static bool isDBNullOrNull(this iCommand r, string sKey)
        {
            object objValue = r.get(sKey);
            return objValue == null || objValue == DBNull.Value ? true : false;
        }

        public static int getInt(this iCommand r, int iCol)
        {
            int iVal = 0;

            string sVal = r.get(iCol).ToString();


            int.TryParse(sVal, out iVal);


            return iVal;

        }

        
        public static int getInt(this iCommand r, string sField)
        {
            int iVal = 0;

            string sVal = r.get(sField).ToString();


            int.TryParse(sVal, out iVal);

            return iVal;
        }

        
        public static string getString(this iCommand r, string sField)
        {
            try
            {
                return r.get(sField).ToString();
            }
            catch (Exception ex) {
                throw new Exception_Core(ex.Message);
            }
        }

        
        public static string getString(this iCommand r, int iCol)
        {
            return r.get(iCol).ToString();
        }



        
        public static decimal getDecimal(this iCommand r, int iCol)
        {
            decimal iVal = 0;

            string sVal = r.get(iCol).ToString();


            decimal.TryParse(sVal, out iVal);

            return iVal;
        }

        
        public static decimal getDecimal(this iCommand r, string sField)
        {
            decimal iVal = 0;
            string sVal = "";

            try
            {
                sVal = r.get(sField).ToString();
            }
            catch (Exception ex)
            { 
                throw ;
            }
            
            decimal.TryParse(sVal, out iVal);

            return iVal;
        }

    }
}
