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

            float a = Vector3.Distance(this.transform.position, D_calcuate.i.PlayerList[i].transform.position);//지금 추가된 몬스터와의 거리

            float b = Vector3.Distance(this.transform.position, players[i - 1].transform.position);//마지막에 추가된 몬스터와의 거리

            if (a < b)
            {
                for (int j = 1; j < players.Count; j++)
                {
                    MonsterBase tmp = players[players.Count - j];//바꿔야할 대상
                    players[players.Count - j - 1] = players[players.Count - j];
                    players[players.Count - j] = tmp;
                    //여기까지가 스왑
                    float alfa = Vector3.Distance(this.transform.position, players[players.Count - j].transform.position);//바꾸고 있는 객체
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
