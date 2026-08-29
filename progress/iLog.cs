using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace tcore2.progress
{
    public interface iLog
    {
        void print(string msg);

        void progressInit(string sInfo,int iCount);
        void progress(int iCount, int iCurrent);
        void progressEnd(string sInfo);

    }

    public class clsLogConsole : iLog
    {

        public int ConsoleX, ConsoleY;
        public void print(string msg)
        {
            Console.WriteLine(msg);
        }

        public void progressInit(string sInfo, int iCount)
        {
            Console.WriteLine(sInfo + " started : ");
            (int x, int y) = Console.GetCursorPosition();
            ConsoleX = x; ConsoleY = y;
        }

        public void progress(int iCount, int iCurrent)
        {
            Console.SetCursorPosition(ConsoleX, ConsoleY);
            Console.Write("    ");
            Console.SetCursorPosition(ConsoleX, ConsoleY);
            decimal iPer = (Convert.ToDecimal(iCurrent) / Convert.ToDecimal(iCount)) * 100;
            iPer = Math.Round(iPer, 2);
            Console.Write($"{iPer}%");
        }

        public void progressEnd(string sInfo)
        {

            Console.WriteLine("");
            Console.WriteLine(sInfo + " finished.........[Done] ");
            ConsoleX = 0;
            ConsoleX = 0;
        }
    }
}
