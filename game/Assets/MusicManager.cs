using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

    public float volume;
       
	public void Init () {
        audio.loop = true;
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
        if (audio.clip && audio.clip.name == soundName) return;
        audio.clip = Resources.Load("music/" + soundName) as AudioClip;
        audio.Play();

        if (soundName == "victoryMusic") 
            audio.loop = false;
        else
            audio.loop = true;
    }
    void OnSoundsFadeTo(float to)
    {
        if (to > 0) to = volume;
       // TweenVolume tv = TweenVolume.Begin(gameObject, 1, to);
        //tv.PlayForward();
        //tv.onFinished.Clear();
    }
    void OnMusicVolumeChanged(float value)
    {
        audio.volume = value;
        volume = value;
    }
    void OnGamePaused(bool paused)
    {
        if(paused)
            audio.Stop();
        else
            audio.Play();
    }
    void stopAllSounds()
    {
        audio.Stop();
    }
}



