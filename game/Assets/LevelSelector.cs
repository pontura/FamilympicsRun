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
        int a = 0;
        foreach (Levels.LevelData levelData in Data.Instance.levels.levels)
        {
            LevelButton newLevelButton = Instantiate(levelButton) as LevelButton;
            newLevelButton.transform.SetParent(container.transform);
            newLevelButton.transform.localPosition = new Vector3(buttonsSeparation * a, 0, 0);
            newLevelButton.Init(this, a + 1);
            a++;
        }
	}

    public void StartLevel(int id)
    {
        Application.LoadLevel("Players");
        Data.Instance.GetComponent<Levels>().currentLevel = id;
    }
    public void Login()
    {
        Application.LoadLevel("Login");
    }
}
