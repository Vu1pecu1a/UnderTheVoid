using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBase : MonsterBase
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    private void OnEnable()
    {
        ResetState();
        D_calcuate.i.MonsterList.Add(this);
        this.DieEvent += RemoveListthis;
        this.agent.enabled = true;
    }
    public override void Search()
    {
        if (D_calcuate.i.PlayerList.Count == 0)
            return;
        target = D_calcuate.i.PlayerList.First();
    }

    void RemoveListthis()
    {
        D_calcuate.i.MonsterList.Remove(this);
        this.agent.enabled= false;
    }

    private void OnDisable()
    {
        if(D_calcuate.i.MonsterList.Contains(this))
        D_calcuate.i.MonsterList.Remove(this);
    }
}
