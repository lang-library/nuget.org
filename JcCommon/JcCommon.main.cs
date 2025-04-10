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
        string projFileName = @"D:\home09\api\nlohmann\main.cpp";
        CppUtil.ParseProject(projFileName);
        CppUtil.DebugDump();
        string dllPath = Installer.InstallResourceDll(
            typeof(Program).Assembly,
            "C:\\dll-dir",
            "JcCommon:add2.dll");
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
        //Echo(jc.Call("add2", EasyObject.FromObject(new object[] { 1111, 2222, 3333 })));
        //Api._wsystem("ping www.youtube.com");
        Echo(Api.RunCommand("ping", "-n", "2", "www.youtube.com"));
        string script = """
                #! /usr/bin/env js
                //+ MyClass1.dll
                echo($1+$2, "タイトル©");
                var MyApi = importNamespace('MyApi');
                echo(MyApi.MyClass1.Add2(111, 222), `MyApi.MyClass1.Add2(111, 222)`);
                """;
        JavaScript myjs = new JavaScript();
        //myjs.Init(new string[] { "MyClass1.dll" });
        myjs.InitForScript(script);
        myjs.Execute(script, 111, 222);
        CSScripting css = new CSScripting(false, null, typeof(Global.EasyObject).Assembly);
        css.Exec("""
            using static Global.EasyObject;
            ShowDetail = true;
            System.Console.WriteLine("from CSScripting");
            Echo("hello from Echo");
            """);
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
