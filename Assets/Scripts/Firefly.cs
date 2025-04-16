using System;
using UnityEngine;

public class Firefly : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        Transform visual = transform.Find("FireflyVisual");
        
        visual.gameObject.AddComponent<FireflyFadeOut>();
    }
}
