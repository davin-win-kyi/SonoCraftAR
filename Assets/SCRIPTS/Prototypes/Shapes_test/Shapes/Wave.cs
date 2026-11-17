using UnityEngine;
using Shapes;

[ExecuteAlways] // Allows visualization in the editor
public class WavePolyline : ImmediateModeShapeDrawer
{
    [Header("Wave Parameters")]
    public float waveAmplitude = 2f; // Height of crests and troughs
    public float waveLength = 2f; // Distance between crests
    public int numWaves = 5; // Number of crests and troughs
    public int pointsPerWave = 20; // Number of points per wave
    public float lineThickness = 0.2f; // Thickness of the polyline
    public Color lineColor = Color.cyan; // Color of the wave
    public bool closedLoop = false; // Whether the polyline is closed

    public override void DrawShapes(Camera cam)
    {
        using (Draw.Command(cam))
        {
            // Configure polyline settings
            Draw.Thickness = lineThickness;
            Draw.Color = lineColor;
            Draw.LineGeometry = LineGeometry.Billboard; // Billboard polyline
            Draw.PolylineJoins = PolylineJoins.Round; // Smooth joins

            // Generate wave points
            PolylinePath path = new();

            path.AddPoint(-3, -0.5f);
            path.AddPoint(-2, 0.5f);
            path.AddPoint(-1, -0.5f);
            path.AddPoint(0, 0.5f);
            path.AddPoint(1, -0.5f);
            path.AddPoint(2, 0.5f);
            path.AddPoint(3, -0.5f);

            // Draw the polyline
            Draw.Polyline(path, closedLoop);
        }
    }
}





// using System.Collections;
// using System.Collections.Generic;
// using Shapes;
// using UnityEngine;

// public class Wave : ImmediateModeShapeDrawer
// {
//     private bool shouldDraw = false;

//     public void ToggleDrawing(bool enable)
//     {
//         shouldDraw = enable;
//     }
    

//     public override void DrawShapes(Camera cam)
//     {
//         using (Draw.Command(cam))
//         {
//             Draw.LineGeometry = LineGeometry.Volumetric3D;
//             Draw.ThicknessSpace = ThicknessSpace.Pixels;
//             Draw.Thickness = 3;
//             Draw.Color = Color.red;
//             Draw.PolylineJoins = PolylineJoins.Round;
//             Draw.PolylineGeometry = PolylineGeometry.Billboard;


//             //ORDER: position - radius - start angle - end angle
//             PolylinePath path = new();
//             path.AddPoint(-2, 0.25f);
//             path.AddPoint(-1, -0.25f);
//             path.AddPoint(0, 0.25f);
//             path.AddPoint(1, -0.25f);
//             path.AddPoint(2, 0.25f);
//             Draw.Polyline(path, PolylineJoins.Round);
//         }
//     }
// }