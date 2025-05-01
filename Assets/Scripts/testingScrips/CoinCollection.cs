using UnityEngine;

public class CoinCollection : MonoBehaviour
{
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
            CoinManager.Instance?.AddCoin();
            
            // should trigger the collecting sound 
            OnFrogCollecting?.Invoke();
            animator.SetTrigger("Collect"); // Triggers collect animation
        }
    }
    
    public void OnCoinCollectedAnimationEnd()
    {
        Destroy(gameObject);
    }
}
