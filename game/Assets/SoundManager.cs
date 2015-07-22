using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{

    void Start()
    {
        Events.OnSoundFX += OnSoundFX;
    }

    void OnDestroy()
    {
        Events.OnSoundFX -= OnSoundFX;
    }

    void OnSoundFX(string soundName)
    {
        if (Data.Instance.soundsVolume == 0) return;

        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(Resources.Load("sound/" + soundName) as AudioClip);

    }
}
