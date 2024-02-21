using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class STATETMP : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshPro text;
    [SerializeField]
    MonsterBase mb;
    private void Update()
    {
        text.text = mb.State.ToString(); 
    }

    private void LateUpdate()
    {
        Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
        transform.LookAt(transform.position + dirFromCamera);
       // transform.position = mb.transform.position + Vector3.up * 3;
    }
}
