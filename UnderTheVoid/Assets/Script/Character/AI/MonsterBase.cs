using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class MonsterBase : FSM<MonsterBase> ,HitModel
{
    public delegate void MoveAction();
    public delegate void AttackAction();
    public event MoveAction MoveEvent;
    public event AttackAction AttackEvent;

    public DemageModel DM;

    NavMeshAgent agent { get; set; }
    public NavMeshAgent _agent;
    [SerializeField]
    public Transform target;
    [SerializeField]
    float attackRange = 2.8f,attackSpeed = 0.5f;
    [SerializeField]
    int hp { get; set; }
    public int HP;
    public int Pow = 1,Int = 3;
   

    public float AttackSpeed;
    private void Awake()
    {
       
    }
    // Start is called before the first frame update
    void Start()
    {
        if (agent == null)
        {
            agent = this.gameObject.GetComponent<NavMeshAgent>();
        }
        _agent = agent;
        hp = HP;
        AttackSpeed = attackSpeed;
        ResetState();
        InitState(this, IDEL.Instance);
        DM = new DemageModel(1,DamageType.Freeze);
        StartCoroutine(StartFSM());
    }

    private void ResetState()
    {
        
    }

    private void OnEnable()
    {
        D_calcuate.playerDie += playerDie;
    }
    void playerDie()
    {
        ChageState(IDEL.Instance);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
           ChageState(Move.Instance);
        }else if(Input.GetKeyDown(KeyCode.R))
        {
           ChageState(IDEL.Instance);
        }
    }

    IEnumerator StartFSM()
    {
        yield return null;
        FsmUpdate();
        StartCoroutine(StartFSM());
    }

    IEnumerator AttackRange()
    {
        yield return new WaitForSeconds(0.1f);

        if(isAttack()==true)
        StartCoroutine(AttackRange());
        else
        ChageState(Attack.Instance);
    }
    IEnumerator CountAttackSpeed()
    {
        yield return new WaitForSeconds(attackSpeed);
        ChageState(IDEL.Instance);
    }
    
    public void attackCoolTime()
    {
        StartCoroutine(CountAttackSpeed());
    }

    public void _Attack()
    {
        StartCoroutine(AttackRange());
    }

    public void StopMove()
    {
        MoveEvent();
        StopCoroutine(AttackRange());
    }

    bool isAttack()
    {
        if (Vector3.Distance(gameObject.transform.position, target.position) >= attackRange)
        {
            Debug.Log(Vector3.Distance(gameObject.transform.position, target.position));
            return true;
        }
        else return false;

    }

    public void AttackToTarget()
    {
        AttackEvent();
    }

    public void TakeDamege(DemageModel damageModel)
    {
        hp -= damageModel.basedamage;
    }
}
class IDEL : FSMSingleton<IDEL>, InterfaceFsmState<MonsterBase>
{

    public void Enter(MonsterBase e)
    {
        Debug.Log("기본상태 진입");
    }

    public void Execute(MonsterBase e)
    {

    }

    public void Exit(MonsterBase e)
    {
        Debug.Log("기본상태 탈출");
    }
}
 class Move :FSMSingleton<Move>, InterfaceFsmState<MonsterBase>
{
    
    public void Enter(MonsterBase e)
    {
        Debug.Log("이동상태 진입");
        e._Attack();
    }

    public void Execute(MonsterBase e)
    {
        e._agent.isStopped = false;
        e._agent.SetDestination(e.target.position);
    }

    public void Exit(MonsterBase e)
    {
        Debug.Log("이동상태 탈출");
        e._agent.isStopped= true;
    }
}

class Attack : FSMSingleton<Attack>, InterfaceFsmState<MonsterBase>
{

    public void Enter(MonsterBase e)
    {
        Debug.Log("공격상태 진입");
        DamageController.DealDamage(e.target.GetComponent<HitModel>(), e.DM,e.target);

        //TextRendererParticleSystem.i.SpawnParticle(e.target.position+Vector3.up, e.DM.basedamage.ToString(), Color.red);
        e.attackCoolTime();
        e.AttackToTarget();
    }

    public void Execute(MonsterBase e)
    {
       
    }

    public void Exit(MonsterBase e)
    {
        Debug.Log("공격상태 탈출");
    }
}

class Stun : FSMSingleton<Stun>, InterfaceFsmState<MonsterBase>
{
    public void Enter(MonsterBase e)
    {
        Debug.Log("기절상태 진입");
    }

    public void Execute(MonsterBase e)
    {

    }

    public void Exit(MonsterBase e)
    {
        Debug.Log("기절상태 탈출");
    }
}
