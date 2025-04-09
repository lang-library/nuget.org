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
        Echo("helloハロー©");
        Echo(Math.Add2(111, 222));
        List<string> lines = Api.TextToLines("""
            a
            b
            c
            """);
        Echo(lines, "lines");
        Echo(Api.FreeTcpPort());
    }
}
