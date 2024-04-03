using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class MouseOverTest : MonoBehaviour ,IPointerEnterHandler
{
    private void OnMouseEnter()
    {
        Debug.Log("진입");
    }

    private void OnMouseExit()
    {
        Debug.Log("탈출");
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("진입");
    }
}
