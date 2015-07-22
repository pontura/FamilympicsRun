using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterFace : MonoBehaviour {

    public Sprite face1;
    public Sprite face2;
    public Sprite face3;
    public Sprite face4;

	public void Init(int id)
    {
        Image image = GetComponent<Image>();
        switch (id)
        {
            case 1: image.sprite = face1; break;
            case 2: image.sprite = face2; break;
            case 3: image.sprite = face3; break;
            case 4: image.sprite = face4; break;
        }
    }
}
