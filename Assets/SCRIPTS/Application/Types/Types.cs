using System.Collections.Generic;
using UnityEngine;


namespace Types {

/**
 * This is all of the objects that represent direction 
 * data for a given object 
 **/
    [System.Serializable]
    public class Direction
    {
        /**
         * This class represent the direction and position information 
         * for an object that is currently in the C#
         * unity scene. 
         **/

        // These are the coordinates of the point
        public float x;
        public float y;
        public float z;

        public Direction() {
            // Default constructor 
         }

        public Direction(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static implicit operator Vector3(Direction d)
        {
            // Convert Direction to Vector3
            return new Vector3(d.x, d.y, d.z);
        }

        public static implicit operator Direction(Vector3 v)
        {
            // Convert Vector3 to Direction
            return new Direction(v.x, v.y, v.z);
        }
    }


/**
 * These are all of the object types representing the 
 * sounds that we want to deal with  
 **/
    [System.Serializable]
    public class SoundData
    {
        public float volume;
        public float frequency;
        public float pitch;
    }

    [System.Serializable]
    public class SoundDataList
    {
        public List<SoundData> soundDataList;
    }


/**
 * These are all of the objects representing the Scripts 
 * that we are going to be running using Roslyn 
 **/
    [System.Serializable]
    public class ScriptData
    {
        /**
         * This represent a script that we generated 
         * which we will run in the background of 
         * our application 
         **/
        public string userPrompt;

        public string scriptContent;

        public bool drawUI;
    }

    [System.Serializable]
    public class ScriptDataList
    {
        /**
         * This represents the C# unity code for each of the 
         * scripts that we will be running in the background 
         **/
        public List<ScriptData> scripts = new List<ScriptData>();
    }



/**
 * These are all of the object types representing the UI
 * we want for the noises we are dealing with 
 **/
    [System.Serializable]
    public class NoiseUIList
    {
        /**
         * This class enables us to get
         * a list of all of the NoiseUI
         * objects a user wants 
         **/
        public List<NoiseUI> objects; 
    }

    [System.Serializable]
    public class NoiseUI
    {
        /**
         * This class represents the UI's that 
         * we want to represent for each noise 
         * where we have three main fields:
         * - object_name: this is the name of 
         *   the object for the UI
         * - features: these are the features 
         *   that we want the UI to have 
         * - color_features: these are the 
         *   features we want to have with respect to color 
         **/
        public string object_name;
        public List<string> features;
        public List<string> color_features;
    }
}
