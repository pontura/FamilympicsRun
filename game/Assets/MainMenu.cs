using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    void Start()
    {
        //forzar a cargar el Data instance:
        float vol = Data.Instance.musicVolume;
    }
    public void GotoLevelSelector()
    {
        Data.Instance.Load("LevelSelector");
    }
}
