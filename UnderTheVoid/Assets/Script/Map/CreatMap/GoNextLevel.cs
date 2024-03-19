using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GoNextLevel : MonoBehaviour
{
    [SerializeField] int i;
    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("¿¡·¯");
            return;
        }
        ScenecManeger.i.GoScene(i);
    }
}
