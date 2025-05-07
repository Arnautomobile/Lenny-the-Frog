using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    public Slider volumeSlider;
    public GameSoundManager soundManager;
    public AudioManager audioManager;
    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume");
    }

    public void ChangeVolume()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        //Call audio function
        if (soundManager != null)
        {
            soundManager.audioManager.SetVolume(volumeSlider.value);
        }
        else
        {
            if (audioManager == null)
            {
                audioManager = FindFirstObjectByType<AudioManager>();
            }
            audioManager.SetVolume(volumeSlider.value);
        }
    }
}
