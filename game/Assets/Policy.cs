using UnityEngine;
using System.Collections;

public class Policy : MonoBehaviour {

    public void Back()
    {
        Data.Instance.Load("LevelSelector");
    }
}
