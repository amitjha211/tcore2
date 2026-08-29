using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace g.cli
{
    

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class cli_attribute : Attribute
    {
        public string commandName { get; set; } = "";

        
    }
}
