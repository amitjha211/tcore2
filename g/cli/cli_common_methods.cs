using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using static apk.clsAPKF_FS_F;
namespace g.cli;

public class cli_common_methods
{

    [cli_attribute]
    public static void ls(cli_model_base _cli)
    {

        string[] lst = f_getListFiles(_cli.apk, _cli._pwd);
        foreach (string sFile in lst)
        {
            Console.WriteLine($"[f] : {sFile} ");
        }

        string[] lst_dir = f_getListDirectories(_cli.apk, _cli._pwd);
        foreach (string sDir in lst_dir)
        {
            Console.WriteLine($"[D] : [{sDir}]");
        }
    }


    [cli_attribute]
    public static void pwd(cli_model_base _cli)
    {
        Console.WriteLine(_cli._pwd);
    }

    [cli_attribute]
    public static void clear(cli_model_base _cli)
    {
        Console.Clear();
    }

    [cli_attribute]
    public static void cd(cli_model_base _cli, string sFolder)
    {
        if (!string.IsNullOrWhiteSpace(sFolder)) _cli._pwd += "/" + sFolder;
    }


    [cli_attribute]
    public static void lscmd(cli_model_base _cli)
    {
        //foreach(Meth _cli._methods
        foreach(MethodInfo _m  in _cli._methods)
        {
            Console.WriteLine($"{_m.ReflectedType.Name} | {_m.Name} ");
        }
    }



}
