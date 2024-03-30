using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField, Tooltip("스탯 UI")] GameObject StatTextMeshPro;
    [SerializeField, Tooltip("스킬 버튼")] GameObject SkillButton;

    [SerializeField, Tooltip("공용 인벤토리 위치")] Transform Inven;
    [SerializeField, Tooltip("스킬 선택 UI 위치")] Transform Skill;
    [SerializeField, Tooltip("스탯 UI위치")] Transform Stat;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    /// <summary>
    /// 플레이어 초상화/인벤토리 생성
    /// </summary>
    public void playerSpawn()
    {
        for (int j = 0; j < D_calcuate.i.AllPassiveSkill.Count + D_calcuate.i.AllActiveSKill.Count; j++)
        {
            Transform skill = Skill.transform.GetComponentInChildren<GridLayoutGroup>().transform;
            Instantiate(SkillButton, skill);    
        }

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

        Toggle[] a = eQ.transform.GetChild(1).GetComponentsInChildren<Toggle>();
        foreach(Toggle b in a)
        { 
            b.onValueChanged.AddListener(Skill.gameObject.SetActive);
            playerUI.GetComponent<Toggle>().onValueChanged.AddListener(delegate { b.SetIsOnWithoutNotify(false); });
            if(b == a[0] || b == a[1])
                b.onValueChanged.AddListener(delegate { ActiveSKillCreat(playerBase, b); });
            else
                b.onValueChanged.AddListener(delegate { PassiveSKillCreat(playerBase, b); });
        }
        eQ.transform.GetChild(4).GetComponent<Toggle>().onValueChanged.AddListener(Stat.gameObject.SetActive);//스탯칸
        eQ.transform.GetChild(4).GetComponent<Toggle>().onValueChanged.AddListener(delegate { StatView(playerBase); });
        playerUI.GetComponent<Toggle>().onValueChanged.AddListener(delegate { eQ.transform.GetChild(4).GetComponent<Toggle>().SetIsOnWithoutNotify(false); });

        StartCoroutine(CorStatPortrait(playerBase, playerUI));

        D_calcuate.i.PlayerData.Add(eQ.transform.GetComponentInChildren<InvenRender>(), new PlayerofData(playerBase,eQ.transform.GetComponentInChildren<InvenRender>(), eQ, playerUI));
        //  Debug.Log(eQ.transform.GetComponentInChildren<InvenRender>());
    }

    

    void SkillStatReSet(bool ina)
    {
        Skill.gameObject.SetActive(false);
        Stat.gameObject.SetActive(false);
    }
    /// <summary>
    /// 패시브 스킬 추가
    /// </summary>
    /// <param name="playerbase"></param>
    /// <param name="playerUI"></param>
    void PassiveSKillCreat(PlayerBase playerbase, Toggle playerUI)
    {
        Transform skill = Skill.transform.GetComponentInChildren<GridLayoutGroup>().transform;
        foreach (Button tmp in skill.GetComponentsInChildren<Button>())
        {
            tmp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = D_calcuate.i.AllPassiveSkill[0].ToString();
            tmp.transform.GetChild(1).GetComponent<Image>().sprite = D_calcuate.i.AllPassiveSkill[0].SkillImage;
            tmp.GetComponent<SKillbutton>().togglebu = playerUI;
            tmp.GetComponent<SKillbutton>().skill = D_calcuate.i.AllPassiveSkill[0];
            tmp.GetComponent<SKillbutton>().playerbase = playerbase;
        }
    }
    /// <summary>
    /// 액티브 스킬 추가
    /// </summary>
    /// <param name="playerbase"></param>
    /// <param name="playerUI"></param>
    void ActiveSKillCreat(PlayerBase playerbase, Toggle playerUI)
    {
        Transform skill = Skill.transform.GetComponentInChildren<GridLayoutGroup>().transform;
        Button[] tmp = skill.GetComponentsInChildren<Button>();
        //foreach (Button tmp in skill.GetComponentsInChildren<Button>())
        for(int i = 0; i < D_calcuate.i.AllActiveSKill.Count; i++)
        {
            tmp[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = D_calcuate.i.AllActiveSKill[i].ToString();
            tmp[i].transform.GetChild(1).GetComponent<Image>().sprite = D_calcuate.i.AllActiveSKill[i].SkillImage;
            tmp[i].GetComponent<SKillbutton>().togglebu = playerUI;
            tmp[i].GetComponent<SKillbutton>().skill = D_calcuate.i.AllActiveSKill[i];
            tmp[i].GetComponent<SKillbutton>().playerbase = playerbase;
        }
    }

    void StatView(PlayerBase playerBase)
    {
        Transform stat = Stat.transform.GetComponentInChildren<GridLayoutGroup>().transform;

        foreach(TextMeshProUGUI tmp in stat.GetComponentsInChildren<TextMeshProUGUI>())
        {
            tmp.gameObject.SetActive(false);
        }

        stat.GetChild(0).gameObject.SetActive(true);
        stat.GetChild(0).GetComponent<TextMeshProUGUI>().text ="HP :"+playerBase.HP.ToString();
        stat.GetChild(1).gameObject.SetActive(true);
        stat.GetChild(1).GetComponent<TextMeshProUGUI>().text = "공격력 :" + playerBase.ATK.ToString();
    }

    IEnumerator CorStatPortrait(PlayerBase playerBase,GameObject PlayerUI)
    {
        while(playerBase != null)
        {
            Transform Portrait = PlayerUI.transform.GetChild(0).GetChild(1); // 플레이어 초상화 관련
            Transform Data = PlayerUI.transform.GetChild(0).GetChild(2); // 플레이어 데이터

            Portrait.GetChild(0).GetComponent<Image>().color = Color.white;
            TextMeshProUGUI[] DataList = Data.GetComponentsInChildren<TextMeshProUGUI>();
            DataList[0].text = playerBase.name;
            DataList[1].text = "체력 :"+ playerBase.HP.ToString();
            DataList[2].text = "공격력 :" + playerBase.ATK.ToString();
            DataList[3].text = "방어력 :" + playerBase.DEF.ToString();
            DataList[4].text = "공격 속도 :" + playerBase.ATKSpeed.ToString();
            DataList[5].text = "이동 속도 :" + playerBase.MoveSpeed.ToString();

            yield return new WaitForSeconds(0.1f);
        }
    }

}
