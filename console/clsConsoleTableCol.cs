using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace tcore2.console
{
    public class clsConsoleTableCol
    {
        public string Title { get; set; }
        public string Field { get; set; }
        public int size { get; set; }
        public ConsoleColor ConsoleColor { get; set; } = ConsoleColor.White;
    }
}
