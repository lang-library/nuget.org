using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CSLisp.Core;
using CSLisp.Data;
using static Global.EasyObject;

namespace CSLisp.Demo
{
    internal class MyLogger : ILogger
    {
        bool ILogger.EnableParsingLogging => false;

        bool ILogger.EnableInstructionLogging => false;

        bool ILogger.EnableStackLogging => false;

        void ILogger.Log(params object[] args)
        {
            //Echo(args, "args");
            //throw new NotImplementedException();
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            //Context ctx = new Context(true, new MyLogger());    // make a new vm + compiler
            Context ctx = new Context();    // make a new vm + compiler
            var ret = ctx.CompileAndExecute("(+ 1 2)");
            Echo(ret, "ret");
            Echo(ret.Count, "ret.Count");
            string exp = Val.DebugPrint(ret[0].output);
            Echo(exp);
            Echo(ret[0].output);
        }
    }
}
