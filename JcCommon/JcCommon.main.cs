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
    delegate IntPtr proto_greeting(IntPtr name);
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
        Handle = Api.LoadLibraryExW(
            dllPath,
            IntPtr.Zero,
            Api.LoadLibraryFlags.LOAD_WITH_ALTERED_SEARCH_PATH
            );
        if (Handle == IntPtr.Zero)
        {
            EasyObject.Log($"DLL not loaded: {dllPath}");
            Environment.Exit(1);
        }
        CallAdd2(Handle);
        CallGreeting(Handle);
        JsonClient jc = new JsonClient("cs-api.dll");
        Echo(jc.Call("add2", EasyObject.FromObject(new object[] { 1111, 2222 })));
    }
    private static void CallAdd2(IntPtr Handle)
    {
        IntPtr Add2Ptr = Api.GetProcAddress(Handle, "add2");
        proto_add2 add2 = (proto_add2)Marshal.GetDelegateForFunctionPointer(Add2Ptr, typeof(proto_add2));
        Echo(add2(11, 22));
    }
    private static void CallGreeting(IntPtr Handle)
    {
        IntPtr GreetingPtr = Api.GetProcAddress(Handle, "greeting");
        proto_greeting greeting = (proto_greeting)Marshal.GetDelegateForFunctionPointer(GreetingPtr, typeof(proto_greeting));
        IntPtr namePtr = Api.StringToUTF8Addr("トム©");
        IntPtr result = greeting(namePtr);
        Api.FreeHGlobal(namePtr);
        Echo(Api.UTF8AddrToString(result));
    }
}
