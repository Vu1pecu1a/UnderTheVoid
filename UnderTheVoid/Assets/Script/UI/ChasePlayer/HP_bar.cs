using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_bar : MonoBehaviour
{
    Image hp;
    [SerializeField]
    MonsterBase mb;
    [SerializeField]
    Vector3 alfa = new Vector3();
    private void OnEnable()
    {
        if(mb==null)
            mb= transform.parent.GetComponent<MonsterBase>();
        // transform.parent = D_calcuate.i.UI_Canvas.transform;
    }

    private void Update()
    {
       transform.rotation = Quaternion.LookRotation(alfa);
      
    }
}
