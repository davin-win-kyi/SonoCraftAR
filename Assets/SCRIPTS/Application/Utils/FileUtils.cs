using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using Types;
using Newtonsoft.Json;

namespace FileUtils 
{
    public class FileUtils
    {
        /**
         * Class Description: This File contains all of the nessecary file related methods that 
         * we will use in order to interact with the files in our system 
         **/
        public static string ReadPromptFromFile(string promptFileName, Dictionary<string, string> varToValue)
        {
            /**
             * In this method we will read in prompts that we made 
             * which will later be augmented with the input given by users 
             * and then passed into GPT
             **/


            // Path of the prompt we are trying to read in 
            string path = Application.dataPath + "/Scripts/Application/Prompts/" + promptFileName;

            try
            {
                // If the file exist, we will read in 
                // the text of the prompt 
                if (File.Exists(path))
                {
                    string prompt = File.ReadAllText(path);

                    Debug.Log("This is the prompt: " + prompt);

                    StringBuilder sb = new StringBuilder(prompt);
                    foreach (string var in varToValue.Keys)
                    {
                        sb.Replace($"<{var}>", varToValue[var]);
                    }
                    return sb.ToString();
                }

                // Log an error if file does not exist
                else
                {
                    Debug.LogError(path + " not found");
                }
            }

            // Issue with reading file 
            catch (Exception e)
            {
                Debug.LogError($"Error: {e.Message}");
            }

            // Could not read file/no file 
            return null;
        }


        // public static void ClearScripts() {
        //     /**
        //      * This method allows us to clear the scripts that we will be running.
        //      * This is nessecary when we want to clear any past scripts for a new
        //      * user or if the current user wants to clear old scripts that are 
        //      * not needed. 
        //      **/ 

        //     // The file with the scripts that we want to clear 
        //     string filePath = Application.dataPath + "/Scripts/Application/Json/Script.json";

        //     // If the file exists, then we will want to clear 
        //     if (File.Exists(filePath))
        //     {
        //         // Clear all of the current scripts we have 
        //         string jsonContent = File.ReadAllText(filePath);

        //         ScriptDataList scriptDataList = JsonUtility.FromJson<ScriptDataList>(jsonContent);
        //         if (scriptDataList != null)
        //         {
        //             scriptDataList.scripts.Clear();
        //             string emptyScriptDataList = JsonUtility.ToJson(scriptDataList, true);
        //             File.WriteAllText(filePath, emptyScriptDataList);
        //         }
        //         else
        //         {
        //             Debug.LogError("Failed to parse the JSON file.");
        //         }
        //     }
        //     else
        //     {
        //         Debug.LogError($"File not found at {filePath}");
        //     }
        // }
        
        public static void ClearScripts() 
{
    /**
     * This method allows us to clear the scripts that we will be running.
     * This is necessary when we want to clear any past scripts for a new
     * user or if the current user wants to clear old scripts that are 
     * not needed. 
     **/ 

    // The file with the scripts that we want to clear 
    string filePath = Application.dataPath + "/Scripts/Application/Json/Scripts.json";

    // If the file exists, then we will want to clear 
    if (File.Exists(filePath))
    {
        try
        {
            // Create an empty script structure
            var emptyScriptData = new
            {
                userPrompt = "",
                scriptContent = "",
                drawUI = false
            };

            // Use Newtonsoft.Json to match the saving format
            string emptyJson = JsonConvert.SerializeObject(emptyScriptData, Formatting.Indented);
            File.WriteAllText(filePath, emptyJson);
            
            Debug.Log("Scripts cleared successfully!");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error clearing scripts: {e.Message}");
        }
    }
    else
    {
        Debug.LogWarning($"No scripts file found at {filePath} to clear.");
    }
}
    }
}
