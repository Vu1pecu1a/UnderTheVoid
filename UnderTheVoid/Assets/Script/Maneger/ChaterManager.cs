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

    [SerializeField, Tooltip("�÷��̾� UI�� ��ġ�ؾ��� ��")] Transform Player;
    [SerializeField, Tooltip("��� UI�� ��ġ�ؾ��� ��")] Transform EQ;

    [SerializeField, Tooltip("�÷��̾� UI")] GameObject Player_UI;
    [SerializeField, Tooltip("��� UI")] GameObject Player_EQ;
    [SerializeField, Tooltip("���� UI")] GameObject StatTextMeshPro;
    [SerializeField, Tooltip("������ ���� UI")] GameObject SimpleTMPro;
    [SerializeField, Tooltip("��ų ��ư")] GameObject SkillButton;

    [SerializeField, Tooltip("���� �κ��丮 ��ġ")] Transform Inven;
    [SerializeField, Tooltip("��ų ���� UI ��ġ")] Transform Skill;
    [SerializeField, Tooltip("���� UI��ġ")] Transform Stat;
    [SerializeField, Tooltip("���� UI ��ġ")] Transform BattelUI;

    // Start is called before the first frame update
    void Start()
    {
        Managers.instance.E_Escape += PlayerEqisoff;
    }

    /// <summary>
    /// �÷��̾� �ʻ�ȭ/�κ��丮 ����
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
        Managers.instance._UI.SkillSet();
    }
    
    public void PlayerEqisoff()
    {
        foreach(Toggle a in 
        Player.GetComponentsInChildren<Toggle>())
        {
            a.isOn = false;
        }
    }

    /// <summary>
    /// ĳ����-�ʻ�ȭ/�κ��丮 ����
    /// </summary>
    /// <param name="playerBase"></param>
    void UI_Set(PlayerBase playerBase)
    {
        GameObject playerUI = Instantiate(Player_UI, Player);

        playerUI.name = playerBase.ClassName + playerUI.name;

        GameObject eQ = Instantiate(Player_EQ, EQ);

        eQ.name = playerBase.ClassName + eQ.name;
        //���� ����â
        GameObject SimplePortait = Instantiate(SimpleTMPro, BattelUI);

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
        eQ.transform.GetChild(4).GetComponent<Toggle>().onValueChanged.AddListener(Stat.gameObject.SetActive);//����ĭ
        eQ.transform.GetChild(4).GetComponent<Toggle>().onValueChanged.AddListener(delegate { StatView(playerBase); });
        playerUI.GetComponent<Toggle>().onValueChanged.AddListener(delegate { eQ.transform.GetChild(4).GetComponent<Toggle>().SetIsOnWithoutNotify(false); });

        StartCoroutine(CorStatPortrait(playerBase, playerUI));
        StartCoroutine(SimplyCorStatPortrait(playerBase, SimplePortait));
        D_calcuate.i.PlayerData.Add(eQ.transform.GetComponentInChildren<InvenRender>(), new PlayerofData(playerBase,eQ.transform.GetComponentInChildren<InvenRender>(), eQ, playerUI));
        //  Debug.Log(eQ.transform.GetComponentInChildren<InvenRender>());
    }

    

    void SkillStatReSet(bool ina)
    {
        Skill.gameObject.SetActive(false);
        Stat.gameObject.SetActive(false);
    }
    /// <summary>
    /// �нú� ��ų �߰�
    /// </summary>
    /// <param name="playerbase"></param>
    /// <param name="playerUI"></param>
    void PassiveSKillCreat(PlayerBase playerbase, Toggle playerUI)
    {
        Transform skill = Skill.transform.GetComponentInChildren<GridLayoutGroup>().transform;
        Button[] tmp = skill.GetComponentsInChildren<Button>(true);
        foreach (Button a in skill.GetComponentsInChildren<Button>())
        {
            a.gameObject.SetActive(false);
        }
        for (int i = 0; i < D_calcuate.i.AllPassiveSkill.Count; i++)
        {
            tmp[i].gameObject.SetActive(true);
            tmp[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = D_calcuate.i.AllPassiveSkill[i].ToString();
            tmp[i].transform.GetChild(1).GetComponent<Image>().sprite = D_calcuate.i.AllPassiveSkill[i].SkillImage;
            tmp[i].GetComponent<SKillbutton>().togglebu = playerUI;
            tmp[i].GetComponent<SKillbutton>().skill = D_calcuate.i.AllPassiveSkill[i];
            tmp[i].GetComponent<SKillbutton>().playerbase = playerbase;
        }
    }
    /// <summary>
    /// ��Ƽ�� ��ų �߰�
    /// </summary>
    /// <param name="playerbase"></param>
    /// <param name="playerUI"></param>
    void ActiveSKillCreat(PlayerBase playerbase, Toggle playerUI)
    {
        Transform skill = Skill.transform.GetComponentInChildren<GridLayoutGroup>().transform;
        Button[] tmp = skill.GetComponentsInChildren<Button>(true);
        foreach (Button a in skill.GetComponentsInChildren<Button>())
        {
            a.gameObject.SetActive(false);
        }
        for(int i = 0; i < D_calcuate.i.AllActiveSKill.Count; i++)
        {
            tmp[i].gameObject.SetActive(true);
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
        stat.GetChild(0).GetComponent<TextMeshProUGUI>().text =
            " HP :" + playerBase.HP.ToString()
            + "\n ���ݷ� :" + playerBase.ATK.ToString()
            + "\n ���� :" + playerBase.DEF.ToString()
            + "\n ���� �ӵ� :" + playerBase.ATKSpeed.ToString()
            + "\n �̵� �ӵ� :" + playerBase.MoveSpeed.ToString();
       // stat.GetChild(1).gameObject.SetActive(true);
       // stat.GetChild(1).GetComponent<TextMeshProUGUI>().text = "���ݷ� :" + playerBase.ATK.ToString();
    }

    IEnumerator CorStatPortrait(PlayerBase playerBase,GameObject PlayerUI)
    {
        Transform Portrait = PlayerUI.transform.GetChild(0).GetChild(1); // �÷��̾� �ʻ�ȭ ����
        Transform Data = PlayerUI.transform.GetChild(0).GetChild(2); // �÷��̾� ������

        Portrait.GetChild(0).GetComponent<Image>().sprite = ExcelParsingSystem.ClassSprite(playerBase.AI);
        TextMeshProUGUI[] DataList = Data.GetComponentsInChildren<TextMeshProUGUI>();
        while (playerBase != null)
        {
            DataList[0].text = playerBase.ClassName;
            DataList[1].text = "ü�� :"+ playerBase.HP.ToString();
            DataList[2].text = "���ݷ� :" + playerBase.ATK.ToString();
            DataList[3].text = "���� :" + playerBase.DEF.ToString();
            DataList[4].text = "���� �ӵ� :" + playerBase.ATKSpeed.ToString();
            DataList[5].text = "�̵� �ӵ� :" + playerBase.MoveSpeed.ToString();

            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator SimplyCorStatPortrait(PlayerBase playerBase, GameObject SimplyPortrait)
    {
        Transform Portrait = SimplyPortrait.transform.GetChild(1); // 
        Image HPBAR = SimplyPortrait.transform.GetChild(0).GetChild(0).GetComponent<Image>(); // 
        Portrait.GetComponent<Image>().sprite = ExcelParsingSystem.ClassSprite(playerBase.AI);
        TextMeshProUGUI[] DataList = SimplyPortrait.GetComponentsInChildren<TextMeshProUGUI>();

        while (playerBase != null)
        {
            HPBAR.fillAmount = playerBase.hpbarreturn();
            DataList[0].text = "���ݷ� :" + playerBase.ATK.ToString();
            DataList[1].text = "���� :" + playerBase.DEF.ToString();
            DataList[2].text = "���� ���� :" + playerBase.State.ToString();
            DataList[3].text = "���� �ӵ� :" + playerBase.ATKSpeed.ToString();
            DataList[4].text = "�̵� �ӵ� :" + playerBase.MoveSpeed.ToString();

            yield return new WaitForSeconds(0.1f);
        }
    }
}
