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
        Debug.Log("�÷��̾� ����");
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
        if (!D_calcuate.isbattel)
            return;

        Skills[0].SkillOn(this);
    }
    public void SkillON() // ���� ��ų
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

        if (D_calcuate.i.PlayerList.Count == 0)
            return;
        player.Clear();
        player.Add(D_calcuate.i.PlayerList[0]);

        if(aI == AI_TYPE.Heal)
        {
            target = player[0];
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