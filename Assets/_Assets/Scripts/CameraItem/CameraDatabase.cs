using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Camera Database")]
public class CameraDatabase : ScriptableObject
{
    public List<CameraSO> cameras = new();
}
