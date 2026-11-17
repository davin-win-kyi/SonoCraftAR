using UnityEngine;

namespace MockUtils {
    public class MockUtils : MonoBehaviour {
        /**
        * Class Description: This class enables us to set the flags of 
        * whether or not we will be using mock data or not
        **/

        // CONSTANTS
        private static MockUtils _instance;
        public static bool USE_MOCK_SOUND_DATA = true;
        public static bool USE_SAMPLE_SCRIPT = false;

        public static MockUtils Instance
        {
            // Instance for MockUtils class 
            get
            {
                if(_instance == null) { Debug.LogError("No instance of MockUtils"); }
                return _instance;
            }
        }

        private void Awake()
        {
            // Set the instance as the system runs 
            _instance = this;
        }
    }
}
