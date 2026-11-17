using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using UnityEngine.SceneManagement;


public class ScriptGeneration : MonoBehaviour
{
    /**
     * Class Description: Generates Scripts for UIs that represent 
     * user defined prompts
     * Important behaviors: 
     * - generating scripts that give UIs that 
     *   represent user input
     * - adding scripts into system 
     **/


    // CONSTANTS
    private string FILE_PATH;
    public static event System.Action ON_SCRIPT_GENERATED;
    public string GPT_FUNCTION_NAME;
    public string SOUND_SIMULATION_FUNCTION_NAME;
    public static event System.Action ON_UPDATED_ENTRY;


    public static Diagnostic[] GetDiagnostics(string code)
    {
        // 1. Parse the code into a syntax tree
        var tree = CSharpSyntaxTree.ParseText(code);

        // 2. Collect all loaded assemblies as metadata references
        var refs = AppDomain.CurrentDomain.GetAssemblies()
                     .Where(a => !a.IsDynamic && !string.IsNullOrEmpty(a.Location))
                     .Select(a => MetadataReference.CreateFromFile(a.Location));

        // 3. Create a compilation
        var comp = CSharpCompilation.Create(
            assemblyName: "InMemoryAssembly",
            syntaxTrees: new[] { tree },
            references: refs,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        // 4. Return only errors/warnings
        return comp.GetDiagnostics()
                   .Where(d => d.Severity == DiagnosticSeverity.Error ||
                               d.Severity == DiagnosticSeverity.Warning)
                   .ToArray();
    }


    public void generateScript(string user_prompt, string enhanced_prompt, float total_time)
    {
        /**
         * Here we will be initializing nessecary ScriptGeneration constants
         **/
        GPT_FUNCTION_NAME = "GeneratedFunction";
        SOUND_SIMULATION_FUNCTION_NAME = "SimulateSoundData";
        addScript(user_prompt, enhanced_prompt, total_time);
    }


    async void addScript(string user_prompt, string enhanced_prompt, float total_time)
    {
        /**
         * Description: this method will generate the script and add the script 
         * into the system as well 
         * Parameters:
         * - user prompt: this is the prompt of the UI that the user wants generated
         * - noiseUI: this is the noise
         **/

        try
        {
            var start_time = Time.time;

            // Initialize ScriptGeneration components
            string promptFileName = "ScriptGeneration/ScriptGenerationNewDesign4.txt";
            string filePath = Application.dataPath + "/Scripts/Application/Json/Scripts.json";
            string prevPrompt = "";
            string prevScript = "";
            if (File.Exists(filePath))
            {
                string jsonText = File.ReadAllText(filePath);
                if (!string.IsNullOrWhiteSpace(jsonText))
                {
                    ScriptData scripts = JsonUtility.FromJson<ScriptData>(jsonText);

                    if (scripts != null)
                    {
                        prevPrompt = scripts.userPrompt;
                        prevScript = scripts.scriptContent;
                    }
                }
            }
            else
            {
                Debug.LogWarning($"Scripts.json not found at: {filePath}");
            }

            string prompt = FileUtils.FileUtils.ReadPromptFromFile(
                promptFileName,
                new Dictionary<string, string>() {
                    { "TAG_ID", user_prompt },
                    { "GPT_FUNCTION_NAME", GPT_FUNCTION_NAME },
                    { "CURRENT_PROMPT", user_prompt },
                    { "PREVIOUS_PROMPT", prevPrompt },
                    { "PREVIOUS_SCRIPT", prevScript },
                    { "ENHANCED_PROMPT", enhanced_prompt },
                });

            var gpt_user_prompt = @"The following is the CURRENT_PROMPT: " + prompt + ". \n" +
                           "The following is the PREVIOUS_PROMPT: " + prevPrompt + ". \n" +
                           "The following is the PREVIOUS_SCRIPT: " + prevScript + ". \n" +
                           "The following is the ENHANCED_PROMPT: " + enhanced_prompt + ". \n";

            string gpt_response =
                await CallGPT.Instance.SendPrompt(prompt,
                                                  gpt_user_prompt,
                                                  false,
                                                  "/Scripts/Application/GPT_Images/good_example.png",
                                                  "/Scripts/Application/GPT_Images/bad_example.png"
                                                 );

            string subclasses = @"                
                public class SoundData
                {
                    public float volume;
                    public float frequency;
                    public float pitch;
                }

                public class SoundDataList
                {
                    public List<SoundData> soundDataList;
                }

                public class ScriptData
                {
                    public string userPrompt;

                    public string scriptContent;

                    public bool drawUI;
                }

                public class ScriptDataList
                {
                    public List<ScriptData> scripts = new List<ScriptData>();
                }";

            if (!gpt_response.Contains("ScriptData"))
            {
                gpt_response += subclasses;
            }

            Diagnostic[] diagnostics = GetDiagnostics(gpt_response);

            string allErrors = string.Join(
                "\n",
                diagnostics.Select(d => d.ToString())
            );

            string OUTPUT_PATH = Application.dataPath + "/Scripts/Application/Json/Output.txt";
            File.WriteAllText(OUTPUT_PATH, gpt_response);

            // Check script for errors
            gpt_response = await ScriptChecker.checkScript(gpt_response, allErrors);

            // Save script for follow up system 
            string formatted_json = gpt_response.Replace("\\n", "\n");
            FILE_PATH = Application.dataPath + "/Scripts/Application/Json/Scripts.json";
            saveScriptToJson(user_prompt, formatted_json, start_time, total_time);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error: {e.Message}");
        }
    }
    

    [System.Serializable]
    public class ScriptData
    {
        public string userPrompt;
        public string scriptContent;
        public bool drawUI;
    }


    public void saveScriptToJson(string user_prompt, string script_content, float start_time, float total_time)
    {
        /**
         * Description: Save a single script to JSON file (replaces entire file)
         * Parameters:
         * - user_prompt: the user prompt
         * - script_content: the generated script content
         **/
        try
        {
            FILE_PATH = Application.dataPath + "/Scripts/Application/Json/Scripts.json";

            // Create a simple script object (no need for a list)
            var scriptData = new ScriptData
            {
                userPrompt = user_prompt,
                scriptContent = script_content,
                drawUI = true
            };

            // Write the JSON file (replacing the entire file)
            string json = JsonUtility.ToJson(scriptData, true);
            File.WriteAllText(FILE_PATH, json);

            var end_time = Time.time;
            var benchmark_time = total_time + (end_time - start_time);

            // For benchmarking of system
            Debug.Log("TOTAL_TIME: " + benchmark_time);

            // Invoke system to rerun scripts
            ON_SCRIPT_GENERATED?.Invoke();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error: {e.Message}");
        }
    }


    private void OnSceneReloaded(Scene scene, LoadSceneMode mode)
    {
        // Remove the listener to prevent multiple calls
        SceneManager.sceneLoaded -= OnSceneReloaded;

        // Now invoke your event after the scene is fully loaded
        ON_UPDATED_ENTRY?.Invoke();
    }


    public void ReloadCurrentScene()
    {
        /**
         * Description: Refresh screen
         **/
        // Get the active scene's name
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Reload the active scene
        SceneManager.LoadScene(currentSceneName);
    }
}
