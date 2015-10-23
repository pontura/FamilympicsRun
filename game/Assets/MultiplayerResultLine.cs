using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MultiplayerResultLine : MonoBehaviour {

    public Text usernameField;
    public Text scorefield;
    public Image image;

    public void Init(string username, string score, Color color)
    {
        usernameField.text = username;
        if (score != "")
            scorefield.text = score;
        image.color = color;
    }
}
