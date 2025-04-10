//css_inc JcCommon.cs
//css_nuget EasyObject
//css_embed add2.dll
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Global;
using static Global.EasyObject;

namespace JcCommon;

public class Program
{
    delegate int proto_add2(int a, int b);
    public static void Main(string[] args)
    {
        Log(args, "args");
        Log(Api.CheckFixedArguments("dummy", 1, args));
        Echo("helloハロー©");
        string projFileName = @"D:\home09\cs-cmd\my\my.cs";
        CscsUtil.ParseProject(projFileName);
        CscsUtil.DebugDump();
        string cs_gen = Api.FindExePath("cs-gen.exe");
        Echo(cs_gen, "cs_gen");
        string dllPath = Installer.InstallResourceDll(
            typeof(Program).Assembly,
            "C:\\dll-dir",
            "JcCommon.main:add2.dll");
        Echo(dllPath, "dllPath");
        IntPtr Handle = IntPtr.Zero;
        Handle = LoadLibraryExW(
            dllPath,
            IntPtr.Zero,
            LoadLibraryFlags.LOAD_WITH_ALTERED_SEARCH_PATH
            );
        if (Handle == IntPtr.Zero)
        {
            EasyObject.Log($"DLL not loaded: {dllPath}");
            Environment.Exit(1);
        }
        CallAdd2(Handle);
    }
    private static void CallAdd2(IntPtr Handle)
    {
        IntPtr Add2Ptr = GetProcAddress(Handle, "add2");
        proto_add2 add2 = (proto_add2)Marshal.GetDelegateForFunctionPointer(Add2Ptr, typeof(proto_add2));
        Echo(add2(11, 22));
    }
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
}
