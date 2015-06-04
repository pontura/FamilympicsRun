using UnityEngine;
using System.Collections;

public class PlayerControllers : MonoBehaviour {

    public GameObject container_B_R;
    public GameObject container_B_L;
    public GameObject container_T_R;
    public GameObject container_T_L;

    public UIPlayerButtons uiPlayerButtons;

	void Start () {
        for (int i = 1; i < Data.Instance.levelData.numPlayers+1; i++)
        {
            UIPlayerButtons newUiPlayerButtons = Instantiate(uiPlayerButtons) as UIPlayerButtons;
            
            GameObject container;
            switch(i)
            {
                case 1: container = container_B_R; break;
                case 2: container = container_T_L; break;
                case 3: container = container_T_R; break;
                default: container = container_B_L; break;
            }
            newUiPlayerButtons.transform.SetParent(container.transform);
            newUiPlayerButtons.transform.localScale = Vector3.one;
            newUiPlayerButtons.transform.localPosition = Vector3.zero;

            newUiPlayerButtons.Init(i);
        }
	}
}
