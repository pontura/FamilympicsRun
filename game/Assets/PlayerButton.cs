using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerButton : MonoBehaviour {

    public Image image;
    public Text field;
    public int id;
    public bool selected;
    private MultiplayerData multiplayerData;

	public void Init () {
        multiplayerData = Data.Instance.multiplayerData;
        image.color = Data.Instance.colors[id-1];
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Toogle();
        });

	}
    void Toogle()
    {
        selected = !selected;
        if (selected) IsOn();
        else IsOff();
    }
    void IsOn()
    {
        field.text = "OK";
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        multiplayerData.AddPlayer(id);
    }
    void IsOff()
    {
        field.text = "X";
        transform.localScale = new Vector3(1f, 1f, 1f);
        multiplayerData.DeletePlayer(id);
    }

}
