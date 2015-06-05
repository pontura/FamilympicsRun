using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public void GotoLevelSelector()
    {
        Application.LoadLevel("LevelSelector");
    }
}
