using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] GameObject UI_Char;
    [SerializeField] GameObject UI_inven;
    [SerializeField] Transform _Maininven;
    [SerializeField] GameObject Char;
    [SerializeField] GameObject BattelUI;
    [SerializeField] GameObject ButtonUI;
    [SerializeField] GameObject GameOver_UI;
    [SerializeField] GameObject GameWin_UI;
    [SerializeField, Tooltip("�ð� ����")] TextMeshProUGUI TimeScale;

    public Transform RewardPanel;


    [Header("��ư")]
    [SerializeField, Tooltip("��ų ���� ������Ʈ")] GameObject SkillToolTip;
    [SerializeField, Tooltip("������ ���� ������Ʈ")] GameObject ItemToolTip;
    [SerializeField, Tooltip("�̴ϸ� ��ư")] Toggle _Minimap;
    [SerializeField, Tooltip("�κ��丮 ��ư")] Toggle _inven;
    public GameObject[] Skill_button;
    [SerializeField] Transform overButton;
    [SerializeField] Transform WinButton;

    public bool isOn { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        isOn = false;
        Managers.instance.Tab += UI_on;
        MapGenerator.i.mapInstanceEvent += UI_on;//�� ���� ���� �̺�Ʈ(�ε� �����)
        overButton.GetComponent<Button>().onClick.AddListener(delegate { ScenecManeger.i.GoScene(0); });
        WinButton.GetComponent<Button>().onClick.AddListener(delegate { ScenecManeger.i.GoScene(0); });
        SkillToolTip.SetActive(false);
        ItemToolTip.SetActive(false);
    }

    /// <summary>
    /// ��ư ���� ������Ʈ Ȱ��ȭ
    /// </summary>
    public void SkillSet()
    {
        for (int i = 0; i < D_calcuate.i.PlayerList.Count; i++)
        {
            Skill_button[i].SetActive(true);
        }
    }
    /// <summary>
    /// ���ӿ��� UI ����
    /// </summary>
    public void UIManager_GameOver()
    {
        GameOver_UI.SetActive(true);
    }
    /// <summary>
    /// ���� �¸� UI ����
    /// </summary>
    public void UIManager_GameWin()
    {
       GameWin_UI.SetActive(true);
    }

    public Transform MainInven()
    {
        return this._Maininven;
    }

    /// <summary>
    /// ��ų��ư ����
    /// </summary>
    /// <param name="player"></param>
    public void LISTEN(PlayerBase player)
    {
        int alfa = D_calcuate.i.PlayerList.FindIndex(s => s == player);
        Skill_button[alfa].GetComponent<Button>().onClick.AddListener(delegate { D_calcuate.i._Playerarray[alfa].ActiveSkillOn(); });
    }

    /// <summary>
    /// ��ų��ư ���� �����
    /// </summary>
    /// <param name="player"></param>
    public void RemoveLISTEN(PlayerBase player)
    {
        int alfa = D_calcuate.i.PlayerList.FindIndex(s => s == player);
        Skill_button[alfa].GetComponent<Button>().onClick.RemoveAllListeners();
    }

    /// <summary>
    /// �̹��� FillAmount ����: ��Ÿ�� 
    /// </summary>
    /// <param name="player"></param>
    /// <param name="a"></param>
    public void FIllSkillCoolTiem(MonsterBase player,float a)
    {
        //int s = D_calcuate.i.PlayerList.FindIndex(s => s == player);
        int s = Array.FindIndex(D_calcuate.i._Playerarray,s => s == player as PlayerBase);
        Managers.instance._UI.Skill_button[s].transform.GetChild(1).GetComponent<Image>().fillAmount = a;
    }

    public void UI_on()
    {
        if (isOn || D_calcuate.isbattel)
        {
            UI_Char.SetActive(false);
            UI_inven.transform.localPosition = new Vector2(3560, -475);
            Char.SetActive(false);
            ItemToolTip.SetActive(false);
            isOn = false;
        }
        else
        {
            UI_Char.SetActive(true);
            UI_inven.transform.localPosition = new Vector2(1560, -475);
            Char.SetActive(true);
            isOn = true;
        }
    }

    public void BattelUIOn(bool isbattle)
    {
        BattelUI.SetActive(isbattle);
    }

    public GameObject SkilltoolTip()
    {
        return SkillToolTip;
    }
    public GameObject ItemtoolTip()
    {
        return ItemToolTip;
    }
    public Toggle minimap()
    {
        _Minimap.SetIsOnWithoutNotify(true);
        return _Minimap;
    }
    public Toggle Inventory()
    {
        return _inven;
    }

    public void SetTime()
    {
        TimeScale.text = "X"+ Time.timeScale.ToString();
    }
}