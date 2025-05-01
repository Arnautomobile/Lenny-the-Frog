using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    private int coinsCollected = 0;
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
        // once we get UI going 
        //UpdateCoinUI();
    }

    public void AddCoin()
    {
        coinsCollected++;
        //UpdateCoinUI();

        Debug.Log($"Coins collected: {coinsCollected}/{totalCoins}");

        if (coinsCollected >= totalCoins)
        {
            Debug.Log("All coins collected! Nice job!");
            OnAllCoinsCollected?.Invoke();
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
