using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Camera Definition")]
public class CameraSO : ScriptableObject
{
    [Header("Identity")]
    // public string id; // unique key, e.g. "cam_polaroid"
    public string cameraName; // e.g. "Polaroid 100"
    public string description;
    public Sprite icon;

    [Header("Price")]
    public int cost = 100;

    // [Header("Stats (optional for later)")]
    // public float maxZoom = 2f;
    // public int resolutionMP = 12;
}
