//css_inc JcCommon.Math.cs
//css_nuget EasyObject
using System.Collections.Generic;
using System.IO;
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
    public static List<string> TextToLines(string text)
    {
        List<string> lines = new List<string>();
        using (StringReader sr = new StringReader(text))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                lines.Add(line);
            }
        }
        return lines;
    }
}
