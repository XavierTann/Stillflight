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

    [SerializeField]
    private CameraZoom cameraZoom;

    [SerializeField]
    private CameraScript cameraScript;

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
    { // Do I really need to check this every frame?
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
        Debug.Log("Camera View entered!");
        cameraMode = true;
        // globalVolume.profile = cameraModeProfile;
        cameraUI.SetActive(true);
        cameraVisual.SetActive(false);
        leftControllerVisual.SetActive(false);
        rightControllerVisual.SetActive(false);
        leftLineVisual.SetActive(false);
        rightLineVisual.SetActive(false);

        cameraZoom.enabled = true;
        cameraScript.enabled = true;
    }

    public void ExitCameraView()
    {
        cameraMode = false;
        Debug.Log("Camera View exited!");
        // globalVolume.profile = normalProfile;
        cameraUI.SetActive(false);
        cameraVisual.SetActive(true);
        leftControllerVisual.SetActive(true);
        rightControllerVisual.SetActive(true);
        leftLineVisual.SetActive(true);
        rightLineVisual.SetActive(true);

        cameraZoom.enabled = false;
        cameraScript.enabled = false;
    }
}
