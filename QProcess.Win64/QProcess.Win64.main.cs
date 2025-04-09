//css_inc QProcess.Win64.cs
//css_nuget EasyObject
//*css_ref* Jc.Math.exe
using static Global.EasyObject;

namespace QProcess;

public class Program
{
    public static void Main(string[] args)
    {
        Echo("helloハロー©");
        Log(args, "args");
        //QProcess_Win64.Execute3("ping.exe", "-n\t2\twww.youtube.com", "");
        QProcess.Win64.Execute("ping.exe", new string[] { "-n", "2", "www.youtube.com" });
    }
}
