using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSnap : MonoBehaviour
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

        // 4) (Optional) Save screenshot from camera — uncomment to use
        /*
        if (captureCamera == null)
        {
            Debug.LogWarning("CameraSnap: No captureCamera assigned.");
            return;
        }

        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        captureCamera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        captureCamera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenShot.Apply();

        captureCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();
        string filename = System.IO.Path.Combine(Application.persistentDataPath, $"photo_{System.DateTime.Now:yyyyMMdd_HHmmss}.png");
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log("Saved photo to: " + filename);
        */
    }
}
