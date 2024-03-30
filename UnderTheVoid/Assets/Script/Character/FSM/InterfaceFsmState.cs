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

public struct DemageModel
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
    Slash =0,
    Bash,
    Stab,
    Fire,
    Freeze,
    Ligthning,
    Heal
}

public enum AI_TYPE
{
    Melee,
    Range,
    Heal
}

public enum AI_State
{
    Idel,
    Walk,
    Attack,
    SpellCast,
    OnSkill,
    Die
}

public enum itemRotae
{
    up = 0,
    right = 90,
    down = 180,
    left = -90
}

