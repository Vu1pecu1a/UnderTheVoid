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
        Debug.Log("플레이어 등장");
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

     //   Debug.Log("플레이어 아이들");
        if (D_calcuate.i.MonsterList.Count == 0)
            return;

        monsters.Clear();
        monsters.Add(D_calcuate.i.MonsterList[0]);

        for (int i=1;i< D_calcuate.i.MonsterList.Count;i++)
        {
            monsters.Add(D_calcuate.i.MonsterList[i]);

            float a = Vector3.Distance(this.transform.position, D_calcuate.i.MonsterList[i].transform.position);//지금 추가된 몬스터와의 거리
            float b = Vector3.Distance(this.transform.position, monsters[i-1].transform.position);//마지막에 추가된 몬스터와의 거리
            if (a<b)
            {
                for (int j = 1; j < monsters.Count; j++)
                {
                    //여기까지가 스왑
                    float alfa = Vector3.Distance(this.transform.position, monsters[monsters.Count-j-1].transform.position);//바꾼 객체
                    float beta = Vector3.Distance(this.transform.position, monsters[monsters.Count-j].transform.position);//바꿔야하는 객체

                    if (alfa<beta)
                    break;

                    MonsterBase tmp = monsters[monsters.Count - j];//바꿔야할 대상 1
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