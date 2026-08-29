using apk;
using g.progress;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace g.cli;
public class cli_model_base
{
    public string _pwd = "@/";

    public progress_model p = null;
    public clsAPK apk = null;
    public Type _type = null;

    public clsCommand cmd = new clsCommand();

    public object obj = null;

    public List<MethodInfo> _methods = new List<MethodInfo>();


}
