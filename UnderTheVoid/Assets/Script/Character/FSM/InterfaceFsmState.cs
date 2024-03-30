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

