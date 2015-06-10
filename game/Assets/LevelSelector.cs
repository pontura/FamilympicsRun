using UnityEngine;
using System.Collections;
using Parse;
using System.Collections.Generic;
using System.Threading.Tasks;

public class LevelSelector : MonoBehaviour {

    public GameObject loginButton;
    public GameObject container;

    [SerializeField]
    LevelButton levelButton;

    private int buttonsSeparation = 300;

	void Start () {
        
        Data.Instance.levelData.ResetChallenge();

        for (int a = 1; a < Data.Instance.levels.levels.Length; a++ )
        {
            LevelButton newLevelButton = Instantiate(levelButton) as LevelButton;
            newLevelButton.transform.SetParent(container.transform);
            newLevelButton.transform.localPosition = new Vector3(buttonsSeparation * (a-2), 0, 0);
            newLevelButton.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            newLevelButton.Init(this, a);
        }
        Events.OnRefreshHiscores();
	}

    public void StartLevel(int id)
    {
        Events.OnLoadParseScore(id);
        Application.LoadLevel("Players");
        Data.Instance.GetComponent<Levels>().currentLevel = id;
    }
    public void Login()
    {
        Application.LoadLevel("Login");
    }
    public void Refresh()
    {
        Data.Instance.levelsData.Refresh();
        Application.LoadLevel("LevelSelector");        
    }
}
