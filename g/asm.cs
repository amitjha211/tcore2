using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace g
{
    public static class asm
    {


        public static object asm_create_object(string sAsm, string sClassPath)
        {
            var obj = Activator.CreateInstance(sAsm, sClassPath).Unwrap();
            return obj;
        }

        public static void asm_set_value(object obj
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
        public static void asm_set_value2(object obj
            , string sPropertyName
            , object objPropertyValue)
        {

            PropertyInfo p = obj.GetType().GetProperty(sPropertyName);
            p.SetValue(obj, objPropertyValue);
        }


    }
}
