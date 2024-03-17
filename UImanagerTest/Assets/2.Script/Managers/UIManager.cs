using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnum;

public class UIManager : Tsingleton<UIManager>
{
    /// <summary>
    /// UI딕셔너리
    /// </summary>
    Dictionary<UIType,WndBase> _uiStorege;

    
    protected override void Init()
    {
        base.Init();
        _uiStorege = new Dictionary<UIType, WndBase>();
    }


    /// <summary>
    /// 씬에 따라 필요없는 UI를 제거합니다.
    /// </summary>
    /// <param name="scene"></param>
    public void InitalzeManager(SceneType scene)
    {

    }


    /// <summary>
    /// UI 만들기/UI SetActive
    /// </summary>
    /// <param name="wnd"></param>
    public void OpenWindow(UIType wnd)
    {
        if( _uiStorege.ContainsKey(wnd))
        {
            _uiStorege[wnd].Open();
        }
        else
        {
            //생성
            WndBase wndbase = CreatWindow(wnd);
            if (wndbase != null)
                _uiStorege.Add(wnd, wndbase);
            else
                Debug.LogFormat("[{0}]이(가) 생성되지 못했습니다. 확인요망", wnd);
        }
    }
    /// <summary>
    /// UI 끄기 [제거 금지]
    /// </summary>
    /// <param name="wnd"></param>
    public void CloseWindow(UIType wnd)
    {

        if (_uiStorege.ContainsKey(wnd))
        {
            _uiStorege[wnd].Close();
        }
        else
        {
            Debug.Log("생성되지 않은 윈도우를 닫으려고 했습니다.");
        }
    }
    /// <summary>
    /// 열려있는지 아닌지 확인
    /// </summary>
    /// <param name="wnd"></param>
    /// <returns></returns>
    public bool IsOpenedWindow(UIType wnd)
    {
        if (_uiStorege.ContainsKey(wnd))
            return _uiStorege[wnd].gameObject.activeSelf;
        else
            return false;
    }

    WndBase CreatWindow(UIType wnd)
    {
        WndBase wndbase = null;
        string Path = "UIprefabs/";
        GameObject prefab = Resources.Load(Path + wnd.ToString()) as GameObject;
        GameObject go = Instantiate(prefab, transform);
        if(go == null)
        {
            Debug.LogFormat("프레펩 [{0}]이(가) Resources폴더에 없습니다.",wnd.ToString());
            return null;
        }

        switch (wnd)
        {
            case UIType.CharacterInfoWnd:
                {
                    CharacterInfoWnd window = go.GetComponent<CharacterInfoWnd>();
                    wndbase = window;
                }
                break;
            case UIType.InventoryWnd:
                {

                }
                break;
            default:
                break;
        }

        return wndbase;
    }

    


   
}
