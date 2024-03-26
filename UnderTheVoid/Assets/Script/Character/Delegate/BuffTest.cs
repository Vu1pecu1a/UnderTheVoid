using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HitModel>() != null)
        {
          // other.GetComponent<MonsterBase>().AddBuff(other.GetComponent<MonsterBase>(), other.GetComponent<MonsterBase>(),0,10,0.5f);
            other.GetComponent<MonsterBase>().AddDeBuff(other.GetComponent<MonsterBase>(), other.GetComponent<MonsterBase>(), 0, 10, 0.1f);
            gameObject.DestroyAPS();
        }
    }
}
