using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (Data.Instance.lastScene == "LevelSelector")
            {
                Application.Quit();
            }
            else
            {
                Data.Instance.Load("LevelSelector");
            }
        }  
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Events.OnAvatarRun(1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Events.OnAvatarRun(1);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Events.OnAvatarJump(1);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Events.OnAvatarRun(2);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Events.OnAvatarRun(2);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Events.OnAvatarJump(2);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Events.OnAvatarRun(3);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            Events.OnAvatarRun(3);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Events.OnAvatarJump(3);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Events.OnAvatarRun(4);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            Events.OnAvatarRun(4);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            Events.OnAvatarJump(4);
        }
    }
}
