using UnityEngine;
using RoslynCSharp;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Types;

public class ScriptsRun : MonoBehaviour
{
    public static ScriptsRun INSTANCE { get; private set; }
    public static ScriptDomain DOMAIN = null;
    public static ScriptProxy SCRIPT_PROXY;
    private string GPT_FUNCTION_NAME;
    private string FILE_PATH;
    // mock data 
    private int mockDataIndex = 0;
    private List<SoundData> mockData = null;
    // Timer for Real-time data
    private float timer = 0f;
    public float delay = 0.01f;


    // Simple script data structure to match new JSON format
    [System.Serializable]
    public class SimpleScriptData
    {
        public string userPrompt;
        public string scriptContent;
        public bool drawUI;
    }


    private void Awake()
    {
        // Singleton pattern to prevent multiple instances
        if (INSTANCE == null)
        {
            INSTANCE = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void Start()
    {
        GPT_FUNCTION_NAME = "GeneratedFunction";
        FILE_PATH = Application.dataPath + "/Scripts/Application/Json/Scripts.json";

        // Subscribe to events
        SubscribeToEvents();
    }


    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        UnsubscribeFromEvents();
    }


    private void SubscribeToEvents()
    {
        ScriptGeneration.ON_SCRIPT_GENERATED += HandleNewScript;
    }


    private void UnsubscribeFromEvents()
    {
        ScriptGeneration.ON_SCRIPT_GENERATED -= HandleNewScript;
    }


    // destroys all existing visualizations
    private void HandleNewScript()
    {

        // Destroy all existing visualizations
        DestroyAllVisualizations();

        // Generate new visualizations
        StartCoroutine(RunScriptsCoroutine());
    }


    // Destroy visualizations for new UI generations
    private void DestroyAllVisualizations()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // resetting static variables
        DOMAIN = null;
        SCRIPT_PROXY = null;

        // stopping all coroutines to reset execution
        StopAllCoroutines();
    }

    public IEnumerator RunScriptsCoroutine()
    {
        /**
         * Utilize this method to run the single script that's saved. 
         **/ 
        yield return new WaitForEndOfFrame();

        try
        {
            if (File.Exists(FILE_PATH))
            {
                // Read the existing JSON
                string scriptsJson = File.ReadAllText(FILE_PATH);

                // Parse as a single script object instead of a list
                SimpleScriptData script = JsonUtility.FromJson<SimpleScriptData>(scriptsJson);

                if (script != null && !string.IsNullOrEmpty(script.scriptContent))
                {
                    // Creating domain now
                    DOMAIN = ScriptDomain.CreateDomain("MyTestDomain", true);

                    // Run the single script if drawUI is true
                    if (script.drawUI)
                    {
                        ScriptType type = DOMAIN.CompileAndLoadMainSource(script.scriptContent);
                        if (type != null)
                        {   
                            // creating a new GameObject for this script instance
                            GameObject scriptObj = new GameObject(script.userPrompt);
                            
                            // making it a child of this GameObject for organization
                            scriptObj.transform.SetParent(this.transform);
                            
                            // creating the script instance on the new GameObject
                            SCRIPT_PROXY = type.CreateInstance(scriptObj);
                            
                            if (MockUtils.MockUtils.USE_MOCK_SOUND_DATA)
                            {
                                RunScriptsMockData();
                            }
                        }
                        else
                        {
                            Debug.LogError("Failed to compile script");
                        }
                    }
                    else
                    {
                        Debug.Log("Script drawUI is false, skipping");
                    }
                }
                else
                {
                    Debug.LogError("Script data is null or empty");
                }
            }
            else
            {
                Debug.LogError("Scripts.json file not found at: " + FILE_PATH);
            }
        }
        // error case
        catch (System.Exception e)
        {
            Debug.LogError($"Error: {e.Message}");
        }
    }

    private void RunScriptsMockData()
    {
        /**
         * Run through this method to run scripts using 
         * mock sound data 
         **/

        // path with mock sound data 
        string mockDataFilePath = Application.dataPath + "/Scripts/Application/Json/MeowingSoundData.json";
        try
        {
            // Get the sound data and run through the sound data with the given script 
            SoundDataList soundDataList;
            if (File.Exists(mockDataFilePath))
            {
                // simulate passing in real-time audio to GeneratedFunction
                string json = File.ReadAllText(mockDataFilePath);
                soundDataList = JsonUtility.FromJson<SoundDataList>(json);
                if (soundDataList != null && soundDataList.soundDataList != null)
                {
                    this.mockData = soundDataList.soundDataList;
                }
                // no sounds to go over 
                else
                {
                    Debug.LogError("(ScriptsRun - mockSoundDataTestUtil) Parsed Mock Data is null or empty!");
                }
            }
            // the sound data is not found 
            else
            {
                Debug.LogError("(ScriptsRun - mockSoundDataTestUtil) Mock sound data file path not found!");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error: {e.Message}");
        }
    }

    void Update()
    {
       timer += Time.deltaTime; 

       if (timer >= delay && SCRIPT_PROXY != null) 
       {
            /**
            * Here we will be giving the UIs of the sounds with the 
            * given SoundData
            **/
                if (mockData != null )
                {
                    if (mockDataIndex >= mockData.Count) mockDataIndex = 0;
                    object[] arg = new object[] { mockData[mockDataIndex].volume, mockData[mockDataIndex].frequency, mockData[mockDataIndex].pitch };

                    SCRIPT_PROXY.Call(GPT_FUNCTION_NAME, new object[] { arg });
                    mockDataIndex++;

                    timer = 0f;
                }
        }
    }
}