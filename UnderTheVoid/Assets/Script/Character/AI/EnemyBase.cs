using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBase : MonsterBase
{
    public List<MonsterBase> players = new List<MonsterBase>();
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    private void OnEnable()
    {
        ResetState();
        D_calcuate.i.MonsterList.Add(this);
        this.DieEvent += RemoveListthis;
        if (this._agent != null)
            this.agent.enabled = true;
    }
    public override void Search()
    {
        if (D_calcuate.i.PlayerList.Count == 0)
            return;

        players.Clear();
        players.Add(D_calcuate.i.PlayerList[0]);

        for (int i = 1; i < D_calcuate.i.PlayerList.Count; i++)
        {
            players.Add(D_calcuate.i.PlayerList[i]);

            float a = Vector3.Distance(this.transform.position, D_calcuate.i.PlayerList[i].transform.position);//���� �߰��� ���Ϳ��� �Ÿ�

            float b = Vector3.Distance(this.transform.position, players[i - 1].transform.position);//�������� �߰��� ���Ϳ��� �Ÿ�

            if (a < b)
            {
                for (int j = 1; j < players.Count; j++)
                {
                    MonsterBase tmp = players[players.Count - j];//�ٲ���� ���
                    players[players.Count - j - 1] = players[players.Count - j];
                    players[players.Count - j] = tmp;
                    //��������� ����
                    float alfa = Vector3.Distance(this.transform.position, players[players.Count - j].transform.position);//�ٲٰ� �ִ� ��ü
                    float beta = Vector3.Distance(this.transform.position, players[players.Count - j - 1].transform.position);

                    if (alfa > beta)
                        break;
                }
            }
        }
        target = players[0];
        //Debug.Log(target.name);
    }

    void RemoveListthis()
    {
        D_calcuate.i.MonsterList.Remove(this);
        this.agent.enabled= false;
    }

    private void OnDisable()
    {
        if(D_calcuate.i.MonsterList.Contains(this))
        D_calcuate.i.MonsterList.Remove(this);
    }
}
