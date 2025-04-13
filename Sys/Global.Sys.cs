//css_nuget EasyObject
using static Global.EasyObject;

namespace Global
{
    public static class Sys
    {
        public static int Add2(int a, int b)
        {
            Echo(a, "a");
            Echo(b, "b");
            return a + b;
        }
    }
}
