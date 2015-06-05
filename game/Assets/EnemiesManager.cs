using UnityEngine;
using System.Collections;

public class EnemiesManager : MonoBehaviour {

    public Enemy hurdle;

	void Start () {
        Events.StartGame += StartGame;
    }
    void OnDestroy()
    {
        Events.StartGame -= StartGame;
    }
    void StartGame()
    {
        Levels.LevelData levelData = Data.Instance.levels.GetCurrentLevelData();
        if(levelData.enemies.HURDLES)
            addEnemy(hurdle);
    }
    void addEnemy(Enemy enemy)
    {
        Events.OnAddEnemy(enemy);
    }
}
