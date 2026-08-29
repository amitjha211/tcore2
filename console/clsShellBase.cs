using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace tcore2.console
{

    public class clsShellBase
    {
        public void ls()
        {

            MethodInfo[] lst = this.GetType().GetMethods();
            string[] args = new string[] { "GetType", "ToString", "Equals", "GetHashCode" };

            foreach (MethodInfo m in lst)
            {
                if (!args.Contains(m.Name))
                    Console.WriteLine(m.Name);
            }

            return;
        }

        public void cwd()
        {
            Console.WriteLine($"Base Direcctory : {AppDomain.CurrentDomain.BaseDirectory}");
            Console.WriteLine($"Current Directory : {Directory.GetCurrentDirectory()}");
        }

    }




}
