using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HitModel>() != null)
        {
            D_calcuate.i.Buff(other.GetComponent<MonsterBase>(), other.GetComponent<MonsterBase>());
            gameObject.DestroyAPS();
        }
    }
}
