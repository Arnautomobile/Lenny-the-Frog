using System;
using UnityEngine;

public class FinishVolume : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            Debug.Log("Level finished!");
    }
}
