using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace tcore2.asm
{
    public class clsASMType
    {

        public string Tag = "";
        public string assemblyName = "";
        public string classPath = "";
    }

    public class clsASM_F
    {

        public static object createObject(string sAsm, string sClassPath)
        {
            var obj = Activator.CreateInstance(sAsm, sClassPath).Unwrap();
            
            return obj;
        }
        public static void applyProperty(object obj
            , string sPropertyName
            , string sPropertyValue)
        {

            PropertyInfo p = obj.GetType().GetProperty(sPropertyName);

            if (p.PropertyType == typeof(string))
            {
                p.SetValue(obj, sPropertyValue);
            }
            else if (p.PropertyType == typeof(int))
            {
                int iValue = Convert.ToInt32(sPropertyValue);
                p.SetValue(obj, iValue);
            }
        }
        public static void applyProperty2(object obj
            , string sPropertyName
            , object objPropertyValue)
        {

            PropertyInfo p = obj.GetType().GetProperty(sPropertyName);
            p.SetValue(obj, objPropertyValue);
        }
    }
}
