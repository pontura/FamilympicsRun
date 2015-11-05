﻿using UnityEngine;
using System.Collections;

public class EnemiesManager : MonoBehaviour {

    [SerializeField]
    public Enemy verticalBar;

    [SerializeField]
    public Enemy hurdle;

    [SerializeField]
    public Enemy winds;

    [SerializeField]
    public Enemy trampolin;

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
        if (levelData.enemies.VERTICAL_BAR.Length>0)
            AddVerticalBar(levelData.enemies.VERTICAL_BAR);
        if (levelData.enemies.TRAMPOLIN)
            addEnemy(trampolin, distance);
        if (levelData.enemies.HURDLES)
            addEnemy(hurdle, distance);
        if (levelData.enemies.WIND)
            addEnemy(winds, distance);
    }
    void addEnemy(Enemy enemy, int distance)
    {
        Events.OnAddEnemy(enemy, distance); 
    }
    void AddVerticalBar(Levels.VerticalBar[] all)
    {
        foreach (Levels.VerticalBar data in all)
        {
            VerticalEnemy newEnemy = Instantiate(verticalBar) as VerticalEnemy;
            newEnemy.transform.SetParent(InSceneContainer.transform);
            newEnemy.transform.localScale = Vector3.one;            
            Vector3 pos = Vector3.zero;
            newEnemy.InitInScene(pos);
            newEnemy.InitVerticalEnemy(data);
        }
    }
}
