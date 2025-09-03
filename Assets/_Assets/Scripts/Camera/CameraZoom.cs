using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class CameraZoom : MonoBehaviour
{
    // CAMERA MODE DISABLES AND ENABLES THIS SCRIPT, SO
    // NO NEED TO CHECK FOR CAMERA MODE IN UPDATE

    [Header("References")]
    public Camera mainCam; // Reference to main camera
    public Camera secondaryCam; // Secondary camera to control

    [Header("Zoom Settings")]
    public float zoomDistance = 5f; // Current zoom level
    public float zoomStep = 0.5f; // Change amount when J or K is pressed
    public float minZoom = 1f;
    public float maxZoom = 20f;

    private bool isZoomingIn = false; // Flag to track if we are zooming in
    private bool isZoomingOut = false; // Flag to track if we are zooming out

    [Header("Input")]
    public InputActionReference zoomIn; // Assign in Inspector
    public InputActionReference zoomOut; // Assign in Inspector

    private void OnEnable()
    {
        // Enable the PlayerControls action map when the script is enabled
        if (zoomIn != null && zoomOut != null)
        {
            // Add input action events to zoom in and out
            zoomIn.action.performed += OnZoomIn;
            zoomOut.action.performed += OnZoomOut;

            zoomIn.action.canceled += OnZoomCanceled;
            zoomOut.action.canceled += OnZoomCanceled;

            // Enable the input actions
            zoomIn.action.Enable();
            zoomOut.action.Enable();
        }
    }

    private void OnDisable()
    {
        // Disable the actions and unsubscribe from events when the script is disabled
        if (zoomIn != null && zoomOut != null)
        {
            zoomIn.action.performed -= OnZoomIn; // Remove the event handler
            zoomOut.action.performed -= OnZoomOut; // Remove the event handler

            zoomIn.action.canceled += OnZoomCanceled;
            zoomOut.action.canceled += OnZoomCanceled;

            zoomIn.action.Disable(); // Disable the zoom-in action
            zoomOut.action.Disable(); // Disable the zoom-out action
        }
    }

    // Event handler for zooming in
    private void OnZoomIn(InputAction.CallbackContext context)
    {
        isZoomingIn = true;
        isZoomingOut = false;
    }

    // Event handler for zooming out
    private void OnZoomOut(InputAction.CallbackContext context)
    {
        isZoomingIn = false;
        isZoomingOut = true;
    }

    private void OnZoomCanceled(InputAction.CallbackContext context)
    {
        // Reset both flags to false if neither button is pressed
        isZoomingIn = false;
        isZoomingOut = false;
    }

    void Update()
    {
        // Press J to zoom in (increase distance)
        if (isZoomingIn)
        {
            Debug.Log("Zooming in");
            zoomDistance = Mathf.Clamp(zoomDistance + zoomStep * Time.deltaTime, minZoom, maxZoom);
            // newPosition = mainCam.transform.position + mainCam.transform.forward * zoomDistance;
        }
        // Press K to zoom out (decrease distance)
        else if (isZoomingOut)
        {
            Debug.Log("Zooming out");
            zoomDistance = Mathf.Clamp(zoomDistance - zoomStep * Time.deltaTime, minZoom, maxZoom);
        }
    }

    void LateUpdate()
    {
        if (mainCam == null || secondaryCam == null)
            return;

        // Use the player's look direction (main camera forward, flattened if you want only horizontal)
        Vector3 lookDir = mainCam.transform.forward.normalized;

        // Set secondary camera position
        secondaryCam.transform.position = mainCam.transform.position + lookDir * zoomDistance;

        // Match rotation to main camera
        secondaryCam.transform.rotation = mainCam.transform.rotation;
    }
}
