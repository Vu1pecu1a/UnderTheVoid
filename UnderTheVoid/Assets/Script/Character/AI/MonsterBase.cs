using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class MonsterBase : FSM<MonsterBase> ,HitModel
{

    #region[UI]
    [SerializeField]
    Renderer _HpBar;
    public void UIOFF()
    {
        _HpBar.gameObject.SetActive(false);
    }

    public void SetHpBar()
    {
        _HpBar.material.SetFloat("_FillAmount", (float)hp / MaxHp);
    }
    public float hpbarreturn()
    {
        return ((float)hp / MaxHp);
    }

    #endregion[UI]
    #region[이벤트]
    public delegate void MoveAction();
    public delegate void AttackAction();
    public delegate void SpellAction();
    public delegate void DieAction();
    public virtual event MoveAction MoveEvent;
    public virtual event AttackAction AttackEvent;
    public virtual event SpellAction SpellEvent;
    protected virtual event DieAction DieEvent;


    public void AttackToTarget()//공격 이벤트용 
    {
        this.AttackEvent();
    }
    public void StopMove()//이동이 종료되면 호출되는 이벤트
    {
        MoveEvent();
    }
    public void DIe()//사망이벤트
    {
        DieEvent();
        State = AI_State.Die;
        ChageState(Die.Instance);
    }
    public void TargetlockOn()//타겟확정 
    {
        target.GetComponent<MonsterBase>().DieEvent += TargetisNull;
    }
    #endregion[이벤트]
    #region[버프/디버프]
    public List<Buff> _Buff = new List<Buff>();
    public List<Buff> _DeBuff = new List<Buff>();
    public void AddBuff(MonsterBase MB, MonsterBase PB,BuffType type,float _dur,float _tic)
    {
        Buff buff = (Buff)D_calcuate.i.bufflist[type].Clone();

        buff.SetData(PB, MB, _dur, _tic);
        StartCoroutine(buff.Tic());
        StartCoroutine(buff.TicofEffect());
        buff._BuffEvent += RemoveBuff;

        DieEvent += buff.EndofDuration;
        D_calcuate.i.roomClear += buff.EndofDuration;
        _Buff.Add(buff);
    }
    public void AddDeBuff(MonsterBase MB, MonsterBase PB, BuffType type, float _dur, float _tic)
    {
        Buff buff = (Buff)D_calcuate.i.Debufflist[type].Clone();

        buff.SetData(PB, MB, _dur, _tic);
        StartCoroutine(buff.Tic());
        StartCoroutine(buff.TicofEffect());
        buff._BuffEvent += RemoveDeBuff;

        DieEvent += buff.EndofDuration;
        D_calcuate.i.roomClear += buff.EndofDuration;
        _DeBuff.Add(buff);
    }
    public void RemoveDeBuff(Buff buff)
    {
        buff._BuffEvent -= RemoveDeBuff;
        DieEvent -= buff.EndofDuration;
        D_calcuate.i.roomClear -= buff.EndofDuration;

        Debug.Log("디버프 종료");
        if (_DeBuff.Contains(buff))
            _DeBuff.Remove(buff);
    }

    public void RemoveBuff(Buff buff)
    {
        buff._BuffEvent -= RemoveBuff;
        DieEvent -= buff.EndofDuration;
        D_calcuate.i.roomClear -= buff.EndofDuration;

        Debug.Log("버프 종료");
        if (_Buff.Contains(buff))
            _Buff.Remove(buff);
    }
    #endregion[버프/디버프]
    #region[유니티 함수]
    protected virtual void Update()
    {
        FsmUpdate();
    }
    private void Awake()
    {

    }
    private void OnEnable()
    {
        //D_calcuate.playerDie += playerDie;
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        ResetState();
        InitState(this, IDEL.Instance);
    }
    #endregion[유니티 함수]
    #region[체크 함수]

    void TargetisNull()
    {
        Debug.Log(" 상대 사망 ");
        target = null;
    }

    public IEnumerator CheckRange()
    {
        yield return new WaitForSeconds(1f);
        Search();
        StartCoroutine(CheckRange());
    }


    public void AttackRange()//사거리 체크
    {

        if (target.gameObject.GetComponent<MonsterBase>().HP <= 0)
            target = null;

        if (target == null)
        {
            ChageState(IDEL.Instance);
            return;
        }

        if (isAttack() == true)
        {
            _agent.SetDestination(target.transform.position);
            return;
        }
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
                    ChageState(HealCast.Instance);
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
        yield return new WaitForSeconds(1 / ATKSpeed);

        if (D_calcuate.isbattel == false || State == AI_State.OnSkill)
            ChageState(IDEL.Instance);

        if (State == AI_State.Attack)
        {
            if (aI == AI_TYPE.Melee)
                ChageState(Attack.Instance);
            else if (aI == AI_TYPE.Range)
                ChageState(RangeAttack.Instance);
        }
        else if (State == AI_State.SpellCast)
        {
            if (aI == AI_TYPE.Heal)
                ChageState(HealCast.Instance);
        }
    }

    public void attackCoolTime()
    {
        StartCoroutine(CountAttackSpeed());
    }

    public void SkillCoolTime(ActiveSkill skill)
    {
        StartCoroutine(CoolTime(skill));
    }
    public IEnumerator CoolTime(ActiveSkill skill)
    {
        skill.isReady = false;
        float i = 0;
        while(skill.CoolTIme > i )
        {
            i += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        skill.isReady = true;
    }

    public void _Attack()
    {
    }

    public void _targetDown()//적이 사망했는지 체크
    {
        if (target == null)
            return;
        Debug.DrawLine(gameObject.transform.position, target.gameObject.transform.position, Color.blue);

        transform.LookAt(target.transform);

        if (target.gameObject.GetComponent<MonsterBase>().HP <= 0)
        {
            target = null;
            ChageState(IDEL.Instance);
            return;
        }

        if (ATKRANGE < Vector3.Distance(transform.position, target.transform.position) && target != null)
        {
            State = AI_State.Walk;
            ChageState(Move.Instance);
        }

        if (target == null)
            ChageState(IDEL.Instance);
    }

    bool isAttack()//사거리 체크용
    {
        //if (target == null)
        //    return false;

        if (Vector3.Distance(gameObject.transform.position, target.transform.position) > ATKRANGE)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public virtual void Search()
    {

    }
    #endregion[체크 함수]
    #region[동작 기반 함수]
    IEnumerator gotoPool(float time, GameObject alfa) //불러온 이펙트 삭제
    {
        yield return new WaitForSeconds(time);
        if (alfa.activeSelf != false)
            alfa.DestroyAPS();
    }
    public void effectSetLine(GameObject effecti)
    {
        effecti.GetComponent<LineRenderer>().SetPosition(0, transform.position + Vector3.up);
        effecti.GetComponent<LineRenderer>().SetPosition(1, target.transform.position + Vector3.up);
        StartCoroutine(gotoPool(0.1f, effecti));
    }//라인 랜더러 불러오기

    public void Throwprojectile(GameObject effecti,DemageModel DM)
    {
        effecti.transform.position = this.gameObject.transform.position + Vector3.up;
        effecti.GetComponent<DCCheck>().onwer = this;
        effecti.GetComponent<DCCheck>().TargetLockOn();
        effecti.GetComponent<DCCheck>().DM = DM;
    }//투사체 발사
    public void effectSet(GameObject effecti)
    {
        if (target != null)
            effecti.transform.position = target.transform.position + Vector3.up;
        StartCoroutine(gotoPool(0.5f, effecti));
    }//타격 이팩트 삭제 코루틴
    public void effectSet(GameObject effecti,GameObject target,DemageModel DM)
    {
        if (target != null)
            effecti.transform.position = target.transform.position + Vector3.up;
        effecti.GetComponent<DCCheck>().onwer = this;
        effecti.GetComponent<DCCheck>().DM = DM;
        StartCoroutine(gotoPool(1f, effecti));
    }
    public virtual void TakeDamege(DemageModel damageModel) // 대미지 계산 파츠
    {
        if (state == AI_State.Die)
            return;

        if (hp > 0)
            hp -= damageModel.basedamage;

        if (hp >= MaxHp)//힐을 받아서 오버할 경우
            hp = MaxHp;
        SetHpBar();

        if (hp <= 0)
            DIe();
    }
    #endregion[동작 기반 함수]
    #region[공격 타입 함수]
    public void BowShot()
    {
        this.AttackEvent();
        _animator.SetFloat("AttackSpeed", ATKSpeed);
        if (target == null)
            return;
        GameObject effecti = ObjPoolManager.i.InstantiateAPS("Arrow_01", null);
        Throwprojectile(effecti,D_calcuate.i.BowShot(ATK));
    }

    public void BowAttack()
    {
        this.AttackEvent();
        _animator.SetFloat("AttackSpeed", ATKSpeed);
        if (target == null)
            return;
        GameObject effecti = ObjPoolManager.i.InstantiateAPS("bowShot", null);
        GameObject effectj = ObjPoolManager.i.InstantiateAPS("CFXR3 Hit Misc A", null);
        effectSetLine(effecti);
        effectSet(effectj);
        DamageController.DealDamage(target.GetComponent<HitModel>(), D_calcuate.i.BowShot(ATK), target.transform);
    }//라인렌더러 기준

    public void MeleeAttack()
    {
        this.AttackEvent();
        DM.basedamage = atk;
        DM.damageType = DamageType.Bash;
        if (target == null)
            return;
        GameObject effecti = ObjPoolManager.i.InstantiateAPS("CFXR3 Hit Misc A", null);
        effectSet(effecti);
        DamageController.DealDamage(target.GetComponent<HitModel>(), DM, target.transform);
    }

    public void Heal()
    {
        if (target == null)
            return;
        this.SpellEvent();
        GameObject effecti = ObjPoolManager.i.InstantiateAPS("bowShot", null);
        effectSetLine(effecti);
        effecti.GetComponent<LineRenderer>().material.color = Color.green;
        DamageController.DealDamage(target.GetComponent<HitModel>(), D_calcuate.i.Heal(atk * 20000), target.transform);
    }
    #endregion[공격 타입 함수]
    protected void ResetState()
    {
        if (agent == null)
        {
            agent = this.gameObject.GetComponent<NavMeshAgent>();
        }
        if (_animator == null)
        {
            _animator = transform.GetChild(1).GetComponent<Animator>();
        }
        if(_objstacle== null)_objstacle = gameObject.GetComponent<NavMeshObstacle>();
        _agent = agent;
        _agent.stoppingDistance = ATKRANGE - 0.5f;
        _objstacle.enabled= false;
        AttackEvent += Debug_log;
        DieEvent += Debug_Die;
        SpellEvent += Debug_log;
        hp = MaxHp;
        _HpBar.gameObject.SetActive(true);
        gameObject.GetComponent<Collider>().enabled = true;
    }//생성된 후 스탯 리셋

    void playerDie()
    {
        ChageState(IDEL.Instance);
    }
    void Debug_log()
    {
    }

    void Debug_Die()
    {
        if(state.Equals(AI_State.Die))
        {
            Debug.Log("DIE");
        }
    }
}

#region[FSM]
class IDEL : FSMSingleton<IDEL>, InterfaceFsmState<MonsterBase>
{

    public void Enter(MonsterBase e)
    {
        e.State = AI_State.Idel;
    }

    public void Execute(MonsterBase e)
    {
        if (D_calcuate.isbattel == false)
            return;

        if (e.target == null)
        {
            e.Search();
        }
        else
        {
            e.ChageState(Move.Instance);
        }
    }

    public void Exit(MonsterBase e)
    {
        if (e.target != null)
            e.TargetlockOn();
    }
}
class Move : FSMSingleton<Move>, InterfaceFsmState<MonsterBase>
{

    public void Enter(MonsterBase e)
    {
        e._objstacle.enabled = false;
        e._agent.enabled = true;
        e._agent.speed = e.MoveSpeed;
        e.State = AI_State.Walk;
        e._animator.SetBool("Walk", true);
        e.StartCoroutine(e.CheckRange());
    }

    public void Execute(MonsterBase e)
    {
        if (e.target != null)
        {
            e.AttackRange();
        }
        else
        {
            e.ChageState(IDEL.Instance);
            return;
        }
    }

    public void Exit(MonsterBase e)
    {
        e.StopCoroutine(e.CheckRange());
        e._animator.SetBool("Walk", false);
        e._agent.enabled = false;
        e._objstacle.enabled = true;
    }
}
class OnSkill : FSMSingleton<OnSkill>, InterfaceFsmState<MonsterBase>
{
    public void Enter(MonsterBase e)
    {
        e.State = AI_State.OnSkill;
        e._animator.SetTrigger("Cast");
        e.attackCoolTime();
    }

    public void Execute(MonsterBase e)
    {
        e._targetDown();
    }

    public void Exit(MonsterBase e)
    {
        if(e.State == AI_State.OnSkill && e is PlayerBase)
        {
            
        }
    }
}


class Attack : FSMSingleton<Attack>, InterfaceFsmState<MonsterBase>
{

    public void Enter(MonsterBase e)
    {
        e.State = AI_State.Attack;
        e._animator.SetTrigger("Attack");
        e._animator.SetFloat("AttackSpeed", e.ATKSpeed);
        e.attackCoolTime();
    }

    public void Execute(MonsterBase e)
    {
        e._targetDown();
    }

    public void Exit(MonsterBase e)
    {
        if (e.State == AI_State.Attack)
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
            e.BowShot();
    }
}
class HealCast : FSMSingleton<HealCast>, InterfaceFsmState<MonsterBase>
{

    public void Enter(MonsterBase e)
    {
        e.State = AI_State.SpellCast;
        e._animator.SetTrigger("Cast");
        e.attackCoolTime();
    }

    public void Execute(MonsterBase e)
    {
        e._targetDown();
    }

    public void Exit(MonsterBase e)
    {
        if (e.State == AI_State.SpellCast)
        {
            e._animator.SetTrigger("CastEnd");
            e.Heal();
        }
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
        e.State = AI_State.Die;
        e._animator.SetTrigger("Die");
        e._agent.enabled = false;
        e._objstacle.enabled = false;
        e.GetComponent<Collider>().enabled = false;
        // e.StopAllCoroutines();
        e.UIOFF();
    }

    public void Execute(MonsterBase e)
    {

    }

    public void Exit(MonsterBase e)
    {
        //e._agent.isStopped = false;
        Debug.Log(e.name);
        Debug.Log("사망상태 탈출");
    }
}

#endregion[FSM]