using System.Collections;
using System.Collections.Generic;
using RoslynCSharp;
using UnityEngine;
using System.IO;
using System;

using Types;

public class ShapesRosylnMockData : MonoBehaviour
{
    private ScriptDomain domain = null;

    //string filePath = Application.dataPath + "/Scripts/Shapes_test/DynamicScripts/DynamicCircleSize.cs";
    string filePath = Application.dataPath + "/Scripts/Prototypes/Complex_UIs/DynamicSpinningColorDiscs.cs";

    bool initCompiler;
    int index = 0;
    string script;
    private ScriptProxy scriptProxy;

    void Start()
    {
        script = File.ReadAllText(filePath);

        initCompiler = true;

        // ===================== feeding it mock sound data ===================== 

        string mockDataFilePath = Application.dataPath + "/Scripts/Application/Json/MockSoundData.json";
        string json = File.ReadAllText(mockDataFilePath);

        SoundDataList soundDataList = JsonUtility.FromJson<SoundDataList>(json);

        domain = ScriptDomain.CreateDomain("MyDomain", initCompiler);

        Debug.Log("==============stuck===============");
        ScriptType type = domain.CompileAndLoadMainSource(script);
        Debug.Log("==============here===============");
        scriptProxy = type.CreateInstance(gameObject);

        // =============== OPTION 1: sound data is feeded IN the compiled script ===============
        //scriptProxy.Call("StartSimulating", soundDataList.soundDataList);

        // =============== OPTION 2: sound data is feeded HERE in the coroutine ===============
        StartCoroutine(SimulateWithMockSoundData(soundDataList.soundDataList));
    }

    IEnumerator SimulateWithMockSoundData(List<SoundData> data)
    {
        Debug.Log("(ShapesRosylnMockData.cs) Starting simulation");
        for (int i = 0; i < data.Count; i++)
        {
            if (index >= data.Count) index = 0;

            object[] arg = new object[] { data[index].volume, data[index].frequency, data[index].pitch };
            scriptProxy.Call("UpdateSoundData", new object[] { arg });

            index++;
            yield return new WaitForSeconds(3f);
        }
    }
}
