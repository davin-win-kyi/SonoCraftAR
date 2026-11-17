using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

public class DynamicCircle : ImmediateModeShapeDrawer
{

    [SerializeField]
    [Range(1f, 10f)] private float sphereRadius = 1f;

    [SerializeField]
    [Range(-5f, 5f)] private float xRange = 0f;

    [SerializeField]
    [Range(-5f, 5f)] private float yRange = 0f;


    //we need this to have the square drawn continuously!!
    public override void DrawShapes(Camera cam)
    {
       
        using (Draw.Command(cam))
        {

            Draw.Color = Color.white;
            Draw.LineGeometry = LineGeometry.Volumetric3D;
            Draw.ThicknessSpace = ThicknessSpace.Pixels;
            Draw.Thickness = 4;

            Vector3 position = new Vector3(xRange, yRange, 0);

            Draw.Sphere(position, sphereRadius);
        }
        
    }

}
