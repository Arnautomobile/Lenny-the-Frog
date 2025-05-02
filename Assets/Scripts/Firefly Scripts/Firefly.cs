using System;
using UnityEngine;

public class Firefly : MonoBehaviour
{
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
            PlayerController2 player = other.GetComponent<PlayerController2>();
            if (player != null) {
                // calls the respawn method for the player setting them back to start of level
                player.Respawn(); 
            }
        }
    }
}
