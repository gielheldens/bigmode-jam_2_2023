using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Unity.VisualScripting;

public class MakeScreenShot : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] Image whereToSHowScreenShot;
    [SerializeField] private float _width;
    [SerializeField] private float _height;

    // private references
    private Canvas _canvas;

    private Start()
    {
        _canvas = GetComponent<Canvas>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(TakeScreenShotaAndShow(_canvas));
        }
    }

    // private IEnumerator TakeScreenShotaAndShow()
    // {

    //     yield return new WaitForEndOfFrame();

    //     Texture2D screenshot = ScreenCapture.CaptureScreenshotAsTexture();

    //     Texture2D newScreenshot = new Texture2D(screenshot.width, screenshot.height, TextureFormat.RGB24, false);
    //     newScreenshot.SetPixels(screenshot.GetPixels());
    //     newScreenshot.Apply();

    //     Destroy(screenshot);

    //     Sprite screenshotSprite = Sprite.Create(newScreenshot, new Rect(0, 0, newScreenshot.width, newScreenshot.height), new Vector2(0.5f, 0.5f));

    //     whereToSHowScreenShot.enabled = true;
    //     whereToSHowScreenShot.sprite = screenshotSprite;
    // }

    private IEnumerator TakeScreenShotaAndShow(Canvas canvas)
    {
        yield return new WaitForEndOfFrame();

        // Create a RenderTexture
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        // Set the target texture of the camera to the RenderTexture
        Camera camera = canvas.worldCamera; // Assuming the canvas has a camera assigned
        camera.targetTexture = renderTexture;

        // Render the canvas to the RenderTexture
        camera.Render();

        // Create a new Texture2D and read the RenderTexture into it
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();

        // Reset the camera's target texture
        camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        // Save the Texture2D as a PNG file (optional)
        byte[] bytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes("CapturedCanvas.png", bytes);

        //Debug.Log("Canvas captured and saved as CapturedCanvas.png");
    }
}
