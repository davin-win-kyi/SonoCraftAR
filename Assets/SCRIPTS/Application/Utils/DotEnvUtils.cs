using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace DotEnvUtils {
    /**
     * Class Description: This class enables folks to load a given .env file and get 
     * the env variables and their values which we need for our
     * system. 
     **/
    public static class DotEnvUtils
    {
        //CONSTANTS

        // This is where we store all of the env variables we find 
        private static Dictionary<string, string> envVariables = new Dictionary<string, string>();

        public static void Load(string filePath)
        {
            // Load the .env file

            if (!File.Exists(filePath))
            {
                Debug.LogError($"File not found: {filePath}");
                return;
            }

            string[] lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                // Skip empty lines or comments
                if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#")) continue;

                string[] parts = line.Split('=', 2); 
                if (parts.Length == 2)
                {
                    string key = parts[0].Trim();
                    string value = parts[1].Trim();

                    envVariables[key] = value;
                }
            }
        }

        public static string Get(string key)
        {
            // Retrieve a value by key
            if (envVariables.TryGetValue(key, out string value))
            {
                return value;
            }

            Debug.LogWarning($"Key not found: {key}");
            return null;
        }
    }
}
