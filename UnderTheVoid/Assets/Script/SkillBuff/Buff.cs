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
    [SerializeField, Tooltip("지속 시간")] float _Duration;
    [SerializeField, Tooltip("버프 효과 틱")] float _Tic;
    [SerializeField, Tooltip("타겟")] MonsterBase _MB;
    [SerializeField, Tooltip("시전자")] MonsterBase _PB;

    /// <summary>
    /// 버프 내용
    /// </summary>
    /// <param 시전자="pb"></param>
    /// <param 타겟 ="mb"></param>
    /// <param 지속 시간 ="dur"></param>
    /// <param 틱 ="tic"></param>
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
        Debug.Log("도트 힐 시작");
        D_calcuate.i.tic += Tic;
    }
    public void EnchantingEffect()
    {
        Debug.Log("도트 힐 하는중");
        D_calcuate.i.ab.DotHeal(_MB);
    }
    public void DisEnchant()
    {
        Debug.Log("도트 힐 종료");
        D_calcuate.i.tic -= Tic;
    }
}