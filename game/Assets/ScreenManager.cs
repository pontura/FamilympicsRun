using UnityEngine;
using System.Collections;

public class ScreenManager : MonoBehaviour {

    public Vector3 scale;
    public bool isTablet;
    


    public Vector3 GetUiPlayerButtonsScale()
    {
        float scaleSum;

        if(!isTablet)
            scaleSum = 0.17f;
        else
            scaleSum = -0.2f;

        return new Vector3(scaleSum, scaleSum, scaleSum);
    }
    public void Start()
    {
        string name = SystemInfo.deviceModel;
        if (name.Substring(0, 4) == "iPad")
        {
            isTablet = true;
            scale = new Vector3(0.8F, 0.8F, 0.8F);
        }
        else if (name.Substring(0, 4) == "iPho")
            isTablet = false;
        else
        {
            float screenHeightInInch = Screen.height / Screen.dpi;
            if (screenHeightInInch < 3.1)
            {
                isTablet = false;
            }
            else
            {
                isTablet = true;
            }
        }
        if (isTablet) 
            scale = Vector3.one;
        else 
            scale = new Vector3(1.2f, 1.2f, 1.2f);
    }

}
