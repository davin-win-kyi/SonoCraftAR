using System.Collections;
using Shapes;
using UnityEngine;

public class DynamicCircleSize : ImmediateModeShapeDrawer
{
    private float sphereRadius = 1f;

    private float xRange = 0f;

    private float yRange = 0f;

    private float dynamicRadius; 


    /// <summary>
    /// CHANGES SIZE BASED ON INCOMING VOLUME
    /// </summary>
    /// <param name="soundData"></param>
    public void UpdateSoundData(object[] soundData)
    {
        if (soundData.Length >= 3)
        {
            string classification = soundData[0] as string;
            float pitch = (float)soundData[1];
            float distance = (float)soundData[2];

            ////min and max
            //float normalizedPitch = Mathf.Lerp(1f, 2f, Mathf.InverseLerp(200, 4000, pitch));
            //dynamicRadius = sphereRadius * normalizedPitch * 2;
            //Debug.Log($"Pitch: {pitch}, Normalized pitch: {normalizedPitch}");

            float rawVolume = Mathf.Clamp01(1f / (distance + 1f)); //acc to gpt, volume is 1/(distance + 1)
            float volume = Mathf.Clamp(rawVolume * 5f, 1f, 10f); //mult by 5 for scaling
            dynamicRadius = sphereRadius * volume; // Adjust based on volume
            Debug.Log($"Raw volume: {rawVolume}, Volume: {volume}");
        }
    }

    public override void DrawShapes(Camera cam)
    {
        using (Draw.Command(cam))
        {
            Draw.Color = Color.green;
            Draw.LineGeometry = LineGeometry.Volumetric3D;
            Draw.ThicknessSpace = ThicknessSpace.Pixels;
            Draw.Thickness = 4;

            Vector3 position = new Vector3(xRange, yRange, 0);

            Draw.Sphere(position, dynamicRadius);
        }
    }
}
