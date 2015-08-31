using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DynamicScrollView : MonoBehaviour
{
    public RectTransform scrollContent;
    public Scrollbar scrollBars;
    public int HideScrollHeight;
    private Vector2 size;

    void Start()
    {
        SetScroll(false);
        Events.OnScrollSizeRefresh += OnScrollSizeRefresh;
    }
    void OnDestroy()
    {
        Events.OnScrollSizeRefresh -= OnScrollSizeRefresh;
    }
    void OnScrollSizeRefresh(Vector2 size)
    {
        SetScroll(false);

        this.size = size;
        scrollContent.sizeDelta = size;
        scrollBars.value = 1;
        Invoke("Refresh", 0.05f);
    }
    void Refresh()
    {
        if (HideScrollHeight > size.y)
             SetScroll(false);
        else
            SetScroll(true);

        scrollBars.value = 1;
    }
    void SetScroll(bool enable)
    {
        if (!enable)
        {
            scrollBars.GetComponent<Image>().enabled = false;
            scrollBars.interactable = false;
        }
        else
        {
            scrollBars.GetComponent<Image>().enabled = true;
            scrollBars.interactable = true;
        }
    }
}
