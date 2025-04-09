//css_inc JcCommon.Math.cs
//css_nuget EasyObject
using static Global.EasyObject;

namespace JcCommon;

public static class Api
{
    public static int Add2(int a, int b)
    {
        Echo(a, "a");
        Echo(b, "b");
        return a + b;
    }
}
