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

    public GameObject UI_Canvas;
    public static D_calcuate i;
    public delegate void RoomClear();
    public RoomClear roomClear;
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

    public DemageModel Killer = new(9999, DamageType.Slash);

    // public static event PlayerHit playerHit;
    //public static event PlayerDie playerDie;


    public static EventHandler hit;
    delegate int intop(int x,int y);
    
    intop op;


    void ReturnD(MonsterBase target,int D)
    {
        target.HP -= D; 
    }

    public void BattelStart()
    {
        Debug.Log("전투 시작");
        StartCoroutine(BattelEnd()); 
    }
    IEnumerator BattelEnd()
    {
       yield return new WaitForSeconds(1);//1초마다 한번씩 검사
        if(MonsterList.Count == 0)
       MapGenerator.i.RoomClearTrue();
        else
            StartCoroutine(BattelEnd());
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
}
