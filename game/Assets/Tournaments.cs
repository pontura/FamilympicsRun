using UnityEngine;
using System.Collections;

public class Tournaments : MonoBehaviour {

    public ScrollLimit scrollLimit;
 
    public int limitScrollSeason1 = -926;
    public int limitScrollSeason2 = -2100;
    public int limitScrollSeason3 = -3200;
    public int limitScrollSeason4 = -4824;

    public GameObject lock1;
    public GameObject lock2;
    public GameObject lock3;

	void Start () {
	    if(Data.Instance.userData.levelProgressionId < 8 )
        {
            scrollLimit.SetLimit(new Vector2(scrollLimit.transform.localPosition.x, limitScrollSeason1));
        }
        else if (Data.Instance.userData.levelProgressionId < 16)
        {
            Destroy(lock1);
            scrollLimit.SetLimit(new Vector2(scrollLimit.transform.localPosition.x, limitScrollSeason2));
        }
        else if (Data.Instance.userData.levelProgressionId < 32)
        {
            Destroy(lock1);
            Destroy(lock2);
            scrollLimit.SetLimit(new Vector2(scrollLimit.transform.localPosition.x, limitScrollSeason3));
        }
        else
        {
            Destroy(lock1);
            Destroy(lock2);
            Destroy(lock3);
            scrollLimit.SetLimit(new Vector2(scrollLimit.transform.localPosition.x, limitScrollSeason4));
        }
	}
    public void PlayTournament(int id)
    {
        print("PlayTournament " + id);
        int levelID = 1;
        switch (id)
        {
            case 1: levelID = 1; break;
            case 2: levelID = 9; break;
            case 3: levelID = 17; break;
            case 4: levelID = 25; break;
        }
        Events.OnLoadParseScore(levelID);
        if (Data.Instance.userData.mode == UserData.modes.SINGLEPLAYER)
            Data.Instance.Load("SinglePlayer");
        else
            Data.Instance.Load("Players");

        Data.Instance.GetComponent<Levels>().currentLevel = levelID;
    }
}
