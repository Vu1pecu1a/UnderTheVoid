using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class MonsterBase : FSM<MonsterBase> ,HitModel
{
    public delegate void MoveAction();
    public delegate void AttackAction();
    public delegate void DieAction();
    public virtual event MoveAction MoveEvent;
    public virtual event AttackAction AttackEvent;
    protected virtual event DieAction DieEvent;
    [SerializeField]
    Renderer _HpBar;
    private void Awake()
    {
       
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        ResetState();
        InitState(this, IDEL.Instance);
    }

    protected void ResetState()
    {
        if (agent == null)
        {
            agent = this.gameObject.GetComponent<NavMeshAgent>();
        }
        if (_animator == null)
        {
            _animator = gameObject.GetComponent<Animator>();
        }
        if(_objstacle== null)_objstacle = gameObject.GetComponent<NavMeshObstacle>();
        _agent = agent;
        _agent.stoppingDistance = attackRange;
        _objstacle.enabled= false;
        AttackEvent += Debug_log;
        DieEvent += Debug_Die;
        hp = MaxHp;
        _HpBar.gameObject.SetActive(true);
    }
    void Debug_log()
    {
    }

    public void TargetlockOn()//타겟확정 
    {
        target.GetComponent<MonsterBase>().DieEvent += TargetisNull;
    }

    public virtual void Search()
    {
        
    }

    void TargetisNull()
    {
        Debug.Log(" 상대 사망 ");
        target = null;
    }

    void Debug_Die()
    {
        if(state.Equals(AI_State.Die))
        {
            Debug.Log("DIE");
        }
    }

    private void OnEnable()
    {
        //D_calcuate.playerDie += playerDie;
    }
    void playerDie()
    {
        ChageState(IDEL.Instance);
    }

    private void Update()
    {
        FsmUpdate();
    }

   public void AttackRange()
    {
        
        if(target.gameObject.GetComponent<MonsterBase>().HP <= 0)
            target = null;

        if (target == null)
            ChageState(IDEL.Instance);

        if (isAttack() == true)
            return;
        else
        {
            switch (aI)
            {
                case AI_TYPE.Melee:
                    ChageState(Attack.Instance);
                    break;
                case AI_TYPE.Range:
                    ChageState(RangeAttack.Instance);
                    break;
                case AI_TYPE.Heal: 
                    ChageState(Attack.Instance);
                    break;
                default:
                    ChageState(Attack.Instance);
                    break;
            }
        }
    }
    IEnumerator CountAttackSpeed()
    {
        if (target == null || target.State == AI_State.Die)
            ChageState(IDEL.Instance);
        yield return new WaitForSeconds(1/attackSpeed);

        if (State == AI_State.Attack)
        {
            if (aI == AI_TYPE.Melee)
            ChageState(Attack.Instance);
            else if(aI == AI_TYPE.Range)
            ChageState(RangeAttack.Instance);
        }
    }
    
    public void attackCoolTime()
    {
        StartCoroutine(CountAttackSpeed());
    }

    public void _Attack()
    {
    }

    public void _targetDown()//적이 사망했는지 체크
    {
        if (target == null)
            return;

        transform.LookAt(target.transform);
        if (target.gameObject.GetComponent<MonsterBase>().HP <= 0)
        {
            target = null;
            ChageState(IDEL.Instance);
        }
            

        if (target == null)
            ChageState(IDEL.Instance);
    }

    public void StopMove()//이동이 종료되면 호출되는 이벤트
    {
        MoveEvent();
    }

    bool isAttack()//사거리 체크용
    {
        if (target == null)
            return false;
        if (Vector3.Distance(gameObject.transform.position, target.transform.position) >= attackRange)
        {
            return true;
        }
        else return false;
    }

    public void AttackToTarget()//공격 이벤트용 
    {
        this.AttackEvent();
    }

    IEnumerator gotoPool(float time,GameObject alfa) //불러온 이펙트 삭제
    {
        yield return new WaitForSeconds(time);
        alfa.DestroyAPS();
    }

    public void BowAttack()
    {
        this.AttackEvent();
        _animator.SetFloat("AttackSpeed", attackSpeed);
        DM.basedamage = D_calcuate.i.bowshot(atk);
        DM.damageType = DamageType.Stab;
        if (target == null)
            return;
        GameObject effecti = ObjPoolManager.i.InstantiateAPS("bowShot", null);
        effecti.GetComponent<LineRenderer>().SetPosition(0, transform.position+Vector3.up);
        effecti.GetComponent<LineRenderer>().SetPosition(1, target.transform.position + Vector3.up);
        DamageController.DealDamage(target.GetComponent<HitModel>(), DM, target.transform);
        StartCoroutine(gotoPool(0.1f,effecti));
    }

    public void MeleeAttack()
    {
        this.AttackEvent();
        DM.basedamage = atk;
        DM.damageType = DamageType.Bash;
        if (target == null)
            return;
        DamageController.DealDamage(target.GetComponent<HitModel>(), DM, target.transform);
    }

    public void DIe()//사망이벤트
    {
        State = AI_State.Die;
        ChageState(Die.Instance);
        DieEvent();
    }

    public void UIOFF()
    {
        _HpBar.gameObject.SetActive(false);
    }

    public virtual void TakeDamege(DemageModel damageModel) // 대미지 계산 파츠
    {
        if (state == AI_State.Die)
            return;

        if (hp > 0)
            hp -= damageModel.basedamage;

        _HpBar.material.SetFloat("_FillAmount", (float)hp / MaxHp);
       // Debug.Log((float)hp / MaxHp);

        if (hp <= 0)
            DIe();
    }
}

class IDEL : FSMSingleton<IDEL>, InterfaceFsmState<MonsterBase>
{

    public void Enter(MonsterBase e)
    {
        e.State = AI_State.Idel;
        Debug.Log("기본상태 진입");
    }

    public void Execute(MonsterBase e)
    {
        if (e.target == null)
        {
           e.Search();
        }else
        {
            e.ChageState(Move.Instance);
        }
    }

    public void Exit(MonsterBase e)
    {
        if(e.target!=null)
        e.TargetlockOn();
        Debug.Log("기본상태 탈출");
    }
}
 class Move :FSMSingleton<Move>, InterfaceFsmState<MonsterBase>
{
    
    public void Enter(MonsterBase e)
    {
        e._objstacle.enabled = false;
        e._agent.enabled = true;
        e.State = AI_State.Walk;
        e._animator.SetBool("Walk", true);
    }

    public void Execute(MonsterBase e)
    {
        if(e.target == null)
        {
            e.ChageState(IDEL.Instance);
            return;
        }
        if(e._agent!=null)
        e._agent.SetDestination(e.target.transform.position);
        e.AttackRange();
    }

    public void Exit(MonsterBase e)
    {
        e._animator.SetBool("Walk", false);
        e._agent.enabled= false;
        e._objstacle.enabled = true;
    }
}

class Attack : FSMSingleton<Attack>, InterfaceFsmState<MonsterBase>
{

    public void Enter(MonsterBase e)
    {
        e.State = AI_State.Attack;
        e._animator.SetTrigger("Attack");
        e._animator.SetFloat("AttackSpeed", e.attackSpeed);
        e.attackCoolTime();
    }

    public void Execute(MonsterBase e)
    {
        e._targetDown();
    }

    public void Exit(MonsterBase e)
    {
        if(e.State == AI_State.Attack)
        e.MeleeAttack();
    }
}
class RangeAttack : FSMSingleton<RangeAttack>, InterfaceFsmState<MonsterBase>
{

    public void Enter(MonsterBase e)
    {
        e.State = AI_State.Attack;
        e._animator.SetTrigger("Shot");
        e.attackCoolTime();
    }

    public void Execute(MonsterBase e)
    {
        e._targetDown();
    }

    public void Exit(MonsterBase e)
    {
        if (e.State == AI_State.Attack)
            e.BowAttack();
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
class Die : FSMSingleton<Die>, InterfaceFsmState<MonsterBase>
{
    public void Enter(MonsterBase e)
    {
        Debug.Log("사망상태 진입");
        e.State = AI_State.Die;
        e._animator.SetTrigger("Die");
        e._agent.enabled = false;
        e._objstacle.enabled = false;
        e.StopAllCoroutines();
        e.UIOFF();
    }

    public void Execute(MonsterBase e)
    {

    }

    public void Exit(MonsterBase e)
    {
        e._agent.isStopped = false;
        Debug.Log("사망상태 탈출");
    }
}
