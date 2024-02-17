using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DamageController
{
    public static void DealDamage(HitModel damageable, DemageModel damageModel,Transform targetPosition)
    {
        damageable.TakeDamege(damageModel);
        TextRendererParticleSystem.i.SpawnParticle(targetPosition.position+Vector3.up*2,damageModel.basedamage.ToString(),damageModel.damageType);
    }
}
public class D_calcuate : MonoBehaviour
{
    [SerializeField]
    GameObject iceball;
    [SerializeField]
    MonsterBase mb;

    public static D_calcuate i;
    delegate void CallintD(int d);
    public delegate int PlayerHit(MonsterBase A,int B);
    public delegate void PlayerDie();
    [SerializeField]
    List<EnemyBase> list = new List<EnemyBase>();
    [SerializeField]
    List<MonsterBase> _playerList = new List<MonsterBase>();
    public List<EnemyBase> MonsterList { get { return list; } set { list = value; } }
    public List<MonsterBase> PlayerList { get { return _playerList; } set { _playerList = value; } }

    // public Dictionary<string, DemageModel> D_C = new Dictionary<string, DemageModel>();



    // public static event PlayerHit playerHit;
    public static event PlayerDie playerDie;


    public static EventHandler hit;
    delegate int intop(int x,int y);
    
    intop op;


    void ReturnD(MonsterBase target,int D)
    {
        target.HP -= D;
    }

    // Start is called before the first frame update
    void Start()
    {
        i = this;
        // mb.AttackEvent += IceBallInstance;
    }

    // Update is called once per frame
     
    void IceBallInstance()
    {
        GameObject a = Instantiate(iceball,mb.target.transform.position,Quaternion.identity);
        a.GetComponent<DCCheck>().onwer = mb;
    }

    public int bowshot(int a)
    {
        int b = a * 2;
        return b;
    }
    static void Swap(ref int a, ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }
}
