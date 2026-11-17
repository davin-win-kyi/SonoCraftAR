using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

public class DynamicRadar : ImmediateModeShapeDrawer
{
    private float sphereRadius = 1f;
    private float xRange = 0f;
    private float yRange = 0f;
    private float dynamicRadius = 1f;

    // This will get updated!
    private int radarRingCount = 1;

    // Time variable to control the fade animation
    private float time;

    public void UpdateSoundData(object[] soundData)
    {
        if (soundData.Length >= 3)
        {
            string classification = soundData[0] as string;
            float pitch = (float)soundData[1];
            float distance = (float)soundData[2];

            // ======= Normalizing the pitch to control the number of radar rings (min 1, max 5) ======= 
            // Mapping pitch values (between 200 to 4000 Hz) to values between 1 and 5
            float mappedPitchValues = Mathf.Lerp(1f, 5f, Mathf.InverseLerp(200, 4000, pitch));
            // Mapping mappedPitchValues for radarRingCount (between 1 to 5)
            radarRingCount = Mathf.Clamp(Mathf.FloorToInt(mappedPitchValues), 1, 5);

            // Size of radius also changing based on volume!
            // dynamicRadius = sphereRadius * Mathf.Clamp(1f / (distance + 1f), 0.5f, 1f);

            // Debug.Log($"Pitch: {pitch}, Radar Rings: {radarRingCount}");
        }
    }

    public override void DrawShapes(Camera cam)
    {
        // Update the time variable
        time += Time.deltaTime;

        using (Draw.Command(cam))
        {
            Draw.LineGeometry = LineGeometry.Volumetric3D;
            Draw.ThicknessSpace = ThicknessSpace.Pixels;
            Draw.Thickness = 3;

            Vector3 position = new Vector3(xRange, yRange, 0);

            // Draw dynamic rings based on radarRingCount
            for (int i = 1; i <= radarRingCount; i++)
            {
                // Calculate alpha value for fading effect
                float alpha = Mathf.Abs(Mathf.Sin(time + i)); // Adjust phase by i to stagger the fading of each ring
                Color ringColor = new Color(0, 1, 0, alpha); // Green color with varying alpha

                Draw.Color = ringColor;
                Draw.Ring(position, dynamicRadius * i * 0.5f); // Increasing ring size
            }
        }
    }
}


/*
using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;


public class DynamicRadar : ImmediateModeShapeDrawer
{
    private float sphereRadius = 1f;
    private float xRange = 0f;
    private float yRange = 0f;
    private float dynamicRadius = 1f;

    //this will get updated!
    private int radarRingCount = 1;

    public void UpdateSoundData(object[] soundData)
    {
        if (soundData.Length >= 3)
        {
            string classification = soundData[0] as string;
            float pitch = (float)soundData[1];
            float distance = (float)soundData[2];

            // ======= normalizing the pitch to control the number of radar rings (min 1, max 5) ======= 
            // mapping pitch values (bet. 200 to 4000 Hz) to values between 1 and 5
            float mappedPitchValues = Mathf.Lerp(1f, 5f, Mathf.InverseLerp(200, 4000, pitch));
            // mapping mappedPitchValues for radarRingCount (bet. 1 to 5)
            radarRingCount = Mathf.Clamp(Mathf.FloorToInt(mappedPitchValues), 1, 5);

            // size of radius also changing based on volume!
            // dynamicRadius = sphereRadius * Mathf.Clamp(1f / (distance + 1f), 0.5f, 1f);

            Debug.Log($"Pitch: {pitch}, Radar Rings: {radarRingCount}");
        }
    }

    public override void DrawShapes(Camera cam)
    {
        using (Draw.Command(cam))
        {
            Draw.Color = Color.green;
            Draw.LineGeometry = LineGeometry.Volumetric3D;
            Draw.ThicknessSpace = ThicknessSpace.Pixels;
            Draw.Thickness = 3;

            Vector3 position = new Vector3(xRange, yRange, 0);

            // Draw dynamic rings based on radarRingCount
            for (int i = 1; i <= radarRingCount; i++)
            {
                Draw.Ring(position, dynamicRadius * i * 0.5f); // Increasing ring size
            }
        }
    }
}
*/