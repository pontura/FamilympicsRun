using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Data : MonoBehaviour
{
    public int winnerID;
    public int numPlayers;
    public int totalScore;

    public float musicVolume = 1;
    public float soundsVolume = 1;

    const string PREFAB_PATH = "Data";

    static Data mInstance = null;

    public List<Color> colors; 

    public static Data Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<Data>();

                if (mInstance == null)
                {
                    GameObject go = Instantiate(Resources.Load<GameObject>(PREFAB_PATH)) as GameObject;
                    mInstance = go.GetComponent<Data>();
                }
            }
            return mInstance;
        }
    }

    void Awake()
    {

        if (!mInstance)
            mInstance = this;

        else
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        GetComponent<UserData>().Init();
        GetComponent<MusicManager>().Init();
    }
    void OnMusicVolumeChanged(float value)
    {
        musicVolume = value;
    }
    void OnSoundsVolumeChanged(float value)
    {
        soundsVolume = value;
    }
    void OnBadgeSelected(int _totalScore)
    {
        this.totalScore = _totalScore;
    }
    public void Reset()
    {
    }
    void OnSaveVolumes(float _musicVolume, float _soundsVolume)
    {
        this.musicVolume = _musicVolume;
        this.soundsVolume = _soundsVolume;
    }
}
