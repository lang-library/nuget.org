//css_nuget EasyObject
using static Global.EasyObject;

namespace QProcess.Win64;

public static class Math
{
    public static int Add2(int a, int b)
    {
        Echo(a, "a");
        Echo(b, "b");
        return a + b;
    }
}
