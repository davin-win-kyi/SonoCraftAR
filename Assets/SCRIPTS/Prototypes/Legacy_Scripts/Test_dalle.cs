using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Test : MonoBehaviour
{
    // File path to the texture (can be a file on your system or relative to Resources folder)
    public string texturePath = "Assets/Resources/MyTexture.png"; // Replace with your texture file path

    public Sprite img1;

    void Start()
    {
        Debug.Log("Test script started");
        GeneratedFunction();
    }

    public void GeneratedFunction()
    {
        // Load the sprite from the Resources folder
        img1 = Resources.Load<Sprite>("bird");

        if (img1 == null)
        {
            Debug.LogError("Failed to load sprite. Ensure the file is in the Resources folder and the path is correct.");
            return;
        }

        // Dynamically create a GameObject for the image
        GameObject imageObject = new GameObject("DynamicImage");
        imageObject.AddComponent<Canvas>(); // Create a Canvas if needed
        Canvas canvas = imageObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Create the Image UI component
        GameObject uiImage = new GameObject("UIImage");
        uiImage.transform.SetParent(imageObject.transform);
        RectTransform rectTransform = uiImage.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(200, 200); // Set size of the image
        rectTransform.anchoredPosition = Vector2.zero; // Center the image on the canvas

        Image imageComponent = uiImage.AddComponent<Image>();
        imageComponent.sprite = img1; // Assign the loaded sprite to the Image component

        Debug.Log("Image successfully loaded and displayed.");
    }
}




/*
using System;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadImageToRawImage : MonoBehaviour
{
    void Start()
    {
        // Replace with your image URL
        GeneratedFunction();
    }

    void GeneratedFunction()
    {
        string filePath = "/Users/davinwinkyi/Documents/Sound-UI-Authoring/Assets/Scripts/Roslyn_test/bird.png";

        if (!File.Exists(filePath))
        {
            Debug.LogError("File does not exist at the specified path: " + filePath);
            return;
        }

        try
        {
            // Read image data from the local file
            byte[] imageBytes = File.ReadAllBytes(filePath);

            // Create a Texture2D and load the image
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);

            // Create a Canvas if one doesn't exist
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObject = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                canvas = canvasObject.GetComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;

                // Optional: Adjust CanvasScaler for better scaling
                CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080);
            }

            // Create the RawImage
            GameObject rawImageObject = new GameObject("DynamicRawImage", typeof(RectTransform), typeof(RawImage));
            rawImageObject.transform.SetParent(canvas.transform);

            // Set up the RectTransform
            RectTransform rectTransform = rawImageObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(100, 100); // Set size (e.g., 100x100 pixels)
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = Vector2.zero; // Center in the screen

            // Apply the texture to the RawImage
            RawImage rawImage = rawImageObject.GetComponent<RawImage>();
            rawImage.texture = texture;

            // Comment out SetNativeSize to keep the specified size
            // rawImage.SetNativeSize();

            Debug.Log("Image successfully loaded and displayed.");
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading image: " + e.Message);
        }
    }
}
*/



/*
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
    private RawImage rawImage;

    public void Start() {
        GeneratedFunction();
    }

    // Function to set up and load the image
    public void GeneratedFunction()
    {
        SetupRawImage();
        string imageUrl = "https://oaidalleapiprodscus.blob.core.windows.net/private/org-YpLKY7JUYstAfhiJPlyDLOCL/user-taXxYOIhd9SvGG2Xeop9i3jr/img-aSucZ7AZMqPD5K5DiY12ZuUs.png?st=2024-11-19T05%3A44%3A49Z&se=2024-11-19T07%3A44%3A49Z&sp=r&sv=2024-08-04&sr=b&rscd=inline&rsct=image/png&skoid=d505667d-d6c1-4a0a-bac7-5c84a87759f8&sktid=a48cca56-e6da-484e-a814-9c849652bcb3&skt=2024-11-18T19%3A16%3A32Z&ske=2024-11-19T19%3A16%3A32Z&sks=b&skv=2024-08-04&sig=4pgbUmujzAyz71O633eZqqUBZylNCiw0zVHtoc0nVVM%3D";
        StartCoroutine(LoadImageFromURL(imageUrl));
    }

    // Dynamically create a RawImage at runtime
    private void SetupRawImage()
    {
        // Create a Canvas if one doesn't already exist
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObject = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        // Create a RawImage GameObject
        GameObject rawImageObject = new GameObject("DynamicRawImage", typeof(RectTransform), typeof(RawImage));
        rawImageObject.transform.SetParent(canvas.transform);

        // Set up the RectTransform to center the image and set a fixed size
        RectTransform rectTransform = rawImageObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(100, 100); // Fixed size (300x300 pixels)
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero; // Center on the screen

        // Assign the RawImage component
        rawImage = rawImageObject.GetComponent<RawImage>();
    }

    // Coroutine to fetch and display the image
    private IEnumerator LoadImageFromURL(string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            rawImage.texture = texture;

            // Scale the image to fit within the sizeDelta, maintaining aspect ratio
            AdjustImageSize(texture);
        }
    }

    // Adjust the size of the RawImage to maintain the aspect ratio
    private void AdjustImageSize(Texture2D texture)
    {
        float imageAspect = (float)texture.width / texture.height;
        float containerAspect = rawImage.rectTransform.sizeDelta.x / rawImage.rectTransform.sizeDelta.y;

        if (imageAspect > containerAspect)
        {
            // Wider than the container: adjust height to fit
            rawImage.rectTransform.sizeDelta = new Vector2(rawImage.rectTransform.sizeDelta.x, rawImage.rectTransform.sizeDelta.x / imageAspect);
        }
        else
        {
            // Taller than the container: adjust width to fit
            rawImage.rectTransform.sizeDelta = new Vector2(rawImage.rectTransform.sizeDelta.y * imageAspect, rawImage.rectTransform.sizeDelta.y);
        }
    }
}
*/






/*
using UnityEngine;

public class RedSquareGenerator : MonoBehaviour
{
    public void GeneratedFunction()
    {
        // Create a new GameObject
        GameObject redSquare = new GameObject("RedSquare");

        // Add a RectTransform component
        RectTransform rectTransform = redSquare.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(100, 100); // Set the size of the square
        rectTransform.localPosition = Vector3.zero; // Center it in the scene

        // Add a CanvasRenderer (required for UI rendering)
        redSquare.AddComponent<CanvasRenderer>();

        // Add an Image component and set its color to red
        UnityEngine.UI.Image image = redSquare.AddComponent<UnityEngine.UI.Image>();
        image.color = Color.red;

        // Ensure the square is part of a Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            // Create a new Canvas if none exists
            GameObject canvasObject = new GameObject("Canvas");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvasObject.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        }

        // Set the square as a child of the Canvas
        redSquare.transform.SetParent(canvas.transform, false);
    }
}
*/