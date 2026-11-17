using System.Collections;
using Shapes;
using UnityEngine;

public class RoslynShapeTest : ImmediateModeShapeDrawer
{
    private bool shouldDraw = false; // Controls whether the shape is drawn
    private Vector3 squareSize = new Vector3(4, 4, 4); // Initial square size
    private Coroutine drawShapesCoroutine; // Reference to the coroutine

    public void ToggleDrawing(bool enable)
    {
        shouldDraw = enable;

        if (shouldDraw)
        {
            // Start the coroutine if it's not running
            if (drawShapesCoroutine == null)
            {
                drawShapesCoroutine = StartCoroutine(DrawShapesCoroutine());
            }
        }
        else
        {
            // Stop the coroutine if it's running
            if (drawShapesCoroutine != null)
            {
                StopCoroutine(drawShapesCoroutine);
                drawShapesCoroutine = null;
            }
        }
    }

    public override void DrawShapes(Camera cam)
    {
        if (shouldDraw)
        {
            using (Draw.Command(cam))
            {
                Draw.Cuboid(Vector3.zero, squareSize);

                Draw.Color = Color.white;
                Draw.LineGeometry = LineGeometry.Volumetric3D;
                Draw.ThicknessSpace = ThicknessSpace.Pixels;
                Draw.Thickness = 4;
            }
        }
    }

    private IEnumerator DrawShapesCoroutine()
    {
        while (shouldDraw)
        {
            GenerateShapes();
            yield return new WaitForSeconds(0.5f); // Adjust for desired speed
        }
    }

    private void GenerateShapes()
    {
        // Randomize the size of the square
        float randomSize = Random.Range(1f, 3f); // Adjust range as needed
        squareSize = new Vector3(randomSize, randomSize, randomSize);
    }
}






// using System.Collections;
// using System.Collections.Generic;
// using Shapes;
// using UnityEngine;

// public class RoslynShapeTest : ImmediateModeShapeDrawer
// {
//     private bool shouldDraw = false;

//     public void ToggleDrawing(bool enable)
//     {
//         shouldDraw = enable;
//     }

//     //we need this to have the square drawn continuously!!
//     public override void DrawShapes(Camera cam)
//     {
//         // read from a file, and then we will
//         // use the SoundData from the file to 
//         // modify the shape that we will be 
//         // displaying to the user

//         if (shouldDraw)
//         {
//             using (Draw.Command(cam))
//             {
//                 Draw.Cuboid(Vector3.zero, new Vector3(4, 4, 4));

//                 Draw.Color = Color.white;
//                 Draw.LineGeometry = LineGeometry.Volumetric3D;
//                 Draw.ThicknessSpace = ThicknessSpace.Pixels;
//                 Draw.Thickness = 4;
//             }
//         }
//     }

// }
