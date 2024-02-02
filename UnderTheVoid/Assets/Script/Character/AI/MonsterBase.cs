using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class MonsterBase : FSM<MonsterBase> ,HitModel
{
    public DemageModel DM;

    NavMeshAgent agent { get; set; }
    public NavMeshAgent _agent;
    [SerializeField]
    public Transform target;
    [SerializeField]
    float attackRange = 0.8f;
    [SerializeField]
    int hp { get; set; }
    public int HP;
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
        ResetState();
        InitState(this, IDEL.Instance);
        DM = new DemageModel(1,DamageType.Slash);
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
    
    
    public void cal()
    {
        
    }

    public void _Attack()
    {
        StartCoroutine(AttackRange());
    }

    public void StopMove()
    {
        StopCoroutine(AttackRange());
    }

    bool isAttack()
    {
        if (Vector3.Distance(gameObject.transform.position, target.position) >= attackRange)
        {
            return true;
        }
        else return false;

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
    }

    public void Execute(MonsterBase e)
    {
        DamageController.DealDamage(e.target.GetComponent<HitModel>(), e.DM);
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
