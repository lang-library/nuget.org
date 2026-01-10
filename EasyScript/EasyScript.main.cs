//css_inc EasyScript.cs
//css_nuget EasyObject
namespace EasyScript;

using static Global.EasyObject;

public class Program
{
    public static void Main(string[] args)
    {
        Log(args, "args");
        Echo("helloハロー©");
        var engine = new Global.EasyScript();
        var result = engine.EvaluateAsEasyObject(
            """
            var answer = 111 + 222;
            echo(answer, "answer");
            return answer;
            """);
            
        Echo(result.IsNumber);
        Echo(result);
    }
}
