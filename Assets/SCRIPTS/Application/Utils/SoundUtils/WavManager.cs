using UnityEngine;
using System.IO;
using System;
public class WavManager: MonoBehaviour
{
    /**
     * Class Description: This class enables us to make a new .wav file 
     * using the Unity C# library
     **/

    // CONSTANTS
    const int HEADER_SIZE = 44;

    private static FileStream CreateEmpty(string filePath)
    {
        /**
         * This method enables us to create an empty .wav file 
         **/ 
        FileStream fileStream = new FileStream(filePath, FileMode.Create);
        byte emptyByte = new byte();
        for (int i = 0; i < HEADER_SIZE; i++)
        {
            fileStream.WriteByte(emptyByte);
        }
        return fileStream;
    }


    public static void Save(string filePath, AudioClip clip)
    {
        /**
         * This method enables us to save the given audoclip as a
         * .wav file 
         **/ 
        if (!filePath.ToLower().EndsWith(".wav"))
        {
            filePath += ".wav";
        }
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        using (FileStream fileStream = CreateEmpty(filePath))
        {
            ConvertAndWrite(fileStream, clip);
            WriteHeader(fileStream, clip);
        }
    }

    private static void ConvertAndWrite(FileStream fileStream, AudioClip clip)
    {
        /** 
         * Here we will convert the data in the AudioClip into 
         * a readable format that the .wav file can understand
         **/
        // retrieve the audioclip as a float array
        float[] samples = new float[clip.samples];
        // fills an array with sample data (float array) from the clip
        clip.GetData(samples, 0);
        Int16[] intData = new Int16[samples.Length];
        Byte[] bytesData = new Byte[samples.Length * 2];
        const float rescaleFactor = 32767;
        // converting the 16-bit int array to a byte array
        //since files are written as bytes
        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);
            Byte[] byteArr = new Byte[2];
            byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }
        // writing the byte array to the file stream
        fileStream.Write(bytesData, 0, bytesData.Length);
    }


    private static void WriteHeader(FileStream fileStream, AudioClip clip)
    {
        /**
         * This is where we will be writing the header of the 
         * files 44 bytes of header data 
         **/
        int hz = clip.frequency;
        int channels = clip.channels;
        int samples = clip.samples;
        fileStream.Seek(0, SeekOrigin.Begin);
        Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
        fileStream.Write(riff, 0, 4);
        Byte[] chunkSize = BitConverter.GetBytes(fileStream.Length - 8);
        fileStream.Write(chunkSize, 0, 4);
        Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
        fileStream.Write(wave, 0, 4);
        Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
        fileStream.Write(fmt, 0, 4);
        Byte[] subChunk1 = BitConverter.GetBytes(16);
        fileStream.Write(subChunk1, 0, 4);
        UInt16 one = 1;
        Byte[] audioFormat = BitConverter.GetBytes(one);
        fileStream.Write(audioFormat, 0, 2);
        Byte[] numChannels = BitConverter.GetBytes(channels);
        fileStream.Write(numChannels, 0, 2);
        Byte[] sampleRate = BitConverter.GetBytes(hz);
        fileStream.Write(sampleRate, 0, 4);
        Byte[] byteRate = BitConverter.GetBytes(hz * channels * 2);
        fileStream.Write(byteRate, 0, 4);
        UInt16 blockAlign = (ushort)(channels * 2);
        fileStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);
        UInt16 bps = 16;
        Byte[] bitsPerSample = BitConverter.GetBytes(bps);
        fileStream.Write(bitsPerSample, 0, 2);
        Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
        fileStream.Write(datastring, 0, 4);
        Byte[] subChunk2 = BitConverter.GetBytes(samples * channels * 2);
        fileStream.Write(subChunk2, 0, 4);
    }
}


