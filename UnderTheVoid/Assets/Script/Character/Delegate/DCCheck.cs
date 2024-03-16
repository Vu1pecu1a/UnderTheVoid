using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCCheck : MonoBehaviour
{
    public MonsterBase onwer;
    [SerializeField]
    MonsterBase tartr;
    IEnumerator gotoPool(float time, GameObject alfa) //불러온 이펙트 삭제
    {
        yield return new WaitForSeconds(time);
        if (alfa.activeSelf != false)
            alfa.DestroyAPS();
    }
    private void OnEnable()
    {
        onwer = null;
        tartr = null;
        StartCoroutine(gotoPool(1f, gameObject));
    }

    public void TargetLockOn()
    {
        tartr = onwer.target;
        transform.LookAt(tartr.transform.position+Vector3.up);
        transform.Rotate(90, 0, 0);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
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
             DamageController.DealDamage(other.GetComponent<HitModel>(),
               D_calcuate.i.BowShot(onwer.ATK) , other.transform);
            gameObject.DestroyAPS();
        }
    }

}
