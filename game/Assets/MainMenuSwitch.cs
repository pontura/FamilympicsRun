using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuSwitch : MonoBehaviour {

    private bool isOn = true;
    private Animation anim;

    public Image bg;
    public Color color1;
    public Color color2;

    void Start()
    {
        anim = GetComponent<Animation>();
    }
   public void Switch(int id)
    {
        if (isOn)
        {
            GetComponent<AnimationExtensions>().Play(anim, "SwitchOff", false, () => Debug.Log("onComplete"));
            bg.color = color2;
        }
        else
        {
            GetComponent<AnimationExtensions>().Play(anim, "SwitchOn", false, () => Debug.Log("onComplete"));
            bg.color = color1;
        }
        
        isOn = !isOn;

        SetAction(id, isOn);
    }
   void SetAction(int id, bool active)
   {
       switch (id)
       {
           case 1: Data.Instance.musicManager.Turn(active); break;
           case 2: Data.Instance.soundManager.Turn(active); break;
       }
   }
}
