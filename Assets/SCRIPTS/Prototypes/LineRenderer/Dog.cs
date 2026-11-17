using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DogShapeRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;

    // Define the dog's shape with 100+ points
    private Vector3[] dogShape = new Vector3[]
    {
        // Head and face
        new Vector3(-1.5f, 2.0f, 0), new Vector3(-1.4f, 2.2f, 0), new Vector3(-1.3f, 2.3f, 0),
        new Vector3(-1.2f, 2.35f, 0), new Vector3(-1.1f, 2.4f, 0), new Vector3(-1.0f, 2.38f, 0),
        new Vector3(-0.9f, 2.35f, 0), new Vector3(-0.8f, 2.3f, 0), new Vector3(-0.7f, 2.2f, 0),
        new Vector3(-0.6f, 2.1f, 0), new Vector3(-0.5f, 2.0f, 0), new Vector3(-0.4f, 1.9f, 0),
        new Vector3(-0.5f, 1.8f, 0), new Vector3(-0.6f, 1.7f, 0), new Vector3(-0.7f, 1.65f, 0),
        new Vector3(-0.8f, 1.6f, 0), new Vector3(-0.9f, 1.55f, 0), new Vector3(-1.0f, 1.5f, 0),

        // Neck
        new Vector3(-1.1f, 1.4f, 0), new Vector3(-1.2f, 1.3f, 0), new Vector3(-1.3f, 1.2f, 0),
        new Vector3(-1.4f, 1.1f, 0),

        // Body (top)
        new Vector3(-1.3f, 1.0f, 0), new Vector3(-1.2f, 0.9f, 0), new Vector3(-1.0f, 0.8f, 0),
        new Vector3(-0.8f, 0.75f, 0), new Vector3(-0.6f, 0.7f, 0), new Vector3(-0.4f, 0.68f, 0),
        new Vector3(-0.2f, 0.65f, 0), new Vector3(0.0f, 0.6f, 0), new Vector3(0.2f, 0.58f, 0),
        new Vector3(0.4f, 0.55f, 0), new Vector3(0.6f, 0.53f, 0), new Vector3(0.8f, 0.5f, 0),

        // Tail
        new Vector3(1.0f, 0.6f, 0), new Vector3(1.2f, 0.7f, 0), new Vector3(1.4f, 0.8f, 0),
        new Vector3(1.6f, 0.9f, 0), new Vector3(1.7f, 1.0f, 0), new Vector3(1.8f, 1.1f, 0),
        new Vector3(1.7f, 1.2f, 0), new Vector3(1.6f, 1.3f, 0), new Vector3(1.4f, 1.2f, 0),
        new Vector3(1.2f, 1.1f, 0), new Vector3(1.0f, 1.0f, 0),

        // Back leg
        new Vector3(0.9f, 0.8f, 0), new Vector3(0.8f, 0.6f, 0), new Vector3(0.7f, 0.4f, 0),
        new Vector3(0.6f, 0.2f, 0), new Vector3(0.5f, 0.0f, 0), new Vector3(0.4f, -0.2f, 0),
        new Vector3(0.3f, -0.4f, 0), new Vector3(0.2f, -0.6f, 0), new Vector3(0.1f, -0.8f, 0),

        // Back paw
        new Vector3(0.0f, -0.9f, 0), new Vector3(-0.1f, -0.85f, 0), new Vector3(-0.2f, -0.8f, 0),
        new Vector3(-0.3f, -0.7f, 0), new Vector3(-0.4f, -0.6f, 0),

        // Body bottom
        new Vector3(-0.6f, -0.5f, 0), new Vector3(-0.8f, -0.4f, 0), new Vector3(-1.0f, -0.3f, 0),
        new Vector3(-1.2f, -0.2f, 0), new Vector3(-1.4f, -0.1f, 0),

        // Front leg
        new Vector3(-1.6f, -0.3f, 0), new Vector3(-1.7f, -0.5f, 0), new Vector3(-1.8f, -0.7f, 0),
        new Vector3(-1.9f, -0.9f, 0),

        // Front paw
        new Vector3(-2.0f, -1.0f, 0), new Vector3(-1.8f, -0.9f, 0), new Vector3(-1.6f, -0.8f, 0),

        // Close to body
        new Vector3(-1.4f, -0.6f, 0), new Vector3(-1.2f, -0.4f, 0), new Vector3(-1.0f, -0.2f, 0),
        new Vector3(-0.8f, 0.0f, 0),

        // Neck and head back
        new Vector3(-1.0f, 0.5f, 0), new Vector3(-1.2f, 1.0f, 0), new Vector3(-1.5f, 1.5f, 0),
        new Vector3(-1.5f, 2.0f, 0),
    };

    void Start()
    {
        // Initialize the LineRenderer
        lineRenderer = GetComponent<LineRenderer>();

        // Set LineRenderer properties
        lineRenderer.positionCount = dogShape.Length;
        lineRenderer.widthMultiplier = 0.1f; // Adjust line thickness
        lineRenderer.loop = true; // Ensures the line is closed

        // Assign the shape points to the LineRenderer
        lineRenderer.SetPositions(dogShape);
    }
}
