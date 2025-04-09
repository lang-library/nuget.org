//css_nuget EasyObject
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using static Global.EasyObject;

namespace JcCommon;

public static class CscsUtil
{
    public static List<string> SrcList = new List<string> { };
    public static List<string> PkgList = new List<string> { };
    public static List<string> AsmList = new List<string> { };
    public static List<string> ResList = new List<string> { };
    public static void DebugDump()
    {
        Log(SrcList, "SrcList");
        Log(PkgList, "PkgList");
        Log(AsmList, "AsmList");
        Log(ResList, "ResList");
    }
    public static void ParseProject(string projFileName)
    {
        string cwd = Directory.GetCurrentDirectory();
        projFileName = Path.GetFullPath(projFileName);
        ParseProjectHelper(projFileName);
        Directory.SetCurrentDirectory(cwd);
        for (int i = 0; i < SrcList.Count; i++)
        {
            string src = SrcList[i];
            ParseSource(src);
        }
    }
    private static void ParseProjectHelper(string projFileName)
    {
        ///Echo(projFileName, "projFileName");
        projFileName = Path.GetFullPath(projFileName);
        if (!SrcList.Contains(projFileName))
            SrcList.Add(projFileName);
        string projDir = Path.GetDirectoryName(projFileName);
        Directory.SetCurrentDirectory(projDir);
        string source = File.ReadAllText(projFileName);
        string[] lines = JcCommon.Api.TextToLines(source).ToArray();
        for (int i = 0; i < lines.Length; i++)
        {
            string pat = @"^//css_inc[ ]+([^ ;]+)[ ]*;?[ ]*";
            Regex r = new Regex(pat);
            Match m = r.Match(lines[i]);
            if (m.Success)
            {
                string srcName = m.Groups[1].Value;
                srcName = Path.GetFullPath(srcName);
                ParseProjectHelper(srcName);
            }
        }
    }
    private static void ParseSource(string srcPath)
    {
        string source = File.ReadAllText(srcPath);
        Directory.SetCurrentDirectory(Path.GetDirectoryName(srcPath));
        string[] lines = JcCommon.Api.TextToLines(source).ToArray();
        for (int i = 0; i < lines.Length; i++)
        {
            {
                string pat = @"^//css_nuget[ ]+([^ ;]+)[ ]*;?[ ]*";
                Regex r = new Regex(pat);
                Match m = r.Match(lines[i]);
                if (m.Success)
                {
                    string pkgName = m.Groups[1].Value;
                    if (!PkgList.Contains(pkgName))
                    {
                        PkgList.Add(pkgName);
                    }
                }

            }
            {
                string pat = @"^//css_ref[ ]+([^ ;]+)[ ]*;?[ ]*";
                Regex r = new Regex(pat);
                Match m = r.Match(lines[i]);
                if (m.Success)
                {
                    string asmName = m.Groups[1].Value;
                    if (!AsmList.Contains(asmName))
                    {
                        AsmList.Add(asmName);
                    }
                }

            }
            {
                string pat = @"^//css_embed[ ]+([^ ;]+)[ ]*;?[ ]*";
                Regex r = new Regex(pat);
                Match m = r.Match(lines[i]);
                if (m.Success)
                {
                    string resName = m.Groups[1].Value;
                    resName = Path.GetFullPath(resName);
                    if (!ResList.Contains(resName))
                    {
                        ResList.Add(resName);
                    }
                }

            }
        }
    }
}
