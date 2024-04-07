using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBase : MonsterBase
{

    public List<MonsterBase> monsters = new List<MonsterBase>();
    public List<PlayerBase> player = new List<PlayerBase>();
    public Skill[] Skills = new Skill[2];
    public Skill[] PasiveSkills = new Skill[6];
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

    public void ActiveSkillOn()
    {
        if (!D_calcuate.isbattel || Skills[0] is not ActiveSkill)
            return;

        Skills[0].SkillOn(this);
    }
    public void SkillON() // 버프 스킬
    {
        if (!D_calcuate.isbattel || Skills[1] is not ActiveSkill)
            return;

        Skills[1].SkillOn(this);
    }

    public void SetPassiveSkill(int i, Skill skil)
    {
        if(PasiveSkills[i] is PassiveSkill)
        PasiveSkills[i].SKillOff(this);

        PasiveSkills[i] = skil;
        PasiveSkills[i].SkillOn(this);
    }

    protected override void Update()
    {
        base.Update();
        if (target != null && state != AI_State.Attack)
        {
            Debug.DrawLine(gameObject.transform.position, target.transform.position, Color.green);
        }
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

        if (D_calcuate.i.PlayerList.Count == 0)
            return;
       //player.Clear();
       // player.Add(D_calcuate.i.PlayerList[0]);

        if(aI == AI_TYPE.Heal)
        {
            player.Clear();
            for(int i =0; i< D_calcuate.i.PlayerList.Count;i++)
            {
                PlayerBase _player = D_calcuate.i.PlayerList[i];
                player.Add(_player);
                if(player.Count>1)
                {
                    for(int k = 0;k<player.Count;k++)
                    {
                        float p1hpvalue = player[i - 1].MAXHP / player[i - 1].HP;//첫번째 플레이어
                        float p2hpvalue = player[i].MAXHP / player[i].HP;//2번째 플레이어
                        if (p1hpvalue < p2hpvalue)
                            break;
                        PlayerBase pmp = player[i];
                        player[i] = player[i - 1];
                        player[i - 1] = pmp;
                    }
                }
            }
            target = player[0];
            Debug.Log(target);
        }
        else
        target = monsters[0];
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