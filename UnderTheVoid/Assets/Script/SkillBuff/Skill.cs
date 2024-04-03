using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#region[스킬 베이스]
public class PassiveSkill : Skill
{
    [SerializeField, Tooltip("스킬 쿨타임")] protected float _coolTime;
    [SerializeField, Tooltip("스킬 이미지")] protected Sprite _sprite;
    [SerializeField, Tooltip("스킬 이름")] protected string _name;
    [SerializeField, Tooltip("스킬 설명")] protected string _skillInfo;
    [SerializeField, Tooltip("스킬 배율")] protected string _mutiple;
    public override float CoolTIme { get => _coolTime; }
    public override Sprite SkillImage { get => _sprite; }

    public override void SkillOn(MonsterBase pb)
    {

    }
    public override void SKillOff(MonsterBase pb)
    {

    }

    public override string _Name { get=> _name; }
    public override string _Info { get => _skillInfo; }
    public override string _Multiplier { get => _mutiple; }
}
public class ActiveSkill : Skill
{
    [SerializeField, Tooltip("스킬 쿨타임")] protected float _coolTime;
    [SerializeField, Tooltip("스킬 이미지")] protected Sprite _sprite;
    [SerializeField, Tooltip("스킬 이름")] protected string _name;
    [SerializeField, Tooltip("스킬 설명")] protected string _skillInfo;
    [SerializeField, Tooltip("스킬 배율")] protected string _mutiple;
    public bool isReady = true;
    public override float CoolTIme { get => _coolTime; }
    public override Sprite SkillImage { get => _sprite; }
    public override void SkillOn(MonsterBase pb)
    {

    }
    public override void SKillOff(MonsterBase pb)
    {

    }
    public override string _Name { get => _name; }
    public override string _Info { get=> _skillInfo; }
    public override string _Multiplier { get => _mutiple; }
}
public abstract class Skill:ICloneable
{
    public abstract float CoolTIme { get; }
    public abstract Sprite SkillImage { get; }
    public abstract void SkillOn(MonsterBase pb);//스킬 사용 효과

    public abstract void SKillOff(MonsterBase pb);//스킬 종료 효과

    public abstract string _Name { get; }
    public abstract string _Info { get; }
    public abstract string _Multiplier { get; }
    public virtual object Clone()
    {
        throw new NotImplementedException();
    }
}
#endregion[스킬 베이스]

#region[액티브 스킬]
public class RapidFire : ActiveSkill,ICloneable
{
    public RapidFire(float coolTime, Sprite sprite,string name,string info,string multiple)
    {
        _coolTime = coolTime;
        _sprite = sprite;
        _name = name;
        _skillInfo = info;
        _mutiple= multiple;
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
        RapidFire rp = new RapidFire(_coolTime, _sprite,_name,_skillInfo,_mutiple);
        return rp;
    }
}
public class FireBall : ActiveSkill
{
    MonsterBase _MB;
    DCCheck dc;
    public FireBall(float coolTime, Sprite sprite, string name, string info, string multiple)
    {
        _coolTime = coolTime;
        _sprite = sprite;
        _name = name;
        _skillInfo = info;
        _mutiple = multiple;
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
        FireBall rp = new FireBall(_coolTime, _sprite,_name,_skillInfo,_mutiple);
        return rp;
    }
}
#endregion[액티브 스킬]

#region[패시브 스킬]
public class RapidFireReinforce : PassiveSkill
{
    public RapidFireReinforce(Sprite sprite, string name, string info, string multiple)
    {
        _sprite = sprite;
        _name = name;
        _skillInfo = info;
        _mutiple = multiple;
    }

    public override void SkillOn(MonsterBase pb)
    {
        D_calcuate.i.ab.ADSpeedPlus(pb, 0.3f);
    }
    public override void SKillOff(MonsterBase pb)
    {
        D_calcuate.i.ab.ADSpeedPlus(pb, -0.3f);
    }
    public override object Clone()
    {
        RapidFireReinforce rp = new RapidFireReinforce(_sprite, _name, _skillInfo, _mutiple);
        return rp;
    }
}
#endregion[패시브 스킬]