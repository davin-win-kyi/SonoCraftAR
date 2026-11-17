using Shapes;
using System.Collections;
using UnityEngine;

public class SphereWave : ImmediateModeShapeDrawer
{
    private bool shouldDraw = false;
    private float[] sphereHeights; // Store dynamic heights for spheres
    private const int numSpheres = 10; // Number of spheres
    private float time = 0f;

    private void Awake()
    {
        sphereHeights = new float[numSpheres];
        StartCoroutine(AnimateSpheres());
    }

    // Toggle drawing
    public void ToggleDrawing(bool enable)
    {
        shouldDraw = enable;
    }

    // Coroutine to animate sphere heights
    private IEnumerator AnimateSpheres()
    {
        while (true) 
        {
            time += Time.deltaTime;
            float offset = Mathf.Sin(time); // Simple consistent oscillation
            for (int i = 0; i < numSpheres; i++)
            {
                // Alternate heights: even indices move up, odd indices move down
                sphereHeights[i] = (i % 2 == 0 ? offset : -offset);
            }
            yield return null; // Wait for the next frame
        }
    }

    // Draw shapes
    public override void DrawShapes(Camera cam)
    {
        if (!shouldDraw) return;

        using (Draw.Command(cam))
        {
            for (int i = 0; i < numSpheres; i++)
            {
                float x = -5 + i; // Map index to x positions
                Draw.Sphere(new Vector3(x, sphereHeights[i], 0), 0.5f);
            }
        }
    }
}
