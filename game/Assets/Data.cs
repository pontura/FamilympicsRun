using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Data : MonoBehaviour
{
    public bool FreeLevels;
    public bool OnlyMultiplayer;

    public float musicVolume = 1;
    public float soundsVolume = 1;

    const string PREFAB_PATH = "Data";

    static Data mInstance = null;

    public List<Color> colors; 
    [HideInInspector]
    public LevelData levelData;
    [HideInInspector]
    public Levels levels;
    [HideInInspector]
    public LevelsData levelsData;
    [HideInInspector]
    public MultiplayerData multiplayerData;
    [HideInInspector]
    public EnergyManager energyManager;

     [HideInInspector]
    public UserData userData;
     [HideInInspector]
    public LoginManager loginManager;
     [HideInInspector]
    public string lastScene;
     [HideInInspector]
    public GameSettings gameSettings;
     [HideInInspector]
    public MusicManager musicManager;
     [HideInInspector]
    public SoundManager soundManager;
     [HideInInspector]
    public Notifications notifications;
     [HideInInspector]
     public FacebookShare facebookShare;

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

       // PlayerPrefs.DeleteAll();

        if (!mInstance)
            mInstance = this;

        else
        {
            Destroy(this.gameObject);
            return;
        }
        gameSettings = GetComponent<GameSettings>();
        loginManager = GetComponent<LoginManager>();
        levelData = GetComponent<LevelData>();
        levels = GetComponent<Levels>();
        levelsData = GetComponent<LevelsData>();
        multiplayerData = GetComponent<MultiplayerData>();
        energyManager = GetComponent<EnergyManager>();
        musicManager = GetComponent<MusicManager>();
        soundManager = GetComponentInChildren<SoundManager>();
        notifications = GetComponent<Notifications>();
        facebookShare = GetComponent<FacebookShare>();
       // levelsData.Init();

        DontDestroyOnLoad(this.gameObject);

        userData = GetComponent<UserData>();
        userData.Init();
        multiplayerData.Init();

        GetComponent<MusicManager>().Init();

        Events.ResetApp += ResetApp;
    }
    void OnMusicVolumeChanged(float value)
    {
        musicVolume = value;
    }
    void OnSoundsVolumeChanged(float value)
    {
        soundsVolume = value;
    }
    void OnSaveVolumes(float _musicVolume, float _soundsVolume)
    {
        this.musicVolume = _musicVolume;
        this.soundsVolume = _soundsVolume;
    }    
    public void Load(string nextScene)
    {
        lastScene = Application.loadedLevelName;
        if (nextScene == "") nextScene = "LevelSelector";
        Debug.Log("Load Scene: " + nextScene);
        Application.LoadLevel(nextScene);
    }
    public void Back()
    {
        Load(lastScene);
    }
    public void ResetApp()
    {
        PlayerPrefs.DeleteAll();
        Data.Instance.levelsData.Reset();
        Data.Instance.userData.Reset();
        Data.Instance.multiplayerData.Reset();
    }

}
