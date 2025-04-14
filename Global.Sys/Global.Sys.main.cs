//css_inc Global.Sys.cs
//css_nuget EasyObject
//css_embed add2.dll
using System;
using System.Runtime.InteropServices;
using static Global.EasyObject;

namespace Global
{
    public class Program
    {
        delegate int proto_add2(int a, int b);
        delegate IntPtr proto_greeting(IntPtr name);
        public static void Main(string[] args)
        {
            Log(args, "args");
            Log(Sys.CheckFixedArguments("dummy", 1, args));
            Echo("helloハロー©");
            //string projFileName = @"D:\home09\api\nlohmann\main.cpp";
            string dllPath = Installer.InstallResourceDll(
                typeof(Program).Assembly,
                "C:\\dll-dir",
                "Global.Sys:add2.dll");
            Echo(dllPath, "dllPath");
            IntPtr Handle = IntPtr.Zero;
            Handle = Sys.LoadLibraryExW(
                dllPath,
                IntPtr.Zero,
                Sys.LoadLibraryFlags.LOAD_WITH_ALTERED_SEARCH_PATH
                );
            if (Handle == IntPtr.Zero)
            {
                EasyObject.Log($"DLL not loaded: {dllPath}");
                Environment.Exit(1);
            }
            CallAdd2(Handle);
            CallGreeting(Handle);
            //Echo(Sys.RunCommand("ping", "-n", "2", "www.youtube.com"));

            string asmPath = Sys.FindExePath("PROGRAM.dll");
            System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFrom(asmPath);
            object methodResult = Sys.CallAssemblyStaticMethod(asm, "PROGRAM.Api", "Add2", 777, 1);
            Echo(methodResult, "methodResult");
            asm = Sys.AssemblyForTypeName("Global.Program");
            Echo(asm == null);
            methodResult = Sys.CallAssemblyStaticMethod(asm, "Global.Program", "Add3", 11, 22, 33);
            Echo(methodResult, "methodResult");
        }
        public static int Add3(int a, int b, int c)
        {
            return a + b + c;
        }
        private static void CallAdd2(IntPtr Handle)
        {
            IntPtr Add2Ptr = Sys.GetProcAddress(Handle, "add2");
            proto_add2 add2 = (proto_add2)Marshal.GetDelegateForFunctionPointer(Add2Ptr, typeof(proto_add2));
            Echo(add2(11, 22));
        }
        private static void CallGreeting(IntPtr Handle)
        {
            IntPtr GreetingPtr = Sys.GetProcAddress(Handle, "greeting");
            proto_greeting greeting = (proto_greeting)Marshal.GetDelegateForFunctionPointer(GreetingPtr, typeof(proto_greeting));
            IntPtr namePtr = Sys.StringToUTF8Addr("トム©");
            IntPtr result = greeting(namePtr);
            Sys.FreeHGlobal(namePtr);
            Echo(Sys.UTF8AddrToString(result));
        }
    }
}
