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

    [Header("Raycast")]
    [Tooltip("How far the photo 'ray' should check for a bird.")]
    [SerializeField]
    private float rayDistance = 100f;

    [Tooltip(
        "LayerMask for birds. Set to the 'Bird' layer in the Inspector (or left empty to auto-resolve)."
    )]
    [SerializeField]
    private LayerMask birdLayerMask;

    private void Awake()
    {
        if (snapEffect == null)
            snapEffect = FindFirstObjectByType<CameraSnapEffect>();

        // Auto-resolve the Bird layer if the mask isn't set in the Inspector.
        if (birdLayerMask.value == 0)
            birdLayerMask = LayerMask.GetMask("Bird");
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

        // 1.5) Raycast in the look direction to check for birds
        RaycastForBird();

        // 2) Trigger snap effect
        if (snapEffect != null)
        {
            snapEffect.PlaySnap();
            Debug.Log("Camera snap effect played");
        }

        // 3) Save the RenderTexture
        CaptureScreenshot();
    }

    /// <summary>
    /// Casts a ray from the renderCamera forward and logs if it hits the Bird layer.
    /// </summary>
    private void RaycastForBird()
    {
        if (renderCamera == null)
        {
            Debug.LogWarning("RaycastForBird: No renderCamera assigned.");
            return;
        }

        Ray ray = new Ray(renderCamera.transform.position, renderCamera.transform.forward);

        // If birdLayerMask is still zero (layer not found), fall back to any hit.
        if (birdLayerMask.value != 0)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, birdLayerMask))
                Debug.Log("Hit a bird!");
            else
                Debug.Log("Hit nothing!");
        }
        else
        {
            // Optional fallback (not layer-filtered) in case the 'Bird' layer doesn't exist.
            if (Physics.Raycast(ray, out RaycastHit _, rayDistance))
                Debug.Log("Hit something (no Bird layer set)!");
            else
                Debug.Log("Hit nothing!");
        }
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
