using UnityEngine;
using UnityEngine.UI;

public class UICoinUpdate : MonoBehaviour {
    
    [SerializeField] private Image[] coinSlots;
    [SerializeField] private Sprite[] emptySprite;
    [SerializeField] private Sprite[] filledSprite;

    private void Start() {
        // Initialize all slots as empty
        bool[] emptyCoins = new bool[coinSlots.Length];
        for (int i = 0; i < emptyCoins.Length; i++)
        {
            emptyCoins[i] = false;
        }
        UpdateDisplay(emptyCoins);
    }
    
    public void UpdateDisplay(bool[] collectedCoins)
    {
        for (int i = 0; i < coinSlots.Length; i++)
        {
            if (i < emptySprite.Length && i < filledSprite.Length)
            {
                // only show filled sprite if this specific coin has been collected, correct coin id
                bool isCollected = i < collectedCoins.Length && collectedCoins[i];
                coinSlots[i].sprite = isCollected ? filledSprite[i] : emptySprite[i];
                Debug.Log($"Coin slot {i}: {(isCollected ? "filled" : "empty")}");
            }
            else
            {
                // you need the same matching spirtes of filled and empty sprites on CoinUI
                Debug.LogWarning($"Not enough sprites, make sure emptySprite and filledSprite arrays have enough elements.");
            }
        }
    }
}
