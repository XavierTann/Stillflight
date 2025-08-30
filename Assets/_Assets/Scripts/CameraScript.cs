using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference rightTriggerAction; // Assign in Inspector

    [Header("Camera & VFX")]
    public Camera renderCamera;
    private CameraSnapEffect snapEffect;

    private void Awake()
    {
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
        // 1) Check camera mode
        if (CameraMode.Instance == null)
        {
            Debug.LogWarning("CameraSnap: No CameraMode.Instance found. Cannot check camera mode.");
            return;
        }

        if (!CameraMode.Instance.cameraMode)
        {
            Debug.Log("CameraSnap: Snap ignored — not in camera mode.");
            return;
        }

        // 2) Trigger snap effect
        if (snapEffect != null)
        {
            snapEffect.PlaySnap();
            Debug.Log("Camera snap effect played");
        }

        // 3) Save the RenderTexture
        CaptureScreenshot();
    }

    void CaptureScreenshot()
    {
        if (renderCamera == null || renderCamera.targetTexture == null)
        {
            Debug.LogError("CaptureScreenshot: captureCamera or targetTexture is missing!");
            return;
        }

        RenderTexture rt = renderCamera.targetTexture;

        // Make sure camera renders into the RT before reading it
        renderCamera.Render();

        // Prepare save path
        string folderPath = Path.Combine(Application.persistentDataPath, "BirdGallery");
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string filePath = Path.Combine(
            folderPath,
            "zoomScreenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png"
        );

        // Saving previous render texture Unity was working on to restore later.
        RenderTexture prevActive = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        RenderTexture.active = prevActive;

        // Save PNG
        File.WriteAllBytes(filePath, tex.EncodeToPNG());
        Debug.Log("ZoomTexture saved to: " + filePath);

        Destroy(tex);
    }
}
