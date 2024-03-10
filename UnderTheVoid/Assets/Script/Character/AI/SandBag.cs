using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandBag : MonoBehaviour,HitModel
{
    [SerializeField]
    private InvenShape _shape;
    [SerializeField]
    public bool[] israck; 
    public void TakeDamege(DemageModel damageModel)
    {
        Debug.Log(damageModel.basedamage);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Èú");
        if (other.GetComponent<HitModel>() != null)
        {//other.GetComponent<HitModel>().TakeDamege(D_calcuate.i.Heal(1));
            DamageController.DealDamage(other.GetComponent<HitModel>(), D_calcuate.i.Heal(1), other.transform);
        }
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
