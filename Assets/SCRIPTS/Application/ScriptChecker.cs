using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;


public class ScriptChecker : MonoBehaviour
{
    /**
     * Class Description: Does a second run over the runability of the code 
     * Things that the class checks:    
     * - conversion errors
     * - nonexsistent classes, methods, libraries etc. 
     **/


    // CONSTANTS
    private string FILE_PATH;
    public static event System.Action ON_SCRIPT_GENERATED;


    public static Task<string> checkScript(string script, string errors)
    {
        /**
         * Description: checks the scripts correctness
         * Parameters:
         * - script: the script that we will check for corectness
         **/

        return checkScriptHelper(script, errors);
    }

    public async static Task<string> checkScriptHelper(string script, string errors)
    {
        /**
         * Description: Method will analyze the script with the correctness prompt
         * Parameters:
         * - script: this is the script we will analyze and correct
         **/
        try
        {
            string promptFilePath = "ScriptChecker/ScriptChecker.txt";
            string gpt_prompt = FileUtils.FileUtils.ReadPromptFromFile(
                promptFilePath,
                new Dictionary<string, string>() {
                    { "GPT_FUNCTION_NAME", "GeneratedFunction"},
                });

            // Pass Script and ScriptChecker prompt to fix script for corectness
            string user_input = "This is the script: ```" + "\n" + script + "``` \n and these are the errors: " + errors;

            string gpt_response = await CallGPT.Instance.SendPrompt(gpt_prompt, user_input, false);

            // Return corrected script
            string formatted_json = gpt_response.Replace("\\n", "\n");
            return formatted_json;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error: {e.Message}");
            return null;
        }
    }



    // OLD VERSION

    public static Task<string> checkScript(string script)
    {
        /**
         * Description: checks the scripts correctness
         * Parameters:
         * - script: the script that we will check for corectness
         **/

        return checkScriptHelper(script);
    }

    public async static Task<string> checkScriptHelper(string script)
    {
        /**
         * Description: Method will analyze the script with the correctness prompt
         * Parameters:
         * - script: this is the script we will analyze and correct
         **/
        try
        {
            string promptFilePath = "/Scripts/Application/Prompts/ScriptChecker/ScriptChecker.txt";

            // Retrive ScriptChecker Prompt
            string scriptCheckerFilename = Application.dataPath + promptFilePath;
            string prompt = File.ReadAllText(scriptCheckerFilename);

            // Pass Script and ScriptChecker prompt to fix script for corectness
            string gpt_input = prompt;
            string gpt_response = await CallGPT.Instance.SendPrompt(gpt_input, script, false);

            // Return corrected script
            string formatted_json = gpt_response.Replace("\\n", "\n");
            return formatted_json; 
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error: {e.Message}");
            return null;
        }
    }
}