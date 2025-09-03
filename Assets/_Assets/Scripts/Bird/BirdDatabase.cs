using System.Collections.Generic;
using UnityEngine;

public class BirdDatabase : MonoBehaviour
{
    public static BirdDatabase Instance { get; private set; }

    // Keeps track of all discovered birds
    private HashSet<BirdSO> discoveredBirds = new HashSet<BirdSO>();
    private List<BirdSO> discoveredList = new List<BirdSO>(); // List for ordered reads

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void DiscoverBird(BirdSO bird)
    {
        if (bird == null)
        {
            Debug.LogWarning("Tried to add null BirdSO to database.");
            return;
        }

        if (discoveredBirds.Add(bird)) // returns true if newly added
        {
            discoveredList.Add(bird); // keep ordered copy
            Debug.Log($"Discovered a new bird: {bird.birdName}");
            //Add credits
            PlayerWallet.Instance.AddCredits(bird.reward);
        }
        else
        {
            Debug.Log($"{bird.birdName} is already discovered.");
        }
    }

    public bool HasDiscovered(BirdSO bird)
    {
        if (bird == null)
            return false;

        return discoveredBirds.Contains(bird);
    }

    public IReadOnlyList<BirdSO> GetAllDiscovered()
    {
        return discoveredList.AsReadOnly();
    }
}
