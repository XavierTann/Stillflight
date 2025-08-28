using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference rightTriggerAction; // Assign in Inspector

    [Header("Camera & VFX")]
    public Camera captureCamera; // Assign your VR main camera (falls back to Camera.main)

    private CameraSnapEffect snapEffect;

    private void Awake()
    {
        if (captureCamera == null)
            captureCamera = Camera.main;
        if (snapEffect == null)
            snapEffect = FindFirstObjectByType<CameraSnapEffect>();
    }

    private void OnEnable()
    {
        if (rightTriggerAction != null)
        {
            rightTriggerAction.action.performed += TakePhoto;
            rightTriggerAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (rightTriggerAction != null)
        {
            rightTriggerAction.action.performed -= TakePhoto;
            rightTriggerAction.action.Disable();
        }
    }

    private void TakePhoto(InputAction.CallbackContext context)
    {
        // 1) Make sure CameraMode exists
        if (CameraMode.Instance == null)
        {
            Debug.LogWarning("CameraSnap: No CameraMode.Instance found. Cannot check camera mode.");
            return;
        }

        // 2) Only allow snap if camera mode is ON
        if (!CameraMode.Instance.cameraMode)
        {
            Debug.Log("CameraSnap: Snap ignored — not in camera mode.");
            return;
        }

        // 3) Proceed with snap
        Debug.Log("Right trigger pressed - taking photo!");

        if (snapEffect != null)
        {
            snapEffect.PlaySnap();
            Debug.Log("Camera snap effect played");
        }
        else
        {
            Debug.LogWarning("CameraSnap: No CameraSnapEffect assigned/found.");
        }

        CaptureScreenshot();
    }

    // void CaptureScreenshot()
    // {
    //     Debug.Log("Capturing screenshot...");
    //     // Get the path to persistent data folder
    //     string folderPath = Path.Combine(Application.persistentDataPath, "BirdGallery");
    //     Debug.Log("Screenshot folder path: " + folderPath);

    //     // Create the folder if it doesn't exist
    //     if (!Directory.Exists(folderPath))
    //     {
    //         Directory.CreateDirectory(folderPath);
    //     }

    //     // Define the screenshot file path (with timestamp)
    //     string filePath = Path.Combine(
    //         folderPath,
    //         "screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png"
    //     );

    //     // Capture and save the screenshot
    //     ScreenCapture.CaptureScreenshot(filePath);
    //     Debug.Log("Screenshot saved to: " + filePath);
    // }

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
            "croppedScreenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png"
        );

        // Define the area to capture (left, top, width, height)
        // These values should be based on the screen coordinates you want to capture
        int x = 100; // X position (horizontal) of the top-left corner of the area
        int y = 100; // Y position (vertical) of the top-left corner of the area
        int width = 500; // Width of the area to capture
        int height = 300; // Height of the area to capture

        // Create a Texture2D to store the captured area
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);

        // Capture the pixels within the defined area
        screenShot.ReadPixels(new Rect(x, y, width, height), 0, 0);
        screenShot.Apply(); // Apply changes to the texture

        // Save the screenshot as a PNG file
        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);
        Debug.Log("Screenshot saved to: " + filePath);
    }
}
