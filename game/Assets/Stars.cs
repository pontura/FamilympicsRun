using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Stars : MonoBehaviour {

    public GameObject stars1;
    public GameObject stars2;
    public GameObject stars3;

	public void Init (int num) {

      //  print("______________________Stars: " + num);
        Reset();

        if (num == 3)
            setOn(stars3);
        else if (num == 2)
            setOn(stars2);
        else if (num == 1)
            setOn(stars1);
	}
    public void Reset()
    {
        setOff(stars1);
        setOff(stars2);
        setOff(stars3);
    }
    void setOff(GameObject stars)
    {
        stars.SetActive(false);
    }
    void setOn(GameObject stars)
    {
        stars.SetActive(true);
    }

}
