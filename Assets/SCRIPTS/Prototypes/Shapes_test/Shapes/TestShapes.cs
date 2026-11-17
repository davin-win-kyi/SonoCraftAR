using Unity;
using Shapes;
using UnityEngine;

[ExecuteAlways]
public class MyScript : ImmediateModeShapeDrawer
{
    public enum shape
    {
        circle,
        triangle,
        rectangle
    }

    [SerializeField] private shape selectedShape;
    public override void DrawShapes(Camera cam)
    {
        using (Draw.Command(cam))
        {
            Draw.LineGeometry = LineGeometry.Volumetric3D;
            Draw.ThicknessSpace = ThicknessSpace.Pixels;
            Draw.Thickness = 4; // 4px wide
            Draw.Color = Color.blue;

            switch (selectedShape)
            {
                case shape.circle:
                    Draw.Sphere(new Vector3(0,0,0), 3f);
                    break;
                case shape.triangle:
                    Draw.Triangle(new Vector3(0,0,0), new Vector3(4,0,0), new Vector3(2,4,0));
                    break;
                case shape.rectangle:
                    Draw.Rectangle(new Vector3(0, 0, 0), 4, 5, 0.4f);
                    break;
                    Draw.Text(new Vector3(0, 0, 0), "Hello World", TextAlign.Center, 24f);
            }
        }
    }
}