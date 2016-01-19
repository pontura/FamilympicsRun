using UnityEngine;
using System.Collections;
using Facebook.Unity;

public class ShareAdvice : MonoBehaviour {

    public GameObject panel;

	void Start () {
        panel.transform.localScale = Data.Instance.screenManager.scale;
        panel.SetActive(false);
        Events.OnFacebookNotConnected += OnFacebookNotConnected;
	}
    void OnDestroy()
    {
        Events.OnFacebookNotConnected -= OnFacebookNotConnected;
    }
    void OnFacebookNotConnected()
    {
        panel.transform.localScale = Data.Instance.screenManager.scale;
        panel.SetActive(true);
        panel.GetComponent<Animation>().Play("PopupOn");
    }

    public void Login()
    {
        Data.Instance.loginManager.FBLogin();
        Close();
    }
    public void Close()
    {
        panel.GetComponent<Animation>().Play("PopupOff");
        Invoke("CloseOff", 0.2f);
    }
    void CloseOff()
    {
        panel.SetActive(false);
    }
}
