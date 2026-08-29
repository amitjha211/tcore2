using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace tcore2.global
{
    public partial class clsGlobal
    {
        public static object shell(params string[] args)
        {

            try
            {

                string sMethod = args[0];

                object objShell = Assembly.GetEntryAssembly().CreateInstance($"{Assembly.GetEntryAssembly().GetName().Name}.clsShell");
                Type objType = objShell.GetType();
                List<object> lstArgs = new List<object>();

                MethodInfo m = objType.GetMethod(sMethod);
                ParameterInfo[] _params = m.GetParameters();

                for (int i = 0; i < _params.Length; i++)
                {
                    ParameterInfo p = _params[0];
                    if (p.ParameterType == typeof(string))
                    {
                        lstArgs.Add(args[i + 1]);
                    }
                    else if (p.ParameterType == typeof(int))
                    {
                        lstArgs.Add(Convert.ToInt32(args[i + 1]));
                    }
                }

                return m.Invoke(objShell, lstArgs.ToArray());
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
                Console.Write("Error has occured, Do you want to repeat ? Y/N ");
                char readKey = Console.ReadKey().KeyChar;
                Console.WriteLine("");
                if (readKey == 'Y' || readKey == 'y')
                {
                    shell(args);
                }
            }

            return null;
        }
    }
}
