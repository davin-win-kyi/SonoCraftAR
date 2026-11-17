using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Networking;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;


[System.Serializable]
public class DocEmbedding
{
    public string text;
    public float[] embedding;
}

public class CallGPT : MonoBehaviour
{
    [Header("OpenAI Info")]
    [SerializeField] string apiUrl = "https://api.openai.com/v1/chat/completions";

    // [SerializeField] string model = "gpt-4o";
    [SerializeField] string model = "o3";

    // Make sure to set this in .env
    private string openAIKey;

    public static CallGPT Instance;

    // which embedding model to call
    public string embeddingModel = "text-embedding-ada-002";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        string envPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", ".env"));
        DotEnvUtils.DotEnvUtils.Load(envPath);
        openAIKey = DotEnvUtils.DotEnvUtils.Get("openAIKey");
    }


    public async Task<string> SendPrompt(string systemPrompt, string userPrompt,
                                            bool textAndImage,
                                            string good_example = "../Scripts/Application/GPT_Images/good_example.png",
                                            string bad_example = "../Scripts/Application/GPT_Images/bad_example.png")
    {
        /**
         * Description: Creating prompt for ScriptGeneration
         * Parameters:
         * - prompt: this is the prompt that the user will be passing
         * - testAndImage: VLM usage flag
         * - good_example: the path of the good image example
         * - bad_example: the path of the bad image example
         **/
        if (string.IsNullOrEmpty(openAIKey))
        {
            Debug.LogError("OpenAI API Key is not set.");
            return null;
        }

        // Message sent to OpenAI
        var messages = new List<object>();

        if (textAndImage)
        {
            messages = new List<object>
            {
                CreateTextMessage("system", systemPrompt),
                CreateTextMessage("user", userPrompt),
            };
        }
        else
        {
            Debug.Log("NOT sending images, not VLM mode");
            messages = new List<object>
            {
                CreateTextMessage("system", systemPrompt),
                CreateTextMessage("user", userPrompt),
            };
        }

        // Packaging payload for GPT
        var payload = new
        {
            model = model,
            messages = messages
        };
        string jsonPayload = JsonConvert.SerializeObject(payload);

        // Send request
        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            // Convert to readable format for GPT
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            // Set WebRequest Headers
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {openAIKey}");

            // Send Request
            var asyncOp = request.SendWebRequest();
            while (!asyncOp.isDone)
                await Task.Yield();

            // Parse Response
            if (request.result == UnityWebRequest.Result.Success)
            {
                var response = request.downloadHandler.text;
                var parsedResponse = JsonConvert.DeserializeObject<OpenAIResponse>(response);
                if (parsedResponse?.choices != null && parsedResponse.choices.Count > 0)
                {
                    // Send response and save response for debugging purposes
                    string assistantMessage = parsedResponse.choices[0].message.content;
                    string filePath = Application.dataPath + "/Scripts/Application/Output.txt";
                    File.WriteAllText(filePath, assistantMessage);
                    return assistantMessage;
                }
                else
                {
                    Debug.LogWarning("No content found in the response.");
                }
            }
            else
            {
                Debug.LogError($"Error: {request.responseCode} - {request.error}");
                Debug.LogError(request.downloadHandler.text);
            }
            return null;
        }
    }


    private object CreateTextMessage(string role, string content)
    {
        /**
         * Description: This will create the text objects for GPT requests
         * Parmeters: 
         * - role: this is the role that the given message will have 
         * - content: this is the textual request given to GPT
         **/

        return new
        {
            role = role,
            content = content
        };
    }
}

public class OpenAIResponse
{
    /**
     * Object format of GPT response
     **/

    public string id { get; set; }
    public string @object { get; set; }
    public int created { get; set; }
    public string model { get; set; }
    public List<Choice> choices { get; set; }
}

public class Choice
{
    /**
     * There are multiple completions of your request
     * and each choice is an alternative response 
     * from GPT
     **/

    public int index { get; set; }
    public Message message { get; set; }
    public string finish_reason { get; set; }
}

public class Message
{   
    /**
     * Contains the content for one Choice response from GPT
     **/
    public string role { get; set; }
    public string content { get; set; }
}
