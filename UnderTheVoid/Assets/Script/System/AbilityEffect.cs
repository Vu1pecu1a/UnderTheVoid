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

    public void ADPlus(MonsterBase pb, float BuffIntensity)
    {
        pb.ATK += (int)(pb.ATK * BuffIntensity);
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
        pb.attackSpeed += pb.attackSpeed * BuffIntensity;
    }

}
