//css_inc Sys.cs
//css_nuget EasyObject
using System;
using static Global.EasyObject;

namespace Sys
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log(args, "args");
            Echo("helloハロー©");
            Echo(Global.Sys.Add2(11, 22));
        }
    }
}
