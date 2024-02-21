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
    [SerializeField]
    public MonsterBase target;
    [SerializeField]
    public float attackRange = 2.8f, attackSpeed = 3f;
    [SerializeField]
    protected int MaxHp ,hp, atk, def, agi;
    [SerializeField]
    protected AI_TYPE aI;
    [SerializeField]
    protected AI_State state;
    public AI_State State { get { return state; } set { state = value; } }
    public int HP { get { return hp; } set { hp = value; } }
    public int ATK { get { return atk; } set { atk = value; } }
}
