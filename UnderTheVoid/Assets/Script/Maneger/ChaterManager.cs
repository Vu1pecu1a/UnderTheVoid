using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ChaterManager : MonoBehaviour
{
    [SerializeField] Transform UI;
    [SerializeField] GameObject invenPrefab;

    [SerializeField, Tooltip("플레이어 UI가 위치해야할 곳")] Transform Player;
    [SerializeField, Tooltip("장비 UI가 위치해야할 곳")] Transform EQ;


    [SerializeField, Tooltip("플레이어 UI")] GameObject Player_UI;
    [SerializeField, Tooltip("장비 UI")] GameObject Player_EQ;
    
    [SerializeField, Tooltip("공용 인벤토리 위치")] Transform Inven;
    [SerializeField, Tooltip("스킬 선택 UI 위치")] Transform Skill;
    [SerializeField, Tooltip("스탯 UI위치")] Transform Stat;

    // Start is called before the first frame update
    void Start()
    {
       
    }
    public void playerSpawn()
    {
        for (int i = 0; i < D_calcuate.i.PlayerList.Count; i++)
        {
            UI_Set(D_calcuate.i.PlayerList[i]);
        }
    }
    
    void UI_Set(PlayerBase playerBase)
    {
        GameObject playerUI = Instantiate(Player_UI, Player);

        playerUI.name = playerBase.name + playerUI.name;

        GameObject eQ = Instantiate(Player_EQ, EQ);

        eQ.name = playerBase.name + eQ.name;

        playerUI.GetComponent<Toggle>().onValueChanged.AddListener(eQ.SetActive);
        playerUI.GetComponent<Toggle>().onValueChanged.AddListener(SkillStatReSet);
        playerUI.GetComponent<Toggle>().group = Player.GetComponent<ToggleGroup>();
        foreach (Toggle a in eQ.transform.GetChild(1).GetComponentsInChildren<Toggle>())
        {
            a.onValueChanged.AddListener(Skill.gameObject.SetActive);
            playerUI.GetComponent<Toggle>().onValueChanged.AddListener(delegate { a.SetIsOnWithoutNotify(false); });
        }
        eQ.transform.GetChild(4).GetComponent<Toggle>().onValueChanged.AddListener(Stat.gameObject.SetActive);
        playerUI.GetComponent<Toggle>().onValueChanged.AddListener(delegate { eQ.transform.GetChild(4).GetComponent<Toggle>().SetIsOnWithoutNotify(false); });
    }

    void SkillStatReSet(bool ina)
    {
        Skill.gameObject.SetActive(false);
        Stat.gameObject.SetActive(false);
    }

    
}
