using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

public class Radar : ImmediateModeShapeDrawer
{
    private bool shouldDraw = false;

    public void ToggleDrawing(bool enable)
    {
        shouldDraw = enable;
    }
    

    public override void DrawShapes(Camera cam)
    {
        using (Draw.Command(cam))
        {
            Draw.LineGeometry = LineGeometry.Volumetric3D;
            Draw.ThicknessSpace = ThicknessSpace.Pixels;
            Draw.Thickness = 3;
            Draw.Color = Color.green;

            //ORDER: position - radius - start angle - end angle
            Draw.Ring(
                0.5f
            );

            Draw.Ring(
                1f
            );

            Draw.Ring(
                1.5f
            );

            Draw.Ring(
                2f
            );

        }
    }
}
