using UnityEngine;
using System.Collections;

public class EnemiesManager : MonoBehaviour {

    [SerializeField]
    public Enemy hurdle;

    [SerializeField]
    public Enemy winds;

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

        int distance = levelData.enemies.distance;
        if(levelData.enemies.HURDLES)
            addEnemy(hurdle, distance);
        if (levelData.enemies.WIND)
            addEnemy(winds, distance);
    }
    void addEnemy(Enemy enemy, int distance)
    {
        Events.OnAddEnemy(enemy, distance); 
    }
}
