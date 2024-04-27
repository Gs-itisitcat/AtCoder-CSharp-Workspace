using System.Reflection;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
public static string GetScriptFolder([CallerFilePath] string path = null) => Path.GetDirectoryName(path);
var rootDir = GetScriptFolder();


var assemblyRegex = new Regex(@"\s*#r .+", RegexOptions.Compiled);
var usingRegex = new Regex(@"using (static )?[\w\.]+;", RegexOptions.Compiled);


var templatePath = Path.Combine(rootDir, "ExpandTemplate.cs");
var template = File.ReadAllText(templatePath);

var codeFile = Args[0];
WriteLine(codeFile);
// var codeFile = "./ABS/ABC086C.csx";

var codeFileInfo = new FileInfo(codeFile);
var codeFileNameWithoutExtension = Path.GetFileNameWithoutExtension(codeFile);
var code = File.ReadAllText(codeFile);

var codeNoAssembly = assemblyRegex.Replace(code, "");
var codeCore = usingRegex.Replace(codeNoAssembly, "");

var combined = template + "\n" + codeCore;
var additionalUsing = usingRegex.Matches(code).Select(m => m.Value).Where(s => !template.Contains(s)).Aggregate("", (p, n) => p + n + "\n");

var dirPath = Path.Combine("Combined", Path.GetRelativePath(Path.Combine(GetScriptFolder(), ".."), codeFileInfo.Directory.FullName));

if (!Directory.Exists(dirPath))
{
    Directory.CreateDirectory(dirPath);
}

var filePath = Path.Combine(dirPath, codeFileNameWithoutExtension + "_Combined.cs");
File.WriteAllText(filePath, additionalUsing + combined);
