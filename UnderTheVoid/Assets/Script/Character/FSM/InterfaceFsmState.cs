public interface InterfaceFsmState<T>
{
    void Enter(T e); // ���� �����Ҷ� ȣ��

    void Execute(T e);// ���� �������϶� ȣ��

    void Exit(T e);//���� �����϶� ȣ��
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