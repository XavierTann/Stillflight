using System.IO;
using UnityEngine;

public class ScreenshotCapture : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) // Capture screenshot when 'S' is pressed
        {
            CaptureScreenshot();
        }
    }

    void CaptureScreenshot()
    {
        // Get the path to persistent data folder
        string folderPath = Path.Combine(Application.persistentDataPath, "BirdGallery");

        // Create the folder if it doesn't exist
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Define the screenshot file path (with timestamp)
        string filePath = Path.Combine(
            folderPath,
            "screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png"
        );

        // Capture and save the screenshot
        ScreenCapture.CaptureScreenshot(filePath);
        Debug.Log("Screenshot saved to: " + filePath);
    }
}
