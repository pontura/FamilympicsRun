using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Events {

    //The music:

    public static System.Action<bool> OnLoading = delegate { };
    public static System.Action OnResetApp = delegate { };
    

    public static System.Action ResetApp = delegate { };
    public static System.Action<float, float> OnSaveVolumes = delegate { };
    public static System.Action<float> OnMusicVolumeChanged = delegate { };
    public static System.Action<float> OnSoundsVolumeChanged = delegate { };
    public static System.Action<string> OnVoice = delegate { };
    public static System.Action<string> OnSoundFX = delegate { };

    public static System.Action<string> OnMusicChange = delegate { };
    public static System.Action<GameObject> OnUIClicked = delegate { };
    public static System.Action OnFacebookFriends = delegate { };
    public static System.Action OnFacebookNotConnected = delegate { };
    public static System.Action OnFacebookInviteFriends = delegate { };
    public static System.Action OnFacebookLogin = delegate { };
    public static System.Action OnParseLogin = delegate { };
    public static System.Action<string, float, int> OnParseLoadedScore = delegate { };
    public static System.Action OnLoadLocalData = delegate { };    
    public static System.Action<int, float> OnNewHiscore = delegate { };
    public static System.Action<string, string> AddFacebookFriend = delegate { };   
    

    //The game:

    
    public static System.Action StartGame = delegate { };
    public static System.Action OnLevelComplete = delegate { };
    public static System.Action<float> OnCheckIfAutomaticChallenge = delegate { };
    public static System.Action OpenIngameMenu = delegate { };
    

    public static System.Action<string> OpenPopUp = delegate { };

    public static System.Action OnEnergyWon = delegate { };

    //bool = bytime
    public static System.Action<bool> GameOver = delegate { };
    public static System.Action<int> AddStarsToCount = delegate { };
    
    public static System.Action OnTimeOver = delegate { };
    public static System.Action<bool> OnGamePaused = delegate { };
    public static System.Action<int, float> OnSaveScore = delegate { };
    public static System.Action OnRefreshHiscores = delegate { };
   // public static System.Action<int> OnLoadParseScore = delegate { };  
    public static System.Action<int, float, int, string> OnAddMultiplayerScore = delegate { };
    public static System.Action<UserData.modes> OnChangePlayMode = delegate { };
    public static System.Action OpenSummary = delegate { };
    public static System.Action OnRaceStartReady = delegate { };
    public static System.Action OnOpenEnergyPopup = delegate { };
    public static System.Action<Color> OnFlashWinLap = delegate { };   
    
    

    public static System.Action<string> OnKeyDown = delegate { };

    //this game
    public static System.Action<int> OnAvatarRun = delegate { };
    public static System.Action<int> OnAvatarJump = delegate { };
    public static System.Action<Player> OnAvatarDie = delegate { };

    //playerID, laps Count
    public static System.Action<int, int> OnAvatarWinLap = delegate { };

   

    public static System.Action<Enemy, int> OnAddEnemy = delegate { };
    public static System.Action<int> OnPowerUpOn = delegate { };
    public static System.Action<int, Powerups.types> OnPowerUpActive = delegate { };

    //Challenges
    public static System.Action OnChallengesLoad = delegate { };
    public static System.Action<string, string> OnChallengeRemind = delegate { };
    public static System.Action<string, string, int, float> OnChallengeCreate = delegate { };
    public static System.Action<string, string, string, float> OnChallengeClose = delegate { };
    public static System.Action<string> OnChallengeDelete = delegate { };
    public static System.Action<string> OnChallengeNotificated = delegate { };
    
    

    public static System.Action<int> OnFalseStart = delegate { };

    //facebookID;

    public static System.Action CheckForNewNotifications = delegate { };
    public static System.Action<string> OnNotificationReceived = delegate { };
    public static System.Action<string, string> SendNotificationTo = delegate { };
    public static System.Action<string> OnAcceptEnergyFrom = delegate { };
    public static System.Action<int> OnRefreshNotifications = delegate { };

    public static System.Action<int> BuyEnergyPack = delegate { };
    public static System.Action<int> AddPlusEnergy = delegate { };

    public static System.Action<int> ReFillEnergy = delegate { };
    public static System.Action<string> SendEnergyTo = delegate { };
    public static System.Action<string> RejectEnergyTo = delegate { };

    public static System.Action<int> OnTournamentStart = delegate { };
    public static System.Action<int, List<int>> OnTournamentAddScores = delegate { };
    public static System.Action OnTournamentFinish = delegate { };
    public static System.Action OnTournamentFinishAskForConfirmation = delegate { };
    public static System.Action<Tutorials.panels> OnTutorialOn = delegate { };
    public static System.Action OnTutorialOff = delegate { };
    
    


}
