using System;
using System.Reflection;
using System.Threading;
using static Global.EasyObject;

namespace Global;
public class JsonApiServer
{
    public JsonApiServer()
    {
    }
    public IntPtr HandleNativeCall(Type apiType, IntPtr nameAddr, IntPtr inputAddr)
    {
        var name = Sys.UTF8AddrToString(nameAddr);
        var input = Sys.UTF8AddrToString(inputAddr);
        EasyObject args = FromJson(input);
        MethodInfo mi = apiType.GetMethod(name);
        object result = null;
        if (mi == null)
        {
            result = $"API not found: {name}";
        }
        else
        {
            try
            {
                result = mi.Invoke(null, new object[] { args });
                var okResult = new object[] { FromObject(result).ToObject() };
                result = okResult;
            }
            catch (TargetInvocationException ex)
            {
                result = ex.InnerException.ToString();
            }
        }
        string output = FromObject(result).ToJson();
        return Sys.StringToUTF8Addr(output);
    }
    public void HandleNativeFree(IntPtr resultAddr)
    {
        Sys.FreeHGlobal(resultAddr);
    }
    public string HandleDotNetCall(Type apiType, string name, string input)
    {
        EasyObject args = FromJson(input);
        MethodInfo mi = apiType.GetMethod(name);
        dynamic result = null;
        if (mi == null)
        {
            result = $"API not found: {name}";
        }
        else
        {
            try
            {
                result = mi.Invoke(null, new object[] { args });
                var okResult = new object[] { FromObject(result).ToObject() };
                result = okResult;
            }
            catch (TargetInvocationException ex)
            {
                result = ex.InnerException.ToString();
            }
        }
        string output = FromObject(result).ToJson();
        return output;
    }
}
