using System.Collections;
using System.Collections.Generic;
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
    
    [SerializeField, Tooltip("���� �κ��丮 ��ġ")] Transform Inven;
    [SerializeField, Tooltip("��ų ���� UI ��ġ")] Transform Skill;
    [SerializeField, Tooltip("���� UI��ġ")] Transform Stat;

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
