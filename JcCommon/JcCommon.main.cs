//css_inc JcCommon.cs
//css_nuget EasyObject
//*css_ref* Jc.Math.exe
using System.Collections.Generic;
using static Global.EasyObject;

namespace JcCommon;

public class Program
{
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
    }
}
