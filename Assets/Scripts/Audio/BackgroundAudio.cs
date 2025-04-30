using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{
    
    private AudioManager audioManager;
    
    void Start()
    {
        DontDestroyOnLoad(gameObject); 
        audioManager = FindFirstObjectByType<AudioManager>();
        audioManager.Play("forestSounds");
        audioManager.Play("backgroundSounds");
        // put name of background sound clip in the quotes above

    }
}
