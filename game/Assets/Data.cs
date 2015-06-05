using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Data : MonoBehaviour
{

    public float musicVolume = 1;
    public float soundsVolume = 1;

    const string PREFAB_PATH = "Data";

    static Data mInstance = null;

    public List<Color> colors; 
    [HideInInspector]
    public LevelData levelData;
    [HideInInspector]
    public Levels levels;

    public UserData userData;

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

        levelData = GetComponent<LevelData>();
        levels = GetComponent<Levels>();

        DontDestroyOnLoad(this.gameObject);

        userData = GetComponent<UserData>();
        userData.Init();

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
    public void Reset()
    {
    }
    void OnSaveVolumes(float _musicVolume, float _soundsVolume)
    {
        this.musicVolume = _musicVolume;
        this.soundsVolume = _soundsVolume;
    }
}
