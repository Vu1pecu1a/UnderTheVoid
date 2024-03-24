using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandBag : MonoBehaviour,HitModel
{
    public void TakeDamege(DemageModel damageModel)
    {
        Debug.Log(damageModel.basedamage);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                transform.position = hit.point+Vector3.up*0.5f;
            }
        }
    }
}
