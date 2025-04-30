using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{
    
    private AudioManager audioManager;
    
    void Start()
    {
        DontDestroyOnLoad(gameObject); 
        audioManager = FindFirstObjectByType<AudioManager>();
        Debug.Log("Background audio playing next");
        audioManager.Play("forestSounds");
        audioManager.Play("backgroundMusic");
        // put name of background sound clip in the quotes above

    }
}
