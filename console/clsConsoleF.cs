using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcore2.console;

public class clsConsoleF
{

    public static clsConsoleTableCol addConsoleTableCol(List<clsConsoleTableCol> cols, string sTitle, string sField, int size)
    {

        clsConsoleTableCol col = new clsConsoleTableCol();
        col.Title = sTitle;
        col.Field = sField;
        col.size = size;

        cols.Add(col);

        return col;
    }
    public static void printTable(DataTable dt, List<clsConsoleTableCol> cols)
    {

        //Printing Columns
        foreach (clsConsoleTableCol col in cols)
        {
            string sTmp = "{0|" + col.size + "}|";
            Console.Write(string.Format(sTmp, col.Title));
        }

        Console.Write("\n\r");

        //foreach (DataRow r in dt.Rows)
        //{
        //    foreach (clsConsoleTableCol col in cols)
        //    {
        //        string sVal = r[col.Field].ToString();
        //        string sTmp = "{0|" + col.size + "}|";
        //        Console.WriteLine(sTmp, sVal);
        //    }
        //}

        Console.WriteLine("end....");
    }
}