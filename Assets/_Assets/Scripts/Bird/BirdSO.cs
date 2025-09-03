using UnityEngine;

[CreateAssetMenu(fileName = "NewBird", menuName = "Birdwatching/Bird", order = 0)]
public class BirdSO : ScriptableObject
{
    [Header("General Info")]
    public string birdName;

    [TextArea]
    public string description;

    [Header("Visuals")]
    public Sprite icon; // For UI (gallery/shop/encyclopedia)
    public GameObject prefab; // For spawning the bird in the world

    [Header("Gameplay")]
    public int reward; // Points awarded for photographing
    public float rarity; // 0–1 scale (1 = common, 0.1 = rare)
    public float flightSpeed; // Movement speed for AI
}
