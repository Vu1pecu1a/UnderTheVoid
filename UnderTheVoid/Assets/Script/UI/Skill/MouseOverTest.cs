using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class MouseOverTest : MonoBehaviour ,IPointerEnterHandler
{
    private void OnMouseEnter()
    {
        Debug.Log("����");
    }

    private void OnMouseExit()
    {
        Debug.Log("Ż��");
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("����");
    }
}
