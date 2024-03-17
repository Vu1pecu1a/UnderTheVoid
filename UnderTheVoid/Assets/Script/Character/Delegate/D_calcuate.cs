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
        if(damageModel.basedamage < 0)
        TextRendererParticleSystem.i.SpawnParticle(targetPosition.position+Vector3.up*2,(damageModel.basedamage*-1).ToString(),damageModel.damageType);
        else
            TextRendererParticleSystem.i.SpawnParticle(targetPosition.position + Vector3.up * 2, damageModel.basedamage.ToString(), damageModel.damageType);


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

    public static bool isbattel=false;


    public delegate void RoomClear();
    public RoomClear roomClear;
    delegate void CallintD(int d);
    public delegate int PlayerHit(MonsterBase A,int B);
    public delegate void PlayerDie();




    [SerializeField]
    List<EnemyBase> list = new List<EnemyBase>();
    [SerializeField]
    List<PlayerBase> _playerList = new List<PlayerBase>();
    public List<EnemyBase> MonsterList { get { return list; } set { list = value; } }
    public List<PlayerBase> PlayerList { get { return _playerList; } set { _playerList = value; } }

    public Dictionary<string, DemageModel> D_C = new Dictionary<string, DemageModel>(); // 대미지 계산기

    public DemageModel Killer = new(9999, DamageType.Slash);


    public DemageModel Heal(int i)
    {
        DemageModel Heal = new(-i, DamageType.Heal);
        return Heal;
    }

    public DemageModel BowShot(int a)
    {
       DemageModel Bowshot = new(a*2, DamageType.Stab);
        return Bowshot;
    }

    public void BattleStart()
    {
        if (PlayerReady())
            return;
        Debug.Log("전투 시작");
        isbattel = true;
        MapGenerator.i.Minimap(0);
        SelectGrid.i.gameObject.SetActive(false);
    }

    public void BattelStart()
    {
        SelectGrid.i.gameObject.SetActive(true);
        StartCoroutine(BattelEnd()); 
    }
    IEnumerator BattelEnd()
    {
       yield return new WaitForSeconds(1);//1초마다 한번씩 검사
        if(MonsterList.Count == 0)
        {
            MapGenerator.i.RoomClearTrue();
            isbattel = false;
            MapGenerator.i.Minimap(true);
            playerTargetnull();
        }
        else
            StartCoroutine(BattelEnd());
    }

    void playerTargetnull()
    {
        foreach(PlayerBase p in PlayerList)
        {
            p.target = null;
        }
    }//전투 종료 이후로 호출할 함수

    private void Awake()
    {
        i = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }
    
    public void PlayerSpawn()
    {
        if (ScenecManeger.i != null)
        foreach (GameObject p in ScenecManeger.i.ChoseCharicterList)
        {
            Instantiate(p);
        }
        Managers.instance._C.playerSpawn();
    }
    /// <summary>
    /// 모든 플레이어가 무대 위로 올라왔는지 확인하는 종류의 함수
    /// </summary>
    /// <returns></returns>
    bool PlayerReady()
    {
        int p = 0;
        foreach (MonsterBase a in PlayerList)
        {
            if (a.GetComponent<SelecPos>().prevpos == null)
                p++;
        }
        return p == PlayerList.Count;
    }

    public void PlayerPositionReset()
    {
        foreach(MonsterBase a in  PlayerList )
        {
            a.gameObject.transform.position = a.GetComponent<SelecPos>().prevpos.position;
        }
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
