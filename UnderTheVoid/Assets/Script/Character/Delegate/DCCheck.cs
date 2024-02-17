using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCCheck : MonoBehaviour
{
    public MonsterBase onwer;

    DemageModel DM;

    private void Awake()
    {
        DM = new DemageModel(1, DamageType.Slash);
        DM.damageType = DamageType.Freeze;
    }

    private void OnEnable()
    {
      
    }

    private void OnDisable()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {

        DM.basedamage = onwer.ATK;
        Debug.Log("Enter");
        if (other.GetComponent<HitModel>() == null)
        {
        }
        else
        {
            DamageController.DealDamage(other.GetComponent<HitModel>(), DM,other.transform);
        }
    }

}
