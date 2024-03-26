using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterBase;

public abstract class Buff
{   /// <summary>
    /// 버프 내용
    /// </summary>
    /// <param 시전자="pb"></param>
    /// <param 타겟 ="mb"></param>
    /// <param 지속 시간 ="dur"></param>
    /// <param 틱 ="tic"></param>
    public void SetData(MonsterBase pb, MonsterBase mb, float dur = 10, float tic = 0.5f)
    {
        this._Duration = dur;
        this._Tic = tic;
        this._MB = mb;
        this._PB = pb;
    }

    public delegate void BuffEvent<T>(T t);
    public event BuffEvent<Buff> _BuffEvent;

    [SerializeField, Tooltip("지속 시간")] protected float _Duration;
    [SerializeField, Tooltip("버프 효과 틱")] protected float _Tic;
    [SerializeField, Tooltip("타겟")] protected MonsterBase _MB;
    [SerializeField, Tooltip("시전자")] protected MonsterBase _PB;

    public bool isTimeOver = false;

    public IEnumerator Tic()
    {
        Enchant();
        while (_Duration > 0)
        {
            _Duration -= 0.1f;

            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("시간 코루틴 종료");
        if(!isTimeOver)
            RemoveEffect();
        else
            DisEnchant();
    }

    public IEnumerator TicofEffect()
    {
        while(_Duration> 0)
        {
            EnchantingEffect();

            yield return new WaitForSeconds(_Tic);
        }
        Debug.Log("효과 코루틴 종료");
    }

    public void EndofDuration()
    {
        _Duration = 0;
        isTimeOver = true;
    }


    /// <summary>
    /// 버프가 추가 됬을때 효과
    /// </summary>
    public abstract void Enchant();

    /// <summary>
    /// 버프가 지속 되는 동안 효과
    /// </summary>
    public abstract void EnchantingEffect();

    public virtual void RemoveEffect() // 자연적으로 0초가 되었을때 발생하는 분기점
    {
        Debug.Log("상태 종료 이벤트 발생 분기");
        DisEnchant();
    }

    /// <summary>
    /// 버프가 종료 되었을 때 효과
    /// </summary>
    public virtual void DisEnchant()
    {
        if (_Duration > 0)
        {
            _BuffEvent.Invoke(this);
        }
        Debug.Log("상태 이상 종료");
    }
}
public class DotHeal : Buff, IAbility
{

    MonsterBase IAbility._MB { get => _MB; }
    float IAbility._duration { get => _Duration; }
    float IAbility._tic { get => _Tic;}

    public override void Enchant()
    {
        Debug.Log("도트 힐 시작");
    }
    public override void EnchantingEffect()
    {
        D_calcuate.i.ab.DotHeal(_MB);
    }
    public override void DisEnchant()
    {
        base.DisEnchant();
        Debug.Log("도트 힐 종료");
    }
}

public class ADbuff : Buff, IAbility
{
    MonsterBase IAbility._MB { get => _MB; }
    float IAbility._duration { get => _Duration; }
    float IAbility._tic { get => _Tic; }
    public override void Enchant()
    {
        Debug.Log("도트 딜 시작");
    }
    public override void EnchantingEffect()
    {
        Debug.Log("도트 딜 하는중");
    }
    public override void DisEnchant()
    {
        base.DisEnchant();
        Debug.Log("도트 딜 종료");
    }
}

public class ADSpeedBuff : Buff, IAbility
{
    MonsterBase IAbility._MB { get => _MB; }
    float IAbility._duration { get => _Duration; }
    float IAbility._tic { get => _Tic; }

    public override void Enchant()
    {
        Debug.Log("도트 딜 시작");
    }
    public override void EnchantingEffect()
    {
        Debug.Log("도트 딜 하는중");
    }
    public override void DisEnchant()
    {
        Debug.Log("도트 딜 종료");
    }
}

public class Bleeding : Buff, IAbility
{
    MonsterBase IAbility._MB { get => _MB; }
    float IAbility._duration { get => _Duration; }
    float IAbility._tic { get => _Tic; }
    public override void Enchant()
    {
        Debug.Log("도트 딜 시작");
    }
    public override void EnchantingEffect()
    {
        D_calcuate.i.ab.DotDamage(_PB, _MB);
    }

    public override void RemoveEffect()
    {
        base.RemoveEffect();
        D_calcuate.i.ab.DotHeal(_MB);
    }

    public override void DisEnchant()
    {
        base.DisEnchant();
        Debug.Log("도트 딜 종료");
    }
}
