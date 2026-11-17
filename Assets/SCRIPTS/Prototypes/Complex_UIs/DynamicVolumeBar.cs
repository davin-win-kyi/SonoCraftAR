using UnityEngine;
using Shapes;

public class DynamicVolumeBar : ImmediateModeShapeDrawer
{
    [Range(0, 1)]
    public float fillAmount = 1;
    public Gradient colorGradient;
    public string title = "Volume Bar";

    // this controls how quickly the animation interpolates
    private float smoothSpeed = 5f;

    private float targetFillAmount = 1f;

    public void UpdateSoundData(object[] soundData)
    {
        if (soundData.Length >= 3)
        {
            string classification = soundData[0] as string;
            float pitch = (float)soundData[1];
            float distance = (float)soundData[2];

            float rawVolume = Mathf.Clamp01(1f / (distance + 1f)); //acc to gpt, volume is 1/(distance + 1)

            targetFillAmount = rawVolume;

            Debug.Log($"Raw volume: {rawVolume}");
        }
    }

    private void Update()
    {
        // interpolating`fillAmount` towards `targetFillAmount`!!!! (this is prob the key to smoother animations!!)
        fillAmount = Mathf.Lerp(fillAmount, targetFillAmount, Time.deltaTime * smoothSpeed);
    }
    public override void DrawShapes(Camera cam)
    {
        using (Draw.Command(cam))
        {
            // making sure the gradient is initialized
            if (colorGradient == null)
            {
                colorGradient = CreateGradient();
                Debug.Log("Created new gradient!");
            }


            Rect rect = new Rect(0, 0, 8, 1);

            // black background
            Draw.Rectangle(rect, 0.3f, Color.black);

            // the colored bar
            Rect fillRect = Inset(rect, 1);
            fillRect.width *= fillAmount;
            Draw.Rectangle(fillRect, colorGradient.Evaluate(fillAmount));

            // Draw the title
            Draw.FontSize = 10;
            Vector2 topLeft = new Vector2(rect.xMin, rect.yMax + 1f);
            Draw.Text(topLeft, title, TextAlign.BaselineLeft);
        }
    }

    Rect Inset(Rect r, float amount)
    {
        return new Rect(r.x + 0.25f, r.y + 0.25f, r.width - amount * 0.5f, r.height - amount * 0.5f);
    }

    Gradient CreateGradient()
    {
        Gradient gradient = new Gradient();

        // Set the color keys
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0].color = Color.yellow; // Start color
        colorKeys[0].time = 0.0f; // At the start of the gradient
        colorKeys[1].color = Color.cyan; // End color
        colorKeys[1].time = 1.0f; // At the end of the gradient

        // Set the alpha keys
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = 1.0f; // Fully opaque at the start
        alphaKeys[0].time = 0.0f;
        alphaKeys[1].alpha = 1.0f; // Fully opaque at the end
        alphaKeys[1].time = 1.0f;

        // Assign the keys to the gradient
        gradient.SetKeys(colorKeys, alphaKeys);

        return gradient;
    }

}
