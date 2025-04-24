using System;
using UnityEngine;

public class Firefly : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        // to mkae sure movement stops 
        FireflyMovement movement = GetComponent<FireflyMovement>();
        if (movement != null)
        {
            movement.enabled = false;
        }
        
        Transform visual = transform.Find("FireflyVisual");
        
        visual.gameObject.AddComponent<FireflyFadeOut>();
        
        if (other.CompareTag("Player")) {
            PlayerController2 player = other.GetComponent<PlayerController2>();
            if (player != null) {
                player.Respawn(); // Call method to reset player position
            }
        }
    }
}
