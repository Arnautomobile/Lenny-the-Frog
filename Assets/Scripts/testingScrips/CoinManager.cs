using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    private int _coinsCollected = 0;
    public int totalCoins = 4;

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

    public void CollectCoin()
    {
        _coinsCollected++;
        //UpdateCoinUI();

        Debug.Log($"Coins collected: {_coinsCollected}/{totalCoins}");

        if (_coinsCollected >= totalCoins)
        {
            Debug.Log("All coins collected! Nice job!");
            // You could store this for rewards later
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
    
    public int GetCoinCount()
    {
        return _coinsCollected;
    }
}
