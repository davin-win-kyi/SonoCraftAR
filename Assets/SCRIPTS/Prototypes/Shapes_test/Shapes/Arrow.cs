using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

public class Arrow : ImmediateModeShapeDrawer
{
    public override void DrawShapes(Camera cam)
    {
        using (Draw.Command(cam))
        {
            Draw.Triangle(new Vector3(-1, 2, 0), new Vector3(0, 4, 0), new Vector3(1,2,0));
            Draw.Rectangle(new Vector3(0, 0, 0), 1, 4);
        }
    }
}
