using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {
           
	public void Init () {
        GetComponent<AudioSource>().loop = true;
        OnMusicVolumeChanged(Data.Instance.musicVolume);

        Events.OnGamePaused += OnGamePaused;
        Events.OnMusicVolumeChanged += OnMusicVolumeChanged;
        Events.OnMusicChange += OnMusicChange;
	}
    void OnDestroy()
    {
        Events.OnGamePaused -= OnGamePaused;
        Events.OnMusicVolumeChanged -= OnMusicVolumeChanged;
        Events.OnMusicChange -= OnMusicChange;
    }

    void OnMusicChange(string soundName)
    {
        if (soundName == "")
        {
            GetComponent<AudioSource>().Stop();
            soundName = "";
        }
        if (GetComponent<AudioSource>().clip && GetComponent<AudioSource>().clip.name == soundName) return;
        GetComponent<AudioSource>().clip = Resources.Load("audio/" + soundName) as AudioClip;
        GetComponent<AudioSource>().Play();

        if (soundName == "victoryMusic") 
            GetComponent<AudioSource>().loop = false;
        else
            GetComponent<AudioSource>().loop = true;
    }
    void OnSoundsFadeTo(float to)
    {
        if (to > 0) to = Data.Instance.musicVolume;
       // TweenVolume tv = TweenVolume.Begin(gameObject, 1, to);
        //tv.PlayForward();
        //tv.onFinished.Clear();
    }
    void OnMusicVolumeChanged(float value)
    {
        GetComponent<AudioSource>().volume = value;
        Data.Instance.musicVolume = value;
        PlayerPrefs.SetFloat("musicVolume", value); 
    }
    public void Turn(bool isACtive)
    {
        int value = 0;
        if (isACtive) value = 1;
        OnMusicVolumeChanged(value);
    }
    void OnGamePaused(bool paused)
    {
        if(paused)
            GetComponent<AudioSource>().Stop();
        else
            GetComponent<AudioSource>().Play();
    }
    void stopAllSounds()
    {
        GetComponent<AudioSource>().Stop();
    }
}



