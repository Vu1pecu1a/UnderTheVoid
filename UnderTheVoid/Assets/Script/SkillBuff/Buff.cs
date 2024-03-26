using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterBase;

public abstract class Buff
{   /// <summary>
    /// ���� ����
    /// </summary>
    /// <param ������="pb"></param>
    /// <param Ÿ�� ="mb"></param>
    /// <param ���� �ð� ="dur"></param>
    /// <param ƽ ="tic"></param>
    public void SetData(MonsterBase pb, MonsterBase mb, float dur = 10, float tic = 0.5f)
    {
        this._Duration = dur;
        this._Tic = tic;
        this._MB = mb;
        this._PB = pb;
    }

    public delegate void BuffEvent<T>(T t);
    public event BuffEvent<Buff> _BuffEvent;

    [SerializeField, Tooltip("���� �ð�")] protected float _Duration;
    [SerializeField, Tooltip("���� ȿ�� ƽ")] protected float _Tic;
    [SerializeField, Tooltip("Ÿ��")] protected MonsterBase _MB;
    [SerializeField, Tooltip("������")] protected MonsterBase _PB;

    public bool isTimeOver = false;

    public IEnumerator Tic()
    {
        Enchant();
        while (_Duration > 0)
        {
            _Duration -= 0.1f;

            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("�ð� �ڷ�ƾ ����");
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
        Debug.Log("ȿ�� �ڷ�ƾ ����");
    }

    public void EndofDuration()
    {
        _Duration = 0;
        isTimeOver = true;
    }


    /// <summary>
    /// ������ �߰� ������ ȿ��
    /// </summary>
    public abstract void Enchant();

    /// <summary>
    /// ������ ���� �Ǵ� ���� ȿ��
    /// </summary>
    public abstract void EnchantingEffect();

    public virtual void RemoveEffect() // �ڿ������� 0�ʰ� �Ǿ����� �߻��ϴ� �б���
    {
        Debug.Log("���� ���� �̺�Ʈ �߻� �б�");
        DisEnchant();
    }

    /// <summary>
    /// ������ ���� �Ǿ��� �� ȿ��
    /// </summary>
    public virtual void DisEnchant()
    {
        if (_Duration > 0)
        {
            _BuffEvent.Invoke(this);
        }
        Debug.Log("���� �̻� ����");
    }
}
public class DotHeal : Buff, IAbility
{

    MonsterBase IAbility._MB { get => _MB; }
    float IAbility._duration { get => _Duration; }
    float IAbility._tic { get => _Tic;}

    public override void Enchant()
    {
        Debug.Log("��Ʈ �� ����");
    }
    public override void EnchantingEffect()
    {
        D_calcuate.i.ab.DotHeal(_MB);
    }
    public override void DisEnchant()
    {
        base.DisEnchant();
        Debug.Log("��Ʈ �� ����");
    }
}

public class ADbuff : Buff, IAbility
{
    MonsterBase IAbility._MB { get => _MB; }
    float IAbility._duration { get => _Duration; }
    float IAbility._tic { get => _Tic; }
    public override void Enchant()
    {
        Debug.Log("��Ʈ �� ����");
    }
    public override void EnchantingEffect()
    {
        Debug.Log("��Ʈ �� �ϴ���");
    }
    public override void DisEnchant()
    {
        base.DisEnchant();
        Debug.Log("��Ʈ �� ����");
    }
}

public class ADSpeedBuff : Buff, IAbility
{
    MonsterBase IAbility._MB { get => _MB; }
    float IAbility._duration { get => _Duration; }
    float IAbility._tic { get => _Tic; }

    public override void Enchant()
    {
        Debug.Log("��Ʈ �� ����");
    }
    public override void EnchantingEffect()
    {
        Debug.Log("��Ʈ �� �ϴ���");
    }
    public override void DisEnchant()
    {
        Debug.Log("��Ʈ �� ����");
    }
}

public class Bleeding : Buff, IAbility
{
    MonsterBase IAbility._MB { get => _MB; }
    float IAbility._duration { get => _Duration; }
    float IAbility._tic { get => _Tic; }
    public override void Enchant()
    {
        Debug.Log("��Ʈ �� ����");
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
        Debug.Log("��Ʈ �� ����");
    }
}
