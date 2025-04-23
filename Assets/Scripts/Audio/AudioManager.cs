using UnityEngine;
using UnityEngine.Audio;
using System;


/**
 * This is the audiomanager class. This utilizes the sound class and stores an array
 * of all the audioclips to be used within the game
 * 
 * In order to play a sound in another script, just create an audioManager variable and
 * call .Play("soundName") in the script to play a sound.
 *
 * 
 */
public class AudioManager : MonoBehaviour
{
    
    public Sound[] sounds;

    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        //mark as don't destroy on load for the audioManager gameobject
        DontDestroyOnLoad(gameObject); 
    }

    /**
     * Used to Play an audio clip
     */
    public void Play(string soundName)
    {
        Sound s = Array.Find(sounds, s => s.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        s.source.Play();
    }
    
    
}