using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerofData
{
    public PlayerBase _pb { get; private set; }
    public InvenRender _InvenRender { get; private set; }
    public GameObject _Equipment { get; private set; }
    public GameObject _portrait { get; private set; }

    //스킬 하고 무기 방어구 구현하기

    public PlayerofData(PlayerBase playerBase,InvenRender invenRender, GameObject equipment, GameObject portrait)
    {
        _pb = playerBase;
        _InvenRender = invenRender;
        _Equipment = equipment;
        _portrait = portrait;
    }
}

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
    public GameObject UI_Canvas;
    public static D_calcuate i;
    public AbilityEffect ab = new AbilityEffect();

    public static bool isbattel=false;

    public delegate void RoomClear();
    public RoomClear roomClear;

    public delegate void Tic();
    public Tic tic;

    delegate void CallintD(int d);
    public delegate int PlayerHit(MonsterBase A,int B);
    public delegate void PlayerDie();

    [SerializeField]
    List<EnemyBase> list = new List<EnemyBase>();
    [SerializeField]
    List<PlayerBase> _playerList = new List<PlayerBase>();
    public List<EnemyBase> MonsterList { get { return list; } set { list = value; } }
    public List<PlayerBase> PlayerList { get { return _playerList; } set { _playerList = value; } }

    public Dictionary<InvenRender, PlayerofData> PlayerData = new Dictionary<InvenRender, PlayerofData>();//플레이어 데이터 참조용

    public Dictionary<string, DemageModel> D_C = new Dictionary<string, DemageModel>(); // 대미지 계산기

    public DemageModel Killer = new(9999, DamageType.Slash);

    public Dictionary<string, IAbility> bufflist = new Dictionary<string, IAbility>();

    public void Buff(MonsterBase MB,MonsterBase PB)
    {
        // 버프 지정
        IAbility buff = new DotHeal(PB,MB, 10, 0.5f);
        // 버프 시작
        buff.Enchant();
        // 버프 종료
        StartCoroutine(buffCor(buff));
    }

    IEnumerator buffCor(IAbility ab)
    {
        yield return new WaitForSeconds(ab._tic);
        ab.EnchantingEffect();

        if (ab._duration <= 0)
            ab.DisEnchant();
        else
            StartCoroutine(buffCor(ab));
    }

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
        if (!PlayerReady())
        {
            isbattel = false;
            return;
        }
        Debug.Log("전투 시작");
        isbattel = true;
        MapGenerator.i.Minimap(0);
        SelectGrid.i.gameObject.SetActive(false);
    }

    public void BattelStart()
    {
        isbattel = false;
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
        else if(PlayerList.Count==0)
        {
            Time.timeScale = 0;
            Managers.instance._UI.UIManager_GameOver();
        }else
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
       // bufflist.Add("DotHeal", new DotHeal());
        tic += Debugaaaa;
    }

    void Debugaaaa()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        tic();
    }

    public void PlayerSpawn()
    {
        //if (ScenecManeger.i != null)
        foreach (GameObject p in ScenecManeger.i.ChoseCharicterList)
        {
            Instantiate(p);
        }
        
        Managers.instance._C.playerSpawn();

        for(int i=0; i < PlayerList.Count; i++)
        {
            if(PlayerList[i].GetComponent<SelecPos>().prevpos == null)
            PlayerList[i].GetComponent<SelecPos>().prevpos = SelectGrid.i._grids[i,i].transform;//최초 위치 체크
            PlayerList[i].gameObject.transform.position = PlayerList[i].GetComponent<SelecPos>().prevpos.position;
            SelectGrid.i.Check();
        }//최초 위치 배정
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
            if (a.GetComponent<SelecPos>().prevpos != null)//현재 위치가 있으면 
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

    public int bowshot(int a)
    {
        int b = a * 2;
        return b;
    }
}
