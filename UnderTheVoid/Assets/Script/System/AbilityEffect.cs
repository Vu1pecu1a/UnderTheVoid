using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    /// <summary>
    /// 버프가 추가 됬을때 효과
    /// </summary>
    public void Enchant();

    /// <summary>
    /// 버프가 지속 되는 동안 효과
    /// </summary>
    public void EnchantingEffect();

    /// <summary>
    /// 버프가 종료 되었을 때 효과
    /// </summary>
    public void DisEnchant();
}


public class AbilityEffect
{
    public void ADPlus(MonsterBase pb)
    {
        pb.ATK += 1;
    }

    public void ADMinus(MonsterBase pb)
    {
        pb.ATK -= 1;
    }

    public void DotHeal(MonsterBase pb)
    {
        DamageController.DealDamage(pb, D_calcuate.i.Heal(pb.ATK), pb.transform);
    }
}
