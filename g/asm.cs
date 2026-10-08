using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace g
{
    public static class asmF
    {

        public static object asm_create_object(string sAsm, string sClassPath)
        {
            var obj = Activator.CreateInstance(sAsm, sClassPath).Unwrap();
            return obj;
        }
        public static object asm_create_object(string _asmString)
        {
            string asmName = "";
            string asmClassPath = "";

            string[] sSplitted = _asmString.Split(';');
            if (sSplitted.Length == 2)
            {
                asmName = sSplitted[0];
                asmClassPath = sSplitted[1];
            }

            return asm_create_object(asmName, asmClassPath);
        }

        public static void asm_run_static_method(string sMethodInfo, params object[] args)
        {
            MethodInfo _method = asm_create_method(sMethodInfo);
            _method.Invoke(null, args);
        }


        public static void asm_run_method(object obj, string sMethodName, params object[] args)
        {
            MethodInfo _method = obj.GetType().GetMethod(sMethodName);
            _method.Invoke(obj, args);
        }
        public static void asm_run_method(object obj,MethodInfo _m, params object[] args)
        {
            _m.Invoke(obj, args);
        }


        public static MethodInfo asm_create_method(string _asmString)
        {
            string asmName = "";
            string asmClassPath = "";
            string methodName = "";

            string[] sSplitted = _asmString.Split(';');
            if (sSplitted.Length == 3)
            {
                asmName = sSplitted[0];
                asmClassPath = sSplitted[1];
                methodName = sSplitted[2];
            }

            Assembly _asm = System.AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(p => p.FullName.StartsWith(asmName));
            Type _t = _asm.GetType(asmClassPath);
            MethodInfo _m = _t.GetMethod(methodName);

            return _m;
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
