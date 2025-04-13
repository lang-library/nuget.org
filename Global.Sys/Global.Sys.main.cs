//css_inc Global.Sys.cs
//css_nuget EasyObject
namespace Global;

using static Global.EasyObject;

public class Program
{
    public static void Main(string[] args)
    {
        Log(args, "args");
        Echo("helloハロー©");
        Echo(Sys.Add2(11, 22));
    }
}
