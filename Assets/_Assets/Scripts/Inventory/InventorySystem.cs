using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    // --- Singleton ---
    public static InventorySystem Instance { get; private set; }

    [Header("Debug/Bootstrap (optional)")]
    [Tooltip("Start the player with these cameras owned (for testing).")]
    public List<CameraSO> startingOwned = new List<CameraSO>();
    public CameraSO startingEquipped;

    private readonly HashSet<CameraSO> _owned = new HashSet<CameraSO>();
    private CameraSO _equipped;

    // --- Queries ---
    public bool IsOwned(CameraSO camera)
    {
        return camera != null && _owned.Contains(camera);
    }

    public CameraSO GetEquipped()
    {
        return _equipped;
    }

    public bool IsEquipped(CameraSO camera)
    {
        return camera != null && camera == _equipped;
    }

    public IReadOnlyCollection<CameraSO> GetAllOwned()
    {
        return _owned; // read-only view
    }

    // --- Mutations ---
    public bool Add(CameraSO camera)
    {
        if (camera == null)
            return false;

        bool added = _owned.Add(camera);

        // Auto-equip if nothing is equipped yet
        if (_equipped == null)
            _equipped = camera;

        return added;
    }

    public bool Remove(CameraSO camera)
    {
        if (camera == null)
            return false;

        bool removed = _owned.Remove(camera);

        if (removed && _equipped == camera)
            _equipped = null; // unequip if it was the equipped one

        return removed;
    }

    public bool Equip(CameraSO camera)
    {
        if (camera == null)
            return false;
        if (!_owned.Contains(camera))
            return false;

        _equipped = camera;
        return true;
    }

    // --- Lifecycle ---
    private void Awake()
    {
        // Singleton enforcement
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Seed test data
        foreach (var cam in startingOwned)
        {
            if (cam != null)
                _owned.Add(cam);
        }

        if (startingEquipped != null && _owned.Contains(startingEquipped))
            _equipped = startingEquipped;
        else if (_equipped == null && _owned.Count > 0)
            _equipped = GetFirstOwned();
    }

    private CameraSO GetFirstOwned()
    {
        foreach (var cam in _owned)
            return cam;
        return null;
    }
}
