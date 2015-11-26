using UnityEngine;
using System.Collections;

public class LoadingMask : MonoBehaviour {

    public GameObject panel;

    void Start()
    {
        panel.SetActive(false);
        Events.OnLoading += OnLoading;
    }
    void OnLoading(bool active)
    {
        panel.SetActive(active);
    }
}
