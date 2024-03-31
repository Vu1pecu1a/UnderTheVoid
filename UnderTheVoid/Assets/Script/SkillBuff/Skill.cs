using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#region[��ų ���̽�]
public class PassiveSkill : Skill
{
    [SerializeField, Tooltip("��ų ��Ÿ��")] protected float _coolTime;
    [SerializeField, Tooltip("��ų �̹���")] protected Sprite _sprite;

    public override float CoolTIme { get => _coolTime; }
    public override Sprite SkillImage { get => _sprite; }

    public override void SkillOn(MonsterBase pb)
    {

    }
    public override void SKillOff(MonsterBase pb)
    {

    }
}
public class ActiveSkill : Skill
{
    [SerializeField, Tooltip("��ų ��Ÿ��")] protected float _coolTime;
    [SerializeField, Tooltip("��ų �̹���")] protected Sprite _sprite;
    public bool isReady = true;
    public override float CoolTIme { get => _coolTime; }
    public override Sprite SkillImage { get => _sprite; }
    public override void SkillOn(MonsterBase pb)
    {

    }
    public override void SKillOff(MonsterBase pb)
    {

    }
}
public abstract class Skill:ICloneable
{
    public abstract float CoolTIme { get; }
    public abstract Sprite SkillImage { get; }
    public abstract void SkillOn(MonsterBase pb);//��ų ��� ȿ��

    public abstract void SKillOff(MonsterBase pb);//��ų ���� ȿ��

    public virtual object Clone()
    {
        throw new NotImplementedException();
    }
}
#endregion[��ų ���̽�]

#region[��Ƽ�� ��ų]
public class RapidFire : ActiveSkill,ICloneable
{
    public RapidFire(float coolTime, Sprite sprite)
    {
        _coolTime = coolTime;
        _sprite = sprite;
    }
    public override void SkillOn(MonsterBase pb)
    {
        pb.AddBuff(pb, pb, BuffType.AdSpeedBuff, 60, 60);
    }

    public override void SKillOff(MonsterBase pb)
    {

    }
    public override object Clone()
    {
        RapidFire rp = new RapidFire(_coolTime, _sprite);
        return rp;
    }
}
public class FireBall : ActiveSkill
{
    MonsterBase _MB;
    DCCheck dc;
    public FireBall(float coolTime, Sprite sprite)
    {
        _coolTime = coolTime;
        _sprite = sprite;
    }
    public override void SkillOn(MonsterBase pb)
    {
        if (!isReady || !D_calcuate.isbattel) return;

        _MB = pb;
        pb.ChageState(OnSkill.Instance);
        Debug.Log(OnSkill.Instance + D_calcuate.isbattel.ToString());
        pb.SkillCoolTime(this);
        GameObject effecti = ObjPoolManager.i.InstantiateAPS("FireBall", null);
        pb.Throwprojectile(effecti, D_calcuate.i.FireBallHit(pb.ATK));
        dc = effecti.GetComponent<DCCheck>();
        dc.Hit += FireBallExplosion;
    }

    void FireBallExplosion(GameObject gm)
    {
       GameObject effecti = ObjPoolManager.i.InstantiateAPS("FireBallLateEffect", null);
        effecti.GetComponent<DCCheck>().Penetrate = true;
        _MB.effectSet(effecti,gm,D_calcuate.i.FireBallExplosion(_MB.ATK));
        dc.Hit -= FireBallExplosion;
    }

    public override void SKillOff(MonsterBase pb)
    {
    }
    public override object Clone()
    {
        FireBall rp = new FireBall(_coolTime, _sprite);
        return rp;
    }
}
#endregion[��Ƽ�� ��ų]

#region[�нú� ��ų]
public class RapidFireReinforce : PassiveSkill
{
    public RapidFireReinforce(Sprite sprite)
    {
        _sprite = sprite;
    }

    public override void SkillOn(MonsterBase pb)
    {
        D_calcuate.i.ab.ADSpeedPlus(pb, 0.3f);
    }
    public override void SKillOff(MonsterBase pb)
    {
        D_calcuate.i.ab.ADSpeedPlus(pb, -0.3f);
    }
}
#endregion[�нú� ��ų]