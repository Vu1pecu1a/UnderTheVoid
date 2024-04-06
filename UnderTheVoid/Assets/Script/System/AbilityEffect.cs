using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    DotHeal,
    AdBuff,
    AdSpeedBuff,
    Max
}
public enum DeBuffType
{
    Bleeding,
    Max
}



public interface IAbility
{
    /// <summary>
    /// 대상
    /// </summary>
    public MonsterBase _MB { get;}

    /// <summary>
    /// 버프 지속 시간
    /// </summary>
    public float _duration { get; }
    /// <summary>
    /// 버프 지속 효과 틱
    /// </summary>
    public float _tic { get; }

    public void SetData(MonsterBase pb, MonsterBase mb, float dur = 10, float tic = 0.5f);
}

/// <summary>
/// 효과 모음집
/// </summary>
public class AbilityEffect 
{

    public void ADPlus(MonsterBase pb)
    {
        pb.ATK += 1;
    }
    public void ADPlus(MonsterBase pb,int atk)
    {
        pb.ATK += atk;
    }

    public void ADPlus(MonsterBase pb, float BuffIntensity)
    {
        pb.buffatk += BuffIntensity;
    }

    public void ADMinus(MonsterBase pb)
    {
        pb.ATK -= 1;
    }

    public void DotHeal(MonsterBase pb)
    {
        DamageController.DealDamage(pb, D_calcuate.i.Heal(pb.ATK), pb.transform);
    }

    public void DotDamage(MonsterBase pb,MonsterBase mb)//도트딜
    {
        DamageController.DealDamage(mb, D_calcuate.i.Bleeding(pb.ATK), mb.transform);
    }

    public void ADSpeedUP(MonsterBase pb,float BuffIntensity)
    {
        pb.buffattackSpeed += BuffIntensity;
    }

    public void ADSpeedPlus(MonsterBase pb,float BuffIntensity)
    {
        pb.ATKSpeed += BuffIntensity;
    }

    /// <summary>
    /// 모든 배율 버프 제거
    /// </summary>
    /// <param name="pb"></param>
    public void BuffOut(MonsterBase pb)
    {
        pb.buffattackSpeed = 0;
        pb.buffatk = 0;
        pb.buffagi = 0;
        pb.buffdef = 0;
        pb.buffmoveSpeed = 0;
    }
}
