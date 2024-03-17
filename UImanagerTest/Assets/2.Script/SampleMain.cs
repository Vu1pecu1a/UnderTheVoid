using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnum;

public class SampleMain : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        UIManager._instance.InitalzeManager(SceneType.Title);
    }
    public void DownButton1(RectTransform rt)
    {
        rt.anchoredPosition += Vector2.down * 20;
    }
    public void UpButton1(RectTransform rt)
    {
        rt.anchoredPosition += Vector2.up * 20;
    }

    public void ClickButtonCharWindow()
    {
        if (UIManager._instance.IsOpenedWindow(UIType.CharacterInfoWnd))
            UIManager._instance.CloseWindow(UIType.CharacterInfoWnd);
        else
            UIManager._instance.OpenWindow(UIType.CharacterInfoWnd);
    }
}
