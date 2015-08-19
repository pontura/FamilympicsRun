using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProfilePicture : MonoBehaviour
{
    //public void Init(Hiscores.Hiscore data)
    //{
    //    setPicture(data.facebookID);
    //}
    public void setPicture(string facebookID)
    {
        if(Data.Instance.userData.mode == UserData.modes.SINGLEPLAYER && !Data.Instance.OnlyMultiplayer)
            StartCoroutine(GetPicture(facebookID));
    }
    IEnumerator GetPicture(string facebookID)
    {
        if (facebookID == "")
            yield break;

       // print("FACEBOOK - GetPicture " + facebookID);

        WWW receivedData = new WWW("https" + "://graph.facebook.com/" + facebookID + "/picture?width=128&height=128");
        yield return receivedData;
        if (receivedData.error == null)
        {
            GetComponent<Image>().sprite = Sprite.Create(receivedData.texture, new Rect(0, 0, 128, 128), Vector2.zero);
        }
        else
        {
            Debug.Log("ERROR trayendo imagen");
        }

    }
}
