using UnityEditor;
using UnityEngine;

public static class CreateInterfaceMenuItem
{
    private const string TemplatePath = "Assets/ScriptTemplates/InterfaceTemplate.cs.txt";

    [MenuItem("Assets/Create/Scripting/C# Interface", false, 81)]
    public static void CreateInterface()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(TemplatePath, "NewInterface.cs");
    }
}