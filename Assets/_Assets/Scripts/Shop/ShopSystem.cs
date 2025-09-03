using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    // --- Singleton ---
    public static ShopSystem Instance { get; private set; }

    private PlayerWallet wallet;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // optional: persists across scenes

        wallet = PlayerWallet.Instance;
    }

    public bool TryBuy(CameraSO cameraSO)
    {
        if (wallet == null)
        {
            Debug.LogError("ShopSystem: No PlayerWallet instance found.");
            return false;
        }

        if (!wallet.SpendCredits(cameraSO.cost))
        {
            Debug.Log("Not enough credits to buy " + cameraSO.cameraName);
            return false;
        }

        Debug.Log("Bought: " + cameraSO.cameraName + " for " + cameraSO.cost + " credits.");
        return true;
    }
}
