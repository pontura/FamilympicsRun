﻿using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public void GotoLevelSelector()
    {
        Data.Instance.Load("LevelSelector");
    }
}
