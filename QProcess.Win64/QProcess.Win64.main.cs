//css_inc QProcess.Win64.cs
//css_nuget EasyObject
//*css_ref* Jc.Math.exe
using static Global.EasyObject;

namespace QProcess.Win64;

public class Program
{
    public static void Main(string[] args)
    {
        Log(args, "args");
        Echo("helloハロー©");
        Echo(Math.Add2(11, 22));
    }
}
