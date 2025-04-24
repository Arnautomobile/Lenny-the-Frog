using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool isCollected = false;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
        {
            isCollected = true;
            CoinManager.Instance.CollectCoin();
            animator.SetTrigger("Collect"); // Triggers collect animation
        }
    }
    
    
    
    public void OnCoinCollectedAnimationEnd()
    {
        Destroy(gameObject);
    }
    // private void Update()
    // {
    //     transform.Rotate(Vector3.up, 90 * Time.deltaTime); // Optional: coin spin
    // }
}
