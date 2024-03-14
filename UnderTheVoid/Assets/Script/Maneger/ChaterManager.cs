using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        UI_Set();
        UI_Set();
        UI_Set();
    }

    
    void UI_Set()
    {
        GameObject playerUI = Instantiate(Player_UI, Player);
        GameObject eQ = Instantiate(Player_EQ, EQ);
        playerUI.GetComponent<Toggle>().onValueChanged.AddListener(eQ.SetActive);
        playerUI.GetComponent<Toggle>().group = Player.GetComponent<ToggleGroup>();
        foreach (Toggle a in eQ.transform.GetChild(1).GetComponentsInChildren<Toggle>())
        {
            a.onValueChanged.AddListener(Skill.gameObject.SetActive);
        }
        eQ.transform.GetChild(4).GetComponent<Toggle>().onValueChanged.AddListener(Stat.gameObject.SetActive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
