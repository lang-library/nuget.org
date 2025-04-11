//css_inc JcCSharp.cs
//css_nuget EasyObject
namespace JcCSharp;

using JcCommon;
using static Global.EasyObject;

public class Program
{
    public static void Main(string[] args)
    {
        Log(args, "args");
        Echo("helloハロー©");
        CSScripting css = new CSScripting(false, null, typeof(Global.EasyObject).Assembly);
        css.Exec("""
            using static Global.EasyObject;
            ShowDetail = true;
            System.Console.WriteLine("from CSScripting");
            Echo("hello from Echo");
            """);
    }
}
