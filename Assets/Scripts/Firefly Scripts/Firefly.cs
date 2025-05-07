using UnityEngine;

public class Firefly : MonoBehaviour
{
    public delegate void FireflyTouched();
    public static event FireflyTouched OnFireflyTouched;
    private void OnTriggerEnter(Collider other) {
        // to mkae sure movement stops once touched 
        FireflyMovement movement = GetComponent<FireflyMovement>();
        if (movement != null)
        {
            movement.enabled = false;
        }
        
        Transform visual = transform.Find("FireflyVisual");
        // still doing the fade out animation
        visual.gameObject.AddComponent<FireflyFadeOut>();
        
        // checking if player hs touches it
        if (other.CompareTag("Player")) {
            GameLogic2 gameLogic = other.GetComponentInParent<GameLogic2>();
            if (gameLogic != null) {
                // playing sound of collision 
                OnFireflyTouched?.Invoke();
                // Set the player as dead and trigger the respawn coroutine
                gameLogic.KillPlayer();
            }
        }
    }
}
