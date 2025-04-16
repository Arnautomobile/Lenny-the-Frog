using UnityEngine;

public class KillingPlayer : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            Debug.Log("Player died! Try again");
    }
}
