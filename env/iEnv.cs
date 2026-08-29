using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using apk.db;

namespace tcore2.env
{
    public interface iEnv
    {
        clsConnectionModel objCon { get; set; }
        clsConnectionModel objConCommon { get; set; }
        string Env { get; set; }
        string DBName { get; set; }
    }
}
