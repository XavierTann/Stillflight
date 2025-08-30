using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class CameraMode : MonoBehaviour
{
    // Singleton instance
    public static CameraMode Instance { get; private set; }

    public Transform xrCamera; // Reference to the XR camera (CenterEye)
    public float activationDistance = 0.25f; // Distance threshold to activate camera view

    [Header("State")]
    [ReadOnly] // OdinInspector attribute to make it visible but not editable in inspector
    public bool cameraMode = false; // Publicly accessible state

    // [Header("Volume")]
    // public Volume globalVolume;
    // public VolumeProfile normalProfile;
    // public VolumeProfile cameraModeProfile;

    [SerializeField]
    private GameObject cameraUI;

    [SerializeField]
    private GameObject cameraVisual;

    [Header("Controller Visuals")]
    [SerializeField]
    private GameObject leftControllerVisual;

    [SerializeField]
    private GameObject rightControllerVisual;

    [SerializeField]
    private GameObject leftLineVisual;

    [SerializeField]
    private GameObject rightLineVisual;

    // Event triggered when the camera mode is activated
    public event System.Action OnCameraModeActivated;

    // Event triggered when the camera mode is deactivated
    public event System.Action OnCameraModeDeactivated;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Debug.Log("CameraMode already exists, destroying new instance");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        cameraUI.SetActive(false);
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, xrCamera.position);

        if (distance <= activationDistance && !cameraMode)
        {
            EnterCameraView();
        }
        else if (distance > activationDistance && cameraMode)
        {
            ExitCameraView();
        }
    }

    public void EnterCameraView()
    {
        // Trigger the event when camera mode is activated
        OnCameraModeActivated?.Invoke();

        Debug.Log("Camera View entered!");
        cameraMode = true;
        // globalVolume.profile = cameraModeProfile;
        cameraUI.SetActive(true);
        cameraVisual.SetActive(false);
        leftControllerVisual.SetActive(false);
        rightControllerVisual.SetActive(false);
        leftLineVisual.SetActive(false);
        rightLineVisual.SetActive(false);
    }

    public void ExitCameraView()
    {
        // Trigger the event when camera mode is deactivated
        OnCameraModeDeactivated?.Invoke();

        cameraMode = false;
        Debug.Log("Camera View exited!");
        // globalVolume.profile = normalProfile;
        cameraUI.SetActive(false);
        cameraVisual.SetActive(true);
        leftControllerVisual.SetActive(true);
        rightControllerVisual.SetActive(true);
        leftLineVisual.SetActive(true);
        rightLineVisual.SetActive(true);
    }
}
