using UnityEngine;
using System.Collections;

public static class Events {

    //The music:
    public static System.Action<float, float> OnSaveVolumes = delegate { };
    public static System.Action<float> OnMusicVolumeChanged = delegate { };
    public static System.Action<float> OnSoundsVolumeChanged = delegate { };
    public static System.Action<string> OnVoice = delegate { };
    public static System.Action<string> OnSoundFX = delegate { };

    public static System.Action<string> OnMusicChange = delegate { };
    public static System.Action<GameObject> OnUIClicked = delegate { };
    public static System.Action OnFacebookFriends = delegate { };
    public static System.Action OnFacebookLogin = delegate { };   
    

    //The game:
    public static System.Action StartGame = delegate { };
    public static System.Action OnLevelComplete = delegate { };
    public static System.Action OpenIngameMenu = delegate { };
    

    public static System.Action<string> OpenPopUp = delegate { };

    public static System.Action OnEnergyWon = delegate { };
    public static System.Action GameOver = delegate { };
    public static System.Action OnTimeOver = delegate { };
    public static System.Action<bool> OnGamePaused = delegate { };
    public static System.Action<int, float> OnSaveScore = delegate { };
    public static System.Action OnRefreshHiscores = delegate { };
    public static System.Action<int> OnLoadParseScore = delegate { };  
    public static System.Action<int, float, int, string> OnAddMultiplayerScore = delegate { };
    public static System.Action<UserData.modes> OnChangePlayMode = delegate { };
    public static System.Action OpenSummary = delegate { };
    public static System.Action OnRaceStartReady = delegate { };
    
    

    public static System.Action<string> OnKeyDown = delegate { };

    //this game
    public static System.Action<int> OnAvatarRun = delegate { };
    public static System.Action<int> OnAvatarJump = delegate { };
    public static System.Action<Player> OnAvatarDie = delegate { };

    //playerID, laps Count
    public static System.Action<int, int> OnAvatarWinLap = delegate { };

   

    public static System.Action<Enemy, int> OnAddEnemy = delegate { };
    public static System.Action OnPowerUpOn = delegate { };
    public static System.Action<int, Powerups.types> OnPowerUpActive = delegate { };

    //Challenges
    public static System.Action<string, string, int, float> OnChallengeCreate = delegate { };
    public static System.Action<string, string, string, float> OnChallengeClose = delegate { };

    public static System.Action<int> OnFalseStart = delegate { };

    public static System.Action<Vector2> OnScrollSizeRefresh = delegate { };     


}
