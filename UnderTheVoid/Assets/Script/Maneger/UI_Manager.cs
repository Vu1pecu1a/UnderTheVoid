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
    public GameObject[] Skill_button;
    [SerializeField, Tooltip("시간 배율")] TextMeshProUGUI TimeScale;

    [SerializeField, Tooltip("스킬 설명 오브젝트")] GameObject SkillToolTip;
    [SerializeField, Tooltip("아이템 설명 오브젝트")] GameObject ItemToolTip;
    [SerializeField, Tooltip("미니맵 버튼")] Toggle _Minimap;
    [SerializeField, Tooltip("인벤토리 버튼")] Toggle _inven;

    [SerializeField] Transform overButton;

    public bool isOn { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        isOn = false;
        Managers.instance.Tab += UI_on;
        MapGenerator.i.mapInstanceEvent += UI_on;//맵 생성 종료 이벤트(로딩 종료시)
        overButton.GetComponent<Button>().onClick.AddListener(delegate { ScenecManeger.i.GoScene(0); });
        SkillToolTip.SetActive(false);
        ItemToolTip.SetActive(false);
    }

    public void SkillSet()
    {
        for (int i = 0; i < D_calcuate.i.PlayerList.Count; i++)
        {
            Skill_button[i].SetActive(true);
        }
    }
    public void UIManager_GameOver()
    {
        GameOver_UI.SetActive(true);
    }

    public void LISTEN(PlayerBase player)
    {
        int alfa = D_calcuate.i.PlayerList.FindIndex(s => s == player);
        Skill_button[alfa].GetComponent<Button>().onClick.AddListener(delegate { D_calcuate.i.PlayerList[alfa].ActiveSkillOn(); });
    }

    public void RemoveLISTEN(PlayerBase player)
    {
        int alfa = D_calcuate.i.PlayerList.FindIndex(s => s == player);
        Skill_button[alfa].GetComponent<Button>().onClick.RemoveAllListeners();
    }
    public void FIllSkillCoolTiem(MonsterBase player,float a)
    {
        Managers.instance._UI.Skill_button[D_calcuate.i.PlayerList.FindIndex(s => s == player)].transform.GetChild(1).GetComponent<Image>().fillAmount = a;
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