//css_inc JcCommon.cs
//css_nuget EasyObject
//*css_ref* Jc.Math.exe
using static Global.EasyObject;

namespace JcCommon;

public class Program
{
    public static void Main(string[] args)
    {
        Log(args, "args");
        Echo("helloハロー©");
        Echo(Api.Add2(11, 22));
        //Echo(Jc.Math.Api.Add2(111, 222));
    }
}
