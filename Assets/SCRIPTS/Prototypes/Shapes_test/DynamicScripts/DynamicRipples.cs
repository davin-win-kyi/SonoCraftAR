using UnityEngine;
using Shapes;
using System.Collections.Generic;

[ExecuteAlways]
public class DynamicRipples : ImmediateModeShapeDrawer
{
    public Gradient colorGradient;
    private float lastPitch = -1f;
    private List<Ripple> ripples = new List<Ripple>();

    [System.Serializable]
    private class Ripple
    {
        public float radius;
        public float thickness;
        public Color colors;
        public float lifeTime;
        public float pitch;
        public Ripple(float radius, float thickness, Color colors, float lifeTime, float pitch)
        {
            this.radius = radius;
            this.thickness = thickness;
            this.colors = colors;
            this.lifeTime = lifeTime;
            this.pitch = pitch;
        }
    }

    private float pitchThreshold = 0.9f; // Minimum difference in pitch to trigger a new ripple
    private float rippleFadeTime = 1f; // Time for ripples to fade out
    private float rippleSpeed = 2f; // Speed at which ripples expand

    // Adds Ripple object to list
    public void UpdateSoundData(float pitch)
    {
        // Spawns ripple at y-position based on the pitch, only if the pitch has changed by a certain threshold
        if (lastPitch < 0f || Mathf.Abs(pitch - lastPitch) >= pitchThreshold)
        {
            // Add a new ripple
            ripples.Add(new Ripple(
                radius: 0f,
                thickness: 0.2f,
                colors: colorGradient.Evaluate(Random.value),
                lifeTime: rippleFadeTime,
                pitch: pitch
            ));

            lastPitch = pitch;
        }
    }

    public override void DrawShapes(Camera cam)
    {
        using (Draw.Command(cam))
        {
            // Update and animate each ripple in Ripples list
            for (int i = ripples.Count - 1; i >= 0; i--)
            {
                Ripple ripple = ripples[i];

                // Update ripple properties
                ripple.radius += rippleSpeed * Time.deltaTime;
                ripple.lifeTime -= Time.deltaTime;

                // Fade out ripple
                Color fadedColor = ripple.colors;
                fadedColor.a = Mathf.Clamp01(ripple.lifeTime / rippleFadeTime);

                // Draw ripple 
                Draw.Ring(
                    pos: Vector3.zero + new Vector3(0, Mathf.Clamp(ripple.pitch, 0, Screen.height), 0),
                    radius: ripple.radius,
                    thickness: ripple.thickness,
                    colors: fadedColor
                );

                // Remove ripple if it has faded out
                if (ripple.lifeTime <= 0f)
                {
                    ripples.RemoveAt(i);
                }
            }
        }
    }

    // hard coded mock data for testing dynamic animation
    private List<float> mockPitches = new List<float>
{
    0.000f, 0.006f, 0.012f, 0.018f, 0.024f, 0.030f, 0.036f, 0.042f, 0.048f, 0.054f,
    0.060f, 0.066f, 0.072f, 0.078f, 0.084f, 0.090f, 0.096f, 0.102f, 0.108f, 0.114f,
    0.120f, 0.126f, 0.132f, 0.138f, 0.144f, 0.150f, 0.156f, 0.162f, 0.168f, 0.174f,
    0.180f, 0.186f, 0.192f, 0.198f, 0.204f, 0.210f, 0.216f, 0.222f, 0.228f, 0.234f,
    0.240f, 0.246f, 0.252f, 0.258f, 0.264f, 0.270f, 0.276f, 0.282f, 0.288f, 0.294f,
    0.300f, 0.306f, 0.312f, 0.318f, 0.324f, 0.330f, 0.336f, 0.342f, 0.348f, 0.354f,
    0.360f, 0.366f, 0.372f, 0.378f, 0.384f, 0.390f, 0.396f, 0.402f, 0.408f, 0.414f,
    0.420f, 0.426f, 0.432f, 0.438f, 0.444f, 0.450f, 0.456f, 0.462f, 0.468f, 0.474f,
    0.480f, 0.486f, 0.492f, 0.498f, 0.504f, 0.510f, 0.516f, 0.522f, 0.528f, 0.534f,
    0.540f, 0.546f, 0.552f, 0.558f, 0.564f, 0.570f, 0.576f, 0.582f, 0.588f, 0.594f,
    0.600f, 0.606f, 0.612f, 0.618f, 0.624f, 0.630f, 0.636f, 0.642f, 0.648f, 0.654f,
    0.660f, 0.666f, 0.672f, 0.678f, 0.684f, 0.690f, 0.696f, 0.702f, 0.708f, 0.714f,
    0.720f, 0.726f, 0.732f, 0.738f, 0.744f, 0.750f, 0.756f, 0.762f, 0.768f, 0.774f,
    0.780f, 0.786f, 0.792f, 0.798f, 0.804f, 0.810f, 0.816f, 0.822f, 0.828f, 0.834f,
    0.840f, 0.846f, 0.852f, 0.858f, 0.864f, 0.870f, 0.876f, 0.882f, 0.888f, 0.894f,
    0.900f, 0.906f, 0.912f, 0.918f, 0.924f, 0.930f, 0.936f, 0.942f, 0.948f, 0.954f,
    0.960f, 0.966f, 0.972f, 0.978f, 0.984f, 0.990f, 0.996f, 1.002f, 1.008f, 2.014f,
    0.660f, 0.666f, 0.672f, 0.678f, 0.684f, 0.690f, 0.696f, 0.702f, 0.708f, 0.714f,
    0.720f, 0.726f, 0.732f, 0.738f, 0.744f, 0.750f, 0.756f, 0.762f, 0.768f, 0.774f,
    0.780f, 0.786f, 0.792f, 0.798f, 0.804f, 0.810f, 0.816f, 0.822f, 0.828f, 0.834f,
    0.840f, 0.846f, 0.852f, 0.858f, 0.864f, 0.870f, 0.876f, 0.882f, 0.888f, 0.894f,
    0.900f, 0.906f, 0.912f, 0.918f, 0.924f, 0.930f, 0.936f, 0.942f, 0.948f, 0.954f,
    0.960f, 0.966f, 0.972f, 0.978f, 0.984f, 0.990f, 0.996f, 1.002f, 1.008f, 5.014f
};

    int ind = 0; 
    private void Update()
    {
        if (ind >= mockPitches.Count) ind = 0;
        UpdateSoundData(mockPitches[ind]);
        ind++;
    }
}
