using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    public Slider volumeSlider;
    public GameSoundManager soundManager;
    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume");
    }

    public void ChangeVolume()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        //Call audio function
        soundManager.audioManager.SetVolume(volumeSlider.value);
    }
}
