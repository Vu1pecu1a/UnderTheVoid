using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    void Start()
    {
        
    }
}
public class DotHeal : IAbility
{
    [SerializeField, Tooltip("���� �ð�")] float _Duration;
    [SerializeField, Tooltip("���� ȿ�� ƽ")] float _Tic;
    [SerializeField, Tooltip("Ÿ��")] MonsterBase _MB;
    [SerializeField, Tooltip("������")] MonsterBase _PB;

    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param ������="pb"></param>
    /// <param Ÿ�� ="mb"></param>
    /// <param ���� �ð� ="dur"></param>
    /// <param ƽ ="tic"></param>
    public DotHeal(MonsterBase pb, MonsterBase mb,float dur = 10,float tic = 0.5f)
    {
        _Duration = dur;
        _Tic = tic;
        _MB = mb;
        _PB = pb;
    }


    MonsterBase IAbility._MB { get => _MB; }
    float IAbility._duration { get => _Duration; }
    float IAbility._tic { get => _Tic;}

    public void Tic()
    {
        _Duration -= Time.deltaTime;
    }

    public void Enchant()
    {
        Debug.Log("��Ʈ �� ����");
        D_calcuate.i.tic += Tic;
    }
    public void EnchantingEffect()
    {
        Debug.Log("��Ʈ �� �ϴ���");
        D_calcuate.i.ab.DotHeal(_MB);
    }
    public void DisEnchant()
    {
        Debug.Log("��Ʈ �� ����");
        D_calcuate.i.tic -= Tic;
    }
}