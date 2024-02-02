public interface InterfaceFsmState<T>
{
    void Enter(T e); // 상태 시작할때 호출

    void Execute(T e);// 상태 진행중일때 호출

    void Exit(T e);//상태 종료일때 호출
}

public interface HitModel
{
    void TakeDamege(DemageModel damageModel);
}

public class DemageModel
{
    public int basedamage;
    public DamageType damageType;

    public DemageModel(int baseDamage,DamageType damageType)
    {
        this.basedamage = baseDamage;
        this.damageType = damageType;
    }
}

public enum DamageType
{
    Slash,
    Bash,
    Stab,
    Fire,
    Freeze,
    Ligthning
}
public class DamageController
{
    public static void DealDamage(HitModel damageable, DemageModel damageModel)
    {
        damageable.TakeDamege(damageModel);
    }
}