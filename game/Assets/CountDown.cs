using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountDown : MonoBehaviour {

    [SerializeField]
    Text field;
    private int num = 3;

    void Start()
    {
        field.text = num.ToString();
        Invoke("nextNum", 1);
    }
    void nextNum()
    {
        num--;
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
