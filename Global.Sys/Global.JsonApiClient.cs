using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Global
{
    public class JsonApiClient
    {
        IntPtr Handle = IntPtr.Zero;
        IntPtr CallPtr = IntPtr.Zero;
        delegate IntPtr proto_Call(IntPtr name, IntPtr args);
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
        }
        public EasyObject Call(string name, EasyObject args)
        {
            IntPtr pName = Sys.StringToUTF8Addr(name);
            proto_Call pCall = (proto_Call)Marshal.GetDelegateForFunctionPointer(this.CallPtr, typeof(proto_Call));
            var argsJson = args.ToJson();
            IntPtr pArgsJson = Sys.StringToUTF8Addr(argsJson);
            IntPtr pResult = pCall(pName, pArgsJson);
            string result = Sys.UTF8AddrToString(pResult);
            result = result.Trim();
            Marshal.FreeHGlobal(pName);
            Marshal.FreeHGlobal(pArgsJson);
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
#if false
    [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern IntPtr LoadLibraryW(string lpFileName);
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern IntPtr LoadLibraryExW(string dllToLoad, IntPtr hFile, LoadLibraryFlags flags);
    [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = false)]
    internal static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
    [System.Flags]
    public enum LoadLibraryFlags : uint
    {
        DONT_RESOLVE_DLL_REFERENCES = 0x00000001,
        LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010,
        LOAD_LIBRARY_AS_DATAFILE = 0x00000002,
        LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 0x00000040,
        LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x00000020,
        LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008,
        LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR = 0x00000100,
        LOAD_LIBRARY_SEARCH_SYSTEM32 = 0x00000800,
        LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000
    }
#endif
    }
}
