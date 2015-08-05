using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountDown : MonoBehaviour {

    [SerializeField]
    Text field;
    private int num = 3;

    void Start()
    {
        Events.OnRaceStartReady += OnRaceStartReady;
    }
    void OnDestroy()
    {
        Events.OnRaceStartReady -= OnRaceStartReady;
    }
    void OnRaceStartReady()
    {
        field.text = num.ToString();
        Invoke("nextNum", 1);
        Events.OnSoundFX("3");
    }
    void nextNum()
    {
        

        num--;

        if (num > 0)
            Events.OnSoundFX((num).ToString());
        else
            Events.OnSoundFX("go");

        field.text = num.ToString();
        if (num <= 0)
        {
            Events.StartGame();
            Destroy(gameObject);
            field = null;
        }
        else
        {
            Invoke("nextNum", 1);
        }
    }
}
