using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SmoothBarController : MonoBehaviour
{
    private RectTransform barTransform;
    private float targetHeight = 100f;
    private float currentHeight = 100f;
    private float minHeight = 50f;
    private float maxHeight = 300f;
    private float transitionSpeed = 2f;

    private bool increasing = true;

    void Start()
    {
        // Create Canvas
        GameObject canvasObject = new GameObject("Canvas", typeof(Canvas));
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        // Create Bar Object
        GameObject barObject = new GameObject("Bar");
        barObject.transform.SetParent(canvasObject.transform);

        // Add Image Component for Bar
        Image barImage = barObject.AddComponent<Image>();
        barImage.color = Color.blue;

        // Set Rounded Edges (requires a sprite with rounded edges)
        barImage.sprite = GenerateRoundedSprite();
        barImage.type = Image.Type.Sliced;

        // Configure RectTransform for the Bar
        barTransform = barObject.GetComponent<RectTransform>();
        barTransform.anchorMin = new Vector2(0.5f, 0.5f);
        barTransform.anchorMax = new Vector2(0.5f, 0.5f);
        barTransform.pivot = new Vector2(0.5f, 0.5f);
        barTransform.sizeDelta = new Vector2(100, currentHeight);
        barTransform.anchoredPosition = Vector2.zero; // Centered on screen
    }

    void Update()
    {
        // Smoothly update bar height
        currentHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * transitionSpeed);
        barTransform.sizeDelta = new Vector2(100, currentHeight);

        // Adjust targetHeight for smooth oscillation
        if (Mathf.Abs(currentHeight - targetHeight) < 1f)
        {
            if (increasing)
                targetHeight = maxHeight;
            else
                targetHeight = minHeight;

            increasing = !increasing;
        }
    }

    private Sprite GenerateRoundedSprite()
    {
        // Create a high-resolution rounded sprite
        int resolution = 256; // Increased resolution for smoother edges
        Texture2D texture = new Texture2D(resolution, resolution, TextureFormat.ARGB32, false);
        texture.filterMode = FilterMode.Bilinear;

        Vector2 center = new Vector2(resolution / 2f, resolution / 2f);
        float radius = resolution / 2f;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                if (distance < radius)
                {
                    texture.SetPixel(x, y, Color.white);
                }
                else
                {
                    texture.SetPixel(x, y, Color.clear);
                }
            }
        }

        texture.Apply();

        Rect rect = new Rect(0, 0, texture.width, texture.height);
        return Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), resolution);
    }
}








// ******* This one is not rounded ******* //

// using System.Collections;
// using UnityEngine;
// using UnityEngine.UI;

// public class SmoothBarController : MonoBehaviour
// {
//     private RectTransform barTransform;
//     private float targetHeight = 100f;
//     private float currentHeight = 100f;
//     private float minHeight = 50f;
//     private float maxHeight = 300f;
//     private float transitionSpeed = 2f;

//     private bool increasing = true;

//     void Start()
//     {
//         // Create Canvas
//         GameObject canvasObject = new GameObject("Canvas", typeof(Canvas));
//         Canvas canvas = canvasObject.GetComponent<Canvas>();
//         canvas.renderMode = RenderMode.ScreenSpaceOverlay;
//         canvasObject.AddComponent<CanvasScaler>();
//         canvasObject.AddComponent<GraphicRaycaster>();

//         // Create Bar Object
//         GameObject barObject = new GameObject("Bar");
//         barObject.transform.SetParent(canvasObject.transform);

//         // Add Image Component for Bar
//         Image barImage = barObject.AddComponent<Image>();
//         barImage.color = Color.blue;

//         // Set Rounded Edges (requires a sprite with rounded edges)
//         barImage.sprite = GenerateRoundedSprite();
//         barImage.type = Image.Type.Sliced;

//         // Configure RectTransform for the Bar
//         barTransform = barObject.GetComponent<RectTransform>();
//         barTransform.anchorMin = new Vector2(0.5f, 0.5f);
//         barTransform.anchorMax = new Vector2(0.5f, 0.5f);
//         barTransform.pivot = new Vector2(0.5f, 0.5f);
//         barTransform.sizeDelta = new Vector2(100, currentHeight);
//         barTransform.anchoredPosition = Vector2.zero; // Centered on screen
//     }

//     void Update()
//     {
//         // Smoothly update bar height
//         currentHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * transitionSpeed);
//         barTransform.sizeDelta = new Vector2(100, currentHeight);

//         // Adjust targetHeight for smooth oscillation
//         if (Mathf.Abs(currentHeight - targetHeight) < 1f)
//         {
//             if (increasing)
//                 targetHeight = maxHeight;
//             else
//                 targetHeight = minHeight;

//             increasing = !increasing;
//         }
//     }

//     private Sprite GenerateRoundedSprite()
//     {
//         // Creates a simple rounded sprite
//         Texture2D texture = new Texture2D(32, 32, TextureFormat.ARGB32, false);
//         texture.filterMode = FilterMode.Bilinear;

//         for (int y = 0; y < texture.height; y++)
//         {
//             for (int x = 0; x < texture.width; x++)
//             {
//                 float distance = Vector2.Distance(new Vector2(x, y), new Vector2(16, 16));
//                 if (distance < 16)
//                 {
//                     texture.SetPixel(x, y, Color.white);
//                 }
//                 else
//                 {
//                     texture.SetPixel(x, y, Color.clear);
//                 }
//             }
//         }

//         texture.Apply();

//         Rect rect = new Rect(0, 0, texture.width, texture.height);
//         return Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 100);
//     }
// }










// using UnityEngine;

// public class SmoothBarControllerNoUI : MonoBehaviour
// {
//     public float MinHeight = 1f; // Minimum height of the bar
//     public float MaxHeight = 5f; // Maximum height of the bar
//     public float Width = 1f; // Width of the bar
//     public float Speed = 2f; // Speed of the height change
//     public Color BarColor = Color.green; // Color of the bar

//     private GameObject bar;
//     private float time;

//     void Start()
//     {
//         // Create a GameObject for the bar
//         bar = GameObject.CreatePrimitive(PrimitiveType.Cube);

//         // Set the initial size and position of the bar
//         bar.transform.localScale = new Vector3(Width, MinHeight, 1f);
//         bar.transform.position = new Vector3(0, MinHeight / 2f, 0);

//         // Set the color of the bar
//         var renderer = bar.GetComponent<Renderer>();
//         if (renderer != null)
//         {
//             renderer.material.color = BarColor;
//         }
//     }

//     void Update()
//     {
//         // Update the time variable for smooth oscillation
//         time += Time.deltaTime * Speed;

//         // Calculate new height using a sine wave
//         float newHeight = Mathf.Lerp(MinHeight, MaxHeight, (Mathf.Sin(time) + 1) / 2);

//         // Update the bar's size
//         bar.transform.localScale = new Vector3(Width, newHeight, 1f);

//         // Update the bar's position to keep it centered on the bottom
//         bar.transform.position = new Vector3(0, newHeight / 2f, 0);
//     }
// }
