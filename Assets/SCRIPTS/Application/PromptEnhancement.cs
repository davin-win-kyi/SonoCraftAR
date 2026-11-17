using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PromptEnhancement: MonoBehaviour
{
    /**
     * Class Description: enhances user input into the Generative AI
     * branch of our system. 
     * Important behaviors: 
     * - enhancing user input
     * - clearing old scripts when a new user uses our system 
     * - calling ScripGeneration when the enhance prompt has 
     *   been made
     **/

    //FIELDS
    [SerializeField]
    public TMP_InputField USER_INPUT;
    public static bool firstTime = true;


    public void Start() {
        // Clear scripts if loading system
        if (firstTime) {
            FileUtils.FileUtils.ClearScripts();
            firstTime = false;
        }
    }


    public async void GetUIs()
    {
        var start_time = Time.time;

        string promptFileName = "PromptEnhancement/PromptEnhancementCodeAndVisualGuidelines.txt";
        string prompt = FileUtils.FileUtils.ReadPromptFromFile(
            promptFileName,
            new Dictionary<string, string>() { 
                                               { "GPT_FUNCTION_NAME", "GeneratedFunction" },
                                                });

        // Separating system prompt from user input
        string enhancedPrompt = await CallGPT.Instance.SendPrompt(prompt, USER_INPUT.text, false);

        GameObject roslynManager = GameObject.Find("RosylnManager");
        ScriptGeneration scriptGeneration = roslynManager.GetComponent<ScriptGeneration>();

        var end_time = Time.time;
        var time_elapsed = end_time - start_time;
        scriptGeneration.generateScript(USER_INPUT.text, enhancedPrompt, time_elapsed);
    }
}