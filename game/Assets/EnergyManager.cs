using UnityEngine;
using System.Collections;

public class EnergyManager : MonoBehaviour {

    public int energyPerSingleRace;
    public int energyPerMultiRace;
    private UserData userData;

	public void Init () {
        this.userData = GetComponent<UserData>();

        if (PlayerPrefs.GetInt("energy") > 0)
            userData.energy = PlayerPrefs.GetInt("energy");

        Events.StartGame += StartGame;
	}
    void OnDestroy()
    {
        Events.StartGame -= StartGame;
    }
    void StartGame()
    {
        if(userData.mode == UserData.modes.SINGLEPLAYER)
            userData.energy -= energyPerSingleRace;
        else  
            userData.energy -= energyPerMultiRace;

        if (userData.energy > 0)
        {
            PlayerPrefs.SetInt("energy", userData.energy);
        }
    }
}
