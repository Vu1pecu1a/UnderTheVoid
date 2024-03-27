using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Base : MonoBehaviour
{
    public Animator _animator;

    protected DemageModel DM = new(0, DamageType.Slash);

    protected NavMeshAgent agent { get; set; }
    public NavMeshAgent _agent;
    public NavMeshObstacle _objstacle;
    [SerializeField]
    public MonsterBase target;
    [SerializeField,Tooltip("초당 공격하는 횟수")]
    protected float attackRange = 2.8f, attackSpeed = 3f,moveSpeed = 3.5f;
    [SerializeField, Tooltip("버프 보정치")]
    public float buffattackRange = 0, buffattackSpeed = 0, buffmoveSpeed = 0;
    [SerializeField]
    protected int MaxHp ,hp, atk, def, agi;
    [SerializeField, Tooltip("버프 보정치")]
    public float buffMaxHp=0, buffhp=0, buffatk=0, buffdef=0, buffagi=0;
    [SerializeField]
    protected AI_TYPE aI;
    [SerializeField]
    protected AI_State state;

    /// <summary>
    /// AI의 현재 상태
    /// </summary>
    public AI_State State { get { return state; } set { state = value; } }
    public int HP { get { return hp; } set { hp = value; } }
    public int ATK { get { return atk + (int)(atk * buffatk); } set { atk = value; } }

    public int DEF { get { return def + (int)(def * buffdef); } set { def= value; } }
    public int Agi { get { return agi; } set { agi = value; } }

    public int MAXHP { get { return MaxHp+(int)(MaxHp * buffMaxHp); } set { MaxHp = value; } }

    public float ATKSpeed { get { return attackSpeed + (int)(attackSpeed * buffattackSpeed);} }
    public float ATKRANGE { get { return attackRange; } }
    public float MoveSpeed { get { return moveSpeed + (moveSpeed * buffmoveSpeed); }}

}
