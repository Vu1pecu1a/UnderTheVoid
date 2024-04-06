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

    public void SetData(MonsterBase pb, MonsterBase mb, float dur = 10, float tic = 0.5f);
}

/// <summary>
/// ȿ�� ������
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

    public void DotDamage(MonsterBase pb,MonsterBase mb)//��Ʈ��
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
    /// ��� ���� ���� ����
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
