using System.Collections;
using System.Collections.Generic;
using System.IO;
using RoslynCSharp;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class TestShapesRoslyn : MonoBehaviour
{
    private ScriptDomain domain = null;

    string filePath = Application.dataPath + "/Scripts/Prototypes/Shapes_test/Shapes/ProceduralTree.cs";
    // /Users/davinwinkyi/Documents/Sound-UI-Authoring/Assets/Scripts/Prototypes/Shapes/Shapes/Compass.cs

    //referencing the dll path for shapes library!
    //string shapesRuntimeDllPath = Path.Combine(Application.dataPath, "../Library/ScriptAssemblies/ShapesRuntime.dll");


    public void Start()
    {
        string script = File.ReadAllText(filePath);

        bool initCompiler = true;

        domain = ScriptDomain.CreateDomain("MyDomain", initCompiler);
        ScriptType type = domain.CompileAndLoadMainSource(script);

        ScriptProxy proxy = type.CreateInstance(gameObject);

        // proxy.Call("UpdateSoundData", true);
        proxy.Call("ToggleDrawing", true);
    }
}
