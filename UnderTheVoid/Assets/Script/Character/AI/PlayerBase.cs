using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBase : MonsterBase
{

    public List<MonsterBase> monsters = new List<MonsterBase>();

    protected override void Start()
    {
        base.Start();
        Debug.Log("�÷��̾� ����");
    }
    private void OnEnable()
    {
        D_calcuate.i.PlayerList.Add(this);
        this.DieEvent += RemoveListthis;
       if(this._agent!=null)
        this.agent.enabled = true;
    }



    public override void Search()
    {

     //   Debug.Log("�÷��̾� ���̵�");
        if (D_calcuate.i.MonsterList.Count == 0)
            return;

        monsters.Clear();
        monsters.Add(D_calcuate.i.MonsterList[0]);

        for (int i=1;i< D_calcuate.i.MonsterList.Count;i++)
        {
            monsters.Add(D_calcuate.i.MonsterList[i]);

            float a = Vector3.Distance(this.transform.position, D_calcuate.i.MonsterList[i].transform.position);//���� �߰��� ���Ϳ��� �Ÿ�
            float b = Vector3.Distance(this.transform.position, monsters[i-1].transform.position);//�������� �߰��� ���Ϳ��� �Ÿ�
            if (a<b)
            {
                for (int j = 1; j < monsters.Count; j++)
                {
                    //��������� ����
                    float alfa = Vector3.Distance(this.transform.position, monsters[monsters.Count-j-1].transform.position);//�ٲ� ��ü
                    float beta = Vector3.Distance(this.transform.position, monsters[monsters.Count-j].transform.position);//�ٲ���ϴ� ��ü

                    if (alfa<beta)
                    break;

                    MonsterBase tmp = monsters[monsters.Count - j];//�ٲ���� ��� 1
                    monsters[monsters.Count - j] = monsters[monsters.Count - j - 1];
                    monsters[monsters.Count - j - 1] = tmp;//2
                }
            }
        }
        target = monsters[0];
        Debug.DrawLine(gameObject.transform.position, target.gameObject.transform.position, Color.blue);
        // Debug.Log(target.name);
    }

    public override void TakeDamege(DemageModel damageModel)
    {
        base.TakeDamege(damageModel);
    }

    private void OnDisable()
    {
        D_calcuate.i.PlayerList.Remove(this);
        this.agent.enabled = false;
    }
    void RemoveListthis()
    {
        D_calcuate.i.PlayerList.Remove(this);
        this.agent.enabled = false;
    }
}