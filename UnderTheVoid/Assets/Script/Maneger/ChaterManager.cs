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
    
    [SerializeField, Tooltip("���� �κ��丮 ��ġ")] Transform Inven;
    [SerializeField, Tooltip("��ų ���� UI ��ġ")] Transform Skill;
    [SerializeField, Tooltip("���� UI��ġ")] Transform Stat;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    /// <summary>
    /// �÷��̾� �ʻ�ȭ/�κ��丮 ����
    /// </summary>
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
        eQ.transform.GetChild(4).GetComponent<Toggle>().onValueChanged.AddListener(Stat.gameObject.SetActive);//����ĭ
        eQ.transform.GetChild(4).GetComponent<Toggle>().onValueChanged.AddListener(delegate { StatView(playerBase); });
        playerUI.GetComponent<Toggle>().onValueChanged.AddListener(delegate { eQ.transform.GetChild(4).GetComponent<Toggle>().SetIsOnWithoutNotify(false); });

        D_calcuate.i.PlayerData.Add(eQ.transform.GetComponentInChildren<InvenRender>(), new PlayerofData(playerBase,eQ.transform.GetComponentInChildren<InvenRender>(), eQ, playerUI));
        //  Debug.Log(eQ.transform.GetComponentInChildren<InvenRender>());
    }

    void SkillStatReSet(bool ina)
    {
        Skill.gameObject.SetActive(false);
        Stat.gameObject.SetActive(false);
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
        stat.GetChild(1).GetComponent<TextMeshProUGUI>().text = "���ݷ� :" + playerBase.ATK.ToString();
    }
}
