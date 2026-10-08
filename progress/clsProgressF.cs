using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcore2.progress
{
    public class clsProgressF
    {

        public static clsProgressModel createAndInit(string sLabel
            , string sName
            , int iCount
            , Action<clsProgressModel> pEvent)
        {

            clsProgressModel p = new clsProgressModel();
            p.progressLabel = sLabel;
            p.progressName = sName;
            p.iCount = iCount;
            clsProgressF.init(p, pEvent);

            return p;

        }


        public static void init(clsProgressModel p
            , Action<clsProgressModel> displayInfo)
        {
            p.action = "init";
            if (displayInfo != null) displayInfo(p);
        }

        public static void progress(clsProgressModel p
            , int _currentRow
            , Action<clsProgressModel> displayInfo)
        {
            p.action = "progress";
            p.iCurrentRow = _currentRow;
            if (displayInfo != null) displayInfo(p);
        }


        public static void progress(clsProgressModel p
            , Action<clsProgressModel> displayInfo)
        {
            p.action = "progress";
            if (displayInfo != null) displayInfo(p);
        }
        public static void end(clsProgressModel p
           , Action<clsProgressModel> displayInfo)
        {
            p.action = "end";
            if (displayInfo != null) displayInfo(p);

            p.action = "";
            p.progressLabel = "";
            p.progressName = "";
            p.iCurrentRow = 0;
            p.iPer = 0;
            p.iCount = 0;
        }

        public static decimal getPercentage(clsProgressModel p)
        {
            decimal iPer = (Convert.ToDecimal(p.iCurrentRow) / Convert.ToDecimal(p.iCount)) * 100;
            return Math.Round(iPer);
        }
        public static string getInfo(clsProgressModel p)
        {
            switch (p.action)
            {
                case "init":
                    return $"{p.progressLabel} has initialized.";
                case "progress":
                    return $"{p.progressLabel} {getPercentage(p)}";
                case "end":
                    return $"{p.progressLabel} has finished. ";
                default:
                    return "Unknown Action";
            }
        }
    }
}
