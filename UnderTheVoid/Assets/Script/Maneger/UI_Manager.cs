using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] Transform overButton;

    bool isOn = false;

    // Start is called before the first frame update
    void Start()
    {
        Managers.instance.Tab += UI_on;
        MapGenerator.i.mapInstanceEvent += UI_on;//맵 생성 종료 이벤트(로딩 종료시)
        overButton.GetComponent<Button>().onClick.AddListener(delegate { ScenecManeger.i.GoScene(0); });
        Skill_button[0].GetComponent<Button>().onClick.AddListener(delegate { D_calcuate.i.PlayerList[0].ActiveSkillOn(); });
        Skill_button[1].GetComponent<Button>().onClick.AddListener(delegate { D_calcuate.i.PlayerList[1].ActiveSkillOn(); });
        Skill_button[2].GetComponent<Button>().onClick.AddListener(delegate { D_calcuate.i.PlayerList[2].ActiveSkillOn(); });
    }


    public void UIManager_GameOver()
    {
        GameOver_UI.SetActive(true);
    }

    public void UI_on()
    {
        if (isOn || D_calcuate.isbattel)
        {
            Debug.Log(isOn);
            UI_Char.SetActive(false);
            UI_inven.transform.localPosition = new Vector2(3560, -475);
            Char.SetActive(false);
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
}