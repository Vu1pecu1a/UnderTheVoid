using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] GameObject UI_Char;
    [SerializeField] GameObject UI_inven;
    [SerializeField] GameObject Char;
    [SerializeField] GameObject BattelUI;
    [SerializeField] GameObject ButtonUI;
    [SerializeField] GameObject GameOver_UI;
    [SerializeField] GameObject[] Skill_button;
    [SerializeField, Tooltip("�ð� ����")] TextMeshProUGUI TimeScale;

    [SerializeField, Tooltip("��ų ���� ������Ʈ")] GameObject SkillToolTip;
    [SerializeField, Tooltip("������ ���� ������Ʈ")] GameObject ItemToolTip;
    [SerializeField, Tooltip("�̴ϸ� ��ư")] Toggle _Minimap;
    [SerializeField, Tooltip("�κ��丮 ��ư")] Toggle _inven;

    [SerializeField] Transform overButton;

    public bool isOn { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        isOn = false;
        Managers.instance.Tab += UI_on;
        MapGenerator.i.mapInstanceEvent += UI_on;//�� ���� ���� �̺�Ʈ(�ε� �����)
        overButton.GetComponent<Button>().onClick.AddListener(delegate { ScenecManeger.i.GoScene(0); });
        Skill_button[0].GetComponent<Button>().onClick.AddListener(delegate { D_calcuate.i.PlayerList[0].ActiveSkillOn(); });
        Skill_button[1].GetComponent<Button>().onClick.AddListener(delegate { D_calcuate.i.PlayerList[1].ActiveSkillOn(); });
        Skill_button[2].GetComponent<Button>().onClick.AddListener(delegate { D_calcuate.i.PlayerList[2].ActiveSkillOn(); });
        SkillToolTip.SetActive(false);
        ItemToolTip.SetActive(false);
    }


    public void UIManager_GameOver()
    {
        GameOver_UI.SetActive(true);
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