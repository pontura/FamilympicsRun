using UnityEngine;
using System.Collections;

public class EnemiesManager : MonoBehaviour {

    [SerializeField]
    public Enemy verticalBar;

    [SerializeField]
    public Enemy hurdle;

    [SerializeField]
    public Enemy winds;

    private Levels.LevelData levelData;

    public GameObject InSceneContainer;

	void Start () {
        levelData = Data.Instance.levels.GetCurrentLevelData();
        Events.StartGame += StartGame;
        
    }
    void OnDestroy()
    {
        Events.StartGame -= StartGame;
    }
    void StartGame()
    {       
        int distance = levelData.enemies.distance;
        if (levelData.enemies.VERTICAL_BAR)
            AddSceneEnemy(verticalBar);
        if(levelData.enemies.HURDLES)
            addEnemy(hurdle, distance);
        if (levelData.enemies.WIND)
            addEnemy(winds, distance);
    }
    void addEnemy(Enemy enemy, int distance)
    {
        Events.OnAddEnemy(enemy, distance); 
    }
    void AddSceneEnemy(Enemy enemy)
    {
        Enemy newEnemy = Instantiate(enemy) as Enemy;
        newEnemy.transform.SetParent(InSceneContainer.transform);
        newEnemy.transform.localScale = Vector3.one;

        Vector3 pos = Vector3.zero;
        newEnemy.InitInScene(pos);
    }
}
