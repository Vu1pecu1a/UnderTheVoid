using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineEnum;

public class UIManager : Tsingleton<UIManager>
{
    /// <summary>
    /// UI��ųʸ�
    /// </summary>
    Dictionary<UIType,WndBase> _uiStorege;

    
    protected override void Init()
    {
        base.Init();
        _uiStorege = new Dictionary<UIType, WndBase>();
    }


    /// <summary>
    /// ���� ���� �ʿ���� UI�� �����մϴ�.
    /// </summary>
    /// <param name="scene"></param>
    public void InitalzeManager(SceneType scene)
    {

    }


    /// <summary>
    /// UI �����/UI SetActive
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
            //����
            WndBase wndbase = CreatWindow(wnd);
            if (wndbase != null)
                _uiStorege.Add(wnd, wndbase);
            else
                Debug.LogFormat("[{0}]��(��) �������� ���߽��ϴ�. Ȯ�ο��", wnd);
        }
    }
    /// <summary>
    /// UI ���� [���� ����]
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
            Debug.Log("�������� ���� �����츦 �������� �߽��ϴ�.");
        }
    }
    /// <summary>
    /// �����ִ��� �ƴ��� Ȯ��
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
            Debug.LogFormat("������ [{0}]��(��) Resources������ �����ϴ�.",wnd.ToString());
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
