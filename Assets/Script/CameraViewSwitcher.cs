using UnityEngine;
using UnityEngine.XR;

public class CameraViewSwitcher : MonoBehaviour
{
    public Transform xrCamera;        // Reference to the XR camera (CenterEye)
    public float activationDistance = 0.25f;  // Distance threshold to activate camera view
    public Camera objectCamera;       // The camera attached to the object
    public Canvas cameraViewCanvas;   // Optional: UI to simulate view

    private bool inCameraView = false;

    void Update()
    {
        if (xrCamera == null || objectCamera == null)
            return;

        float distance = Vector3.Distance(transform.position, xrCamera.position);

        if (distance <= activationDistance && !inCameraView)
        {
            EnterCameraView();
        }
        else if (distance > activationDistance && inCameraView)
        {
            ExitCameraView();
        }
    }

    void EnterCameraView()
    {
        inCameraView = true;
        objectCamera.enabled = true;
        Debug.Log("Camera View entered!")

        // Optional: Use a RenderTexture and show it in a UI canvas
        if (cameraViewCanvas != null)
            cameraViewCanvas.enabled = true;

        // Optionally disable main XR camera
        // xrCamera.GetComponent<Camera>().enabled = false;
    }

    void ExitCameraView()
    {
        inCameraView = false;
        objectCamera.enabled = false;

        if (cameraViewCanvas != null)
            cameraViewCanvas.enabled = false;

        // xrCamera.GetComponent<Camera>().enabled = true;
    }
}
