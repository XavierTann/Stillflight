using UnityEngine;
using UnityEngine.Events;

public class PlayerWallet : MonoBehaviour
{
    // --- Singleton ---
    public static PlayerWallet Instance { get; private set; }

    [SerializeField]
    private int startingCredits = 250;

    private int balance;

    public int Balance
    {
        get { return balance; }
    }

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // optional, keeps wallet across scenes

        balance = startingCredits;
    }

    public void AddCredits(int amount)
    {
        balance += amount;
    }

    public bool SpendCredits(int amount)
    {
        if (amount > balance)
            return false;
        else
        {
            balance -= amount;
            return true;
        }
    }
}
