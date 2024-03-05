using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCCheck : MonoBehaviour
{
    public MonsterBase onwer;
    [SerializeField]
    MonsterBase tartr;

    private void OnEnable()
    {
        onwer = null;
        tartr = null;
    }

    public void TargetLockOn()
    {
        tartr = onwer.target;
        transform.LookAt(tartr.transform.position+Vector3.up);
        transform.Rotate(90, 0, 0);
    }

    private void OnDisable()
    {
    //    Rigidbody rb = this.GetComponent<Rigidbody>();
    //    rb.velocity= Vector3.zero;
    //    rb.angularVelocity= Vector3.zero;
        tartr = null;
    }

    private void Update()
    {
        if (tartr != null)
        {
            transform.Translate(Vector3.up * Time.deltaTime * 20);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HitModel>() != null && other.tag != onwer.tag)
        {
            Debug.Log("Enter");
            DamageController.DealDamage(other.GetComponent<HitModel>(),
               D_calcuate.i.BowShot(onwer.ATK) , other.transform);
            gameObject.DestroyAPS();
        }
    }

}
