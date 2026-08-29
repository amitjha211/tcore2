using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static tcore2.global.clsGlobal;
using apk;
using System.Reflection;
using System.Reflection.Metadata;
namespace g.cli;

public class cliF
{

    public static bool collectArg(MethodInfo _m, cli_model_base _cli, List<object> objArgs)
    {
        objArgs.Add(_cli);


        ParameterInfo[] _params = _m.GetParameters().ToArray();

        for (int i = 1; i < _params.Length; i++)
        {
            ParameterInfo _p = _params[i];

            Console.Write($"{_p.Name} : ");
            string sValue = Console.ReadLine();
            object objValue = null;


            if (_p.ParameterType == typeof(string)) objValue = sValue;
            if (_p.ParameterType == typeof(int)) objValue = Convert.ToInt32(sValue);
            if (_p.ParameterType == typeof(decimal)) objValue = parseDecimal(sValue);

            if (objValue == null) throw new exception_g($"Unable to parse value [{sValue}] for param [{_p.Name} | {_p.ParameterType.FullName}]");

            objArgs.Add(objValue);
        }

        return true;
    }


    public static void cli_addCommands(cli_model_base _cli, Type _type)
    {

       MethodInfo[] _methods = _type.GetMethods()
      .Where(m => m.IsDefined(typeof(cli_attribute), inherit: true))
      .ToArray();


        _cli._methods.AddRange(_methods);
    }

    public static void cli_loop_command(cli_model_base _cli)
    {
        string sKey = "";

        do
        {

            Console.Write($">>");
            sKey = Console.ReadLine();

            //MethodInfo _method = _type.GetMethod(sKey);
            MethodInfo _method = _cli._methods.Find(p => p.Name == sKey);
            if (_method == null)
            {
                Console.WriteLine("Method not found !");
            }
            else
            {
                List<object> _lstArgs = new List<object>();

                try
                {
                    collectArg(_method,_cli, _lstArgs);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Argument Parsing failed : " + ex.ToString());
                    Console.ResetColor();

                    continue;
                }

                _method.Invoke(null, _lstArgs.ToArray());
            }
        } while (sKey != "exit" && sKey != "q");
    }
}
