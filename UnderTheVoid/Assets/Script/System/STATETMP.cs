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
}
