using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandBag : MonoBehaviour,HitModel
{

    public void TakeDamege(DemageModel damageModel)
    {
        Debug.Log(damageModel.basedamage);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
