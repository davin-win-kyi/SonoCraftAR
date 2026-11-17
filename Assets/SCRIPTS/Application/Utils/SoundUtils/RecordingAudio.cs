using UnityEngine;
using System.IO;
using System;
public class RecordingAudio : MonoBehaviour
{
    /**
     * Class description: This class allows a user to start recording audio
     * using the Unity C# library
     **/

    // CONSTANTS
    private AudioClip recordedClip; 
    [SerializeField] AudioSource audioSource;
    private float sampleRate;
    private float startTime;
    private float recordingLength;
    private string filePath = "test.wav";
    private string directoryPath = "Assets/Scripts/SoundData";
    private string device;
    private int currentSamplePosition;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) {
            Debug.Log("audio source is null");
        }
        device = Microphone.devices[0];
        int sampleRate = 44100; 
        //how many seconds we want our audioclip to be AT MOST (max is 3599)
        int lengthSec = 60;

        // Link to Microphone documentation
        // - https://docs.unity3d.com/ScriptReference/Microphone.Start.html
        recordedClip = Microphone.Start(device, false, lengthSec, sampleRate);
    }


    public void StopRecording()
    {
        /**
         * This method will allow us to stop the recording 
         * and save the current contents that we have stored 
         **/
        currentSamplePosition = Microphone.GetPosition(device);
        Microphone.End(null);
        recordingLength = 5;
        recordedClip = TrimClip(recordedClip, recordingLength);
        SaveRecording();
        // start a new recording
        Start();
    }


    private AudioClip TrimClip(AudioClip clip, float length)
    {
        /**
         * Here we will be trimming the given audio of the file 
         * by "length" seconds 
         **/ 
        // this is to get the previous 3-5 seconds 
        int samples = (int)(clip.frequency * length);
        // get the place where we want to start getting audio from 
        int start = Math.Max(currentSamplePosition - samples, 0);
        // get the data
        float[] data = new float[samples];
        clip.GetData(data, start);
        // return the clip with the audio of interest
        AudioClip trimmedClip = AudioClip.Create(clip.name, samples, clip.channels, clip.frequency, false);
        trimmedClip.SetData(data, 0);
        return trimmedClip;
    }

    // saves our audioclip as a .wav file
    public void SaveRecording()
    {
        /**
         * Here we will be saving the given recording 
         * to a .wav file 
         **/ 
        if (recordedClip != null)
        {
            string path = Path.Combine(directoryPath, filePath);
            WavManager.Save(path, recordedClip);
        }
        else
        {
            Debug.Log("SaveRecording(): No recording found!");
        }
    }
}
