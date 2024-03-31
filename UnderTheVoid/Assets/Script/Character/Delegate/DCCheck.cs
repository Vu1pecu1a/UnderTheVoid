using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterBase;

public class DCCheck : MonoBehaviour
{
    public MonsterBase onwer;
    [SerializeField]
    MonsterBase tartr;
    public DemageModel DM;

    public bool Penetrate = false;

    public delegate void GetHit<T>(T t);
    public virtual event GetHit<GameObject> Hit;

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
        Hit += DebugDemage;
    }


    void DebugDemage(GameObject gm)
    {
        //Debug.Log(DM.basedamage);
    }

    public void TargetLockOn()
    {
        tartr = onwer.target;
        if (onwer.target == null) return;
        transform.LookAt(tartr.transform.position+Vector3.up);
        transform.Rotate(90, 0, 0);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        Hit -= DebugDemage;
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
            DamageController.DealDamage(other.GetComponent<HitModel>(),DM , other.transform);
            Hit(other.gameObject);
            if(!Penetrate)gameObject.DestroyAPS();
        }
    }

}
