using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Global
{
    public class JsonApiClient
    {
        IntPtr Handle = IntPtr.Zero;
        IntPtr CallPtr = IntPtr.Zero;
        IntPtr FreePtr = IntPtr.Zero;
        delegate IntPtr proto_Call(IntPtr pName, IntPtr pArgs);
        delegate void proto_Free(IntPtr pResult);
        public JsonApiClient(string dllSpec)
        {
            string dllPath = Sys.FindExePath(dllSpec);
            if (dllPath is null)
            {
                EasyObject.Log(dllSpec, "dllSpec");
                EasyObject.Log(dllPath, "dllPath");
                Environment.Exit(1);
            }
            this.LoadDll(dllPath);
        }
        public JsonApiClient(string dllSpec, string cwd)
        {
            string dllPath = Sys.FindExePath(dllSpec, cwd);
            if (dllPath is null)
            {
                EasyObject.Log(dllSpec, "dllSpec");
                EasyObject.Log(dllPath, "dllPath");
                Environment.Exit(1);
            }
            this.LoadDll(dllPath);
        }
        public JsonApiClient(string dllSpec, Assembly assembly)
        {
            string dllPath = Sys.FindExePath(dllSpec, assembly);
            if (dllPath is null)
            {
                EasyObject.Log(dllSpec, "dllSpec");
                EasyObject.Log(dllPath, "dllPath");
                Environment.Exit(1);
            }
            this.LoadDll(dllPath);
        }
        private void LoadDll(string dllPath)
        {
            this.Handle = Sys.LoadLibraryExW(
                dllPath,
                IntPtr.Zero,
                Sys.LoadLibraryFlags.LOAD_WITH_ALTERED_SEARCH_PATH
                );
            if (this.Handle == IntPtr.Zero)
            {
                EasyObject.Log($"DLL not loaded: {dllPath}");
                Environment.Exit(1);
            }
            this.CallPtr = Sys.GetProcAddress(Handle, "Call");
            if (this.CallPtr == IntPtr.Zero)
            {
                EasyObject.Log("Call() not found");
                Environment.Exit(1);
            }
            this.FreePtr = Sys.GetProcAddress(Handle, "Free");
            if (this.FreePtr == IntPtr.Zero)
            {
                EasyObject.Log("Free() not found");
                Environment.Exit(1);
            }
        }
        public EasyObject Call(string name, EasyObject args)
        {
            proto_Call pCall = (proto_Call)Marshal.GetDelegateForFunctionPointer(this.CallPtr, typeof(proto_Call));
            proto_Free pFree = (proto_Free)Marshal.GetDelegateForFunctionPointer(this.FreePtr, typeof(proto_Free));
            IntPtr pName = Sys.StringToUTF8Addr(name);
            var argsJson = args.ToJson();
            IntPtr pArgsJson = Sys.StringToUTF8Addr(argsJson);
            IntPtr pResult = pCall(pName, pArgsJson);
            string result = Sys.UTF8AddrToString(pResult);
            pFree(pResult);
            Sys.FreeHGlobal(pName);
            Sys.FreeHGlobal(pArgsJson);
            result = result.Trim();
            if (result.StartsWith("\""))
            {
                string error = EasyObject.FromJson(result).Cast<string>();
                throw new Exception(error);
            }
            else if (result.StartsWith("["))
            {
                var list = EasyObject.FromJson(result);
                if (list.Count == 0) return EasyObject.FromObject(null);
                return list[0];
            }
            else
            {
                string error = $"Malformed result json: {result}";
                throw new Exception(error);
            }
        }
    }
}
