using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

public class Pulse : ImmediateModeShapeDrawer
{
    public override void DrawShapes(Camera cam)
    {
        using (Draw.Command(cam))
        {
            Draw.LineGeometry = LineGeometry.Volumetric3D;
            Draw.ThicknessSpace = ThicknessSpace.Pixels;
            Draw.Thickness = 4;
            Draw.Color = Color.yellow;

            //ORDER: position - radius - start angle - end angle
            Draw.Arc(
                Vector3.zero,
                0.1f,
                Mathf.PI * 0.75f,
                Mathf.PI * 0.25f
            ); 
            
            Draw.Arc(
                Vector3.zero + new Vector3(0, 0.1f, 0),
                1f,
                Mathf.PI * 0.75f,
                Mathf.PI * 0.25f
            );

            Draw.Arc(
                Vector3.zero + new Vector3(0, 0.2f, 0),
                2f,
                Mathf.PI * 0.75f,
                Mathf.PI * 0.25f
            );

            Draw.Arc(
                Vector3.zero + new Vector3(0, 0.3f, 0),
                3f,
                Mathf.PI * 0.75f,
                Mathf.PI * 0.25f
            );
        }

    }
}
