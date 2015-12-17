using UnityEngine;
using System.Collections;

public class Tutorials : MonoBehaviour {

    public GameObject panel;

    public GameObject tutorial_walls;
    public GameObject tutorial_trampolin;
    public GameObject tutorial_run;
    public GameObject tutorial_mud;
    public GameObject tutorial_hurdles;

    public enum panels
    {
        WALLS,
        TRAMPOLIN,
        RUN,
        MUD,
        HURDLES
    }

	void Start () {
        panel.SetActive(false);
        Events.OnTutorialOn += OnTutorialOn;
	}
    void OnDestroy()
    {
        Events.OnTutorialOn -= OnTutorialOn;
    }
    void SetOff()
    {
        tutorial_walls.SetActive(false);
        tutorial_trampolin.SetActive(false);
        tutorial_run.SetActive(false);
        tutorial_mud.SetActive(false);
        tutorial_hurdles.SetActive(false);
    }    
    void OnTutorialOn(panels _panel)
    {
        SetOff();
        switch (_panel)
        {
            case panels.HURDLES: 
                if (!Data.Instance.userData.ready_tutorial_hurdles) 
                {
                    tutorial_hurdles.SetActive(true); SetOn();
                    Data.Instance.userData.ready_tutorial_hurdles = true;
                }
                break;
            case panels.MUD:
                if (!Data.Instance.userData.ready_tutorial_mud) 
                {
                    tutorial_mud.SetActive(true); SetOn();
                    Data.Instance.userData.ready_tutorial_mud = true;
                } 
                break;
            case panels.RUN:
                if (!Data.Instance.userData.ready_tutorial_run) 
                {
                    tutorial_run.SetActive(true); SetOn();
                    Data.Instance.userData.ready_tutorial_run = true;
                } 
                break;
            case panels.TRAMPOLIN:
                if (!Data.Instance.userData.ready_tutorial_trampolin) 
                {
                    tutorial_trampolin.SetActive(true); SetOn();
                    Data.Instance.userData.ready_tutorial_trampolin = true;
                } 
                break;
            case panels.WALLS:
                if (!Data.Instance.userData.ready_tutorial_walls) 
                {
                    tutorial_walls.SetActive(true); SetOn();
                    Data.Instance.userData.ready_tutorial_walls = true;
                } 
                break;
        }
    }
    void SetOn()
    {
        Time.timeScale = 0;
        panel.SetActive(true);
    }
    public void Close()
    {
        print("Close");
        panel.SetActive(false);
        Time.timeScale = 1;
    }
}
