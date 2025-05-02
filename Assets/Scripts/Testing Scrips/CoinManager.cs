using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private UICoinUpdate coinUpdate;
    
    public static CoinManager Instance;

    private bool[] collectedCoins;
    public int totalCoins = 3;

    public delegate void AllCoinsCollected();
    public static event AllCoinsCollected OnAllCoinsCollected;

    // once UI is ready
    // [SerializeField] private Text coinText;

    private void Awake()
    {
        // making sure only one instance is active
        if (Instance == null) Instance = this;
        // if there is destory it
        else Destroy(gameObject);
    }

    private void Start()
    {
        collectedCoins = new bool[totalCoins];
        for (int i = 0; i < totalCoins; i++)
        {
            collectedCoins[i] = false;
        }
        coinUpdate?.UpdateDisplay(collectedCoins);
    }

    public void AddCoin(int coinId)
    {
        if (coinId >= 0 && coinId < totalCoins)
        {
            collectedCoins[coinId] = true;
            coinUpdate?.UpdateDisplay(collectedCoins);

            Debug.Log($"Coin {coinId + 1} collected!");

            // Check if all coins are collected
            bool allCollected = true;
            foreach (bool collected in collectedCoins)
            {
                if (!collected)
                {
                    allCollected = false;
                    break;
                }
            }

            if (allCollected)
            {
                Debug.Log("All coins collected! Nice job!");
                OnAllCoinsCollected?.Invoke();
            }
        }
        else
        {
            Debug.LogWarning($"Invalid coin ID: {coinId}");
        }
    }

    // UI for Coin
    // private void UpdateCoinUI()
    // {
    //     if (coinText != null)
    //     {
    //         coinText.text = $"{_coinsCollected}/{totalCoins}";
    //     }
    // }
    
}
