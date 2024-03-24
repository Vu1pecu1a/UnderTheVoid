using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IAbility
{
    /// <summary>
    /// ���
    /// </summary>
    public MonsterBase _MB { get;}

    /// <summary>
    /// ���� ���� �ð�
    /// </summary>
    public float _duration { get; }
    /// <summary>
    /// ���� ���� ȿ�� ƽ
    /// </summary>
    public float _tic { get; }

    /// <summary>
    /// ������ �߰� ������ ȿ��
    /// </summary>
    public void Enchant();

    /// <summary>
    /// ������ ���� �Ǵ� ���� ȿ��
    /// </summary>
    public void EnchantingEffect();

    /// <summary>
    /// ������ ���� �Ǿ��� �� ȿ��
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
