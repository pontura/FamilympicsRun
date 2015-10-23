using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlashWinLap : MonoBehaviour {

    public GameObject panel;
    public Image image;

	void Start () {
        SetOff();
        Events.OnFlashWinLap += OnFlashWinLap;
	}
    void OnDestroy()
    {
        Events.OnFlashWinLap -= OnFlashWinLap;
    }
    void OnFlashWinLap(Color _color)
    {
        Color color = _color;
        color.a = 0.5f;
        panel.SetActive(true);
        image.color = color;
        Invoke("SetOff", 0.1f);
    }
    void SetOff()
    {
        panel.SetActive(false);
    }
}
