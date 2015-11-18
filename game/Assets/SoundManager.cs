using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    void Start()
    {
        Events.OnSoundFX += OnSoundFX;
        Events.OnSoundsVolumeChanged += OnSoundsVolumeChanged;
    }

    void OnDestroy()
    {
        Events.OnSoundFX -= OnSoundFX;
        Events.OnSoundsVolumeChanged -= OnSoundsVolumeChanged;
    }
    void OnSoundsVolumeChanged(float value)
    {
        GetComponent<AudioSource>().volume = value;
        Data.Instance.soundsVolume = value;
        PlayerPrefs.SetFloat("soundsVolume", value); 
    }
    void OnSoundFX(string soundName)
    {
        if (Data.Instance.soundsVolume == 0) return;

        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(Resources.Load("audio/" + soundName) as AudioClip);
    }
    public void Turn(bool isACtive)
    {
        int value = 0;
        if (isACtive) value = 1;
        OnSoundsVolumeChanged(value);
        
    }
}
