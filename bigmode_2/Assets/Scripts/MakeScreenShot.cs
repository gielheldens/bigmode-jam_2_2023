using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Unity.VisualScripting;

public class MakeScreenShot : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] Image whereToSHowScreenShot;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(TakeScreenShotaAndShow());
        }
    }

    private IEnumerator TakeScreenShotaAndShow()
    {

        yield return new WaitForEndOfFrame();

        Texture2D screenshot = ScreenCapture.CaptureScreenshotAsTexture();

        Texture2D newScreenshot = new Texture2D(screenshot.width, screenshot.height, TextureFormat.RGB24, false);
        newScreenshot.SetPixels(screenshot.GetPixels());
        newScreenshot.Apply();

        Destroy(screenshot);

        Sprite screenshotSprite = Sprite.Create(newScreenshot, new Rect(0, 0, newScreenshot.width, newScreenshot.height), new Vector2(0.5f, 0.5f));

        whereToSHowScreenShot.enabled = true;
        whereToSHowScreenShot.sprite = screenshotSprite;
        SaveScreenshot(newScreenshot);
    }

    private void SaveScreenshot(Texture2D screenshot)
    {
        string fileName = Path.GetRandomFileName() + ".png";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        byte[] pngShot = screenshot.EncodeToPNG();

        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        File.WriteAllBytes(filePath, pngShot);

        Debug.Log("screenshot save to " + filePath);
    }
}
