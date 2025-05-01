using UnityEngine;

public class CoinCollection : MonoBehaviour
{

    // coid id is the same as the index of the coin in the CoinManager
    [SerializeField] private int coinId = 0;
    private bool isCollected = false;
    private Animator animator;

    public delegate void FrogEating();
    public static event FrogEating OnFrogCollecting;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other) {
        if (isCollected) return;

        if (other.CompareTag("Player")) {
            isCollected = true;
            CoinManager.Instance?.AddCoin(coinId);
            
            // should trigger the collecting sound 
            OnFrogCollecting?.Invoke();
            // triggering the animation
            animator.SetTrigger("Collect");
        }
    }
    
    public void OnCoinCollectedAnimationEnd()
    {
        Destroy(gameObject);
    }
}
