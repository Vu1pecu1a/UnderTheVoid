using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelecPos : MonoBehaviour
{
    [SerializeField]
    Vector3 objectHitPostion;
    Vector3 newPos;
    Vector3 distance;
    public Transform prevpos;//마지막위치
    RaycastHit hitRay, hitLayerMask;

    private void OnMouseUp()
    {
        distance = Vector3.zero;
        objectHitPostion = newPos;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.blue);

        int layerMask = 1 << LayerMask.NameToLayer("ROAD");
        if (Physics.Raycast(ray, out hitLayerMask, Mathf.Infinity, layerMask))
        {
            MapGrid alfa = hitLayerMask.collider.gameObject.GetComponent<MapGrid>();
            if(alfa.occupanction()==false)//점유 상태가 아닐때
            {
                gameObject.transform.position = hitLayerMask.collider.gameObject.transform.position;
                alfa.isoccupancy(true,this.gameObject.GetComponent<PlayerBase>());
                prevpos.GetComponent<MapGrid>().isoccupancy(false,null);
            }
            else
            {
                gameObject.transform.position = hitLayerMask.collider.gameObject.transform.position;
                alfa.player.transform.position = prevpos.position;// 이전 위치
                alfa.player.GetComponent<SelecPos>().prevpos = prevpos;
                prevpos.GetComponent<MapGrid>().isoccupancy(true, this.gameObject.GetComponent<PlayerBase>());
            }
        }
        else
        {
            gameObject.transform.position = prevpos.position;//올바른 위치가 아니면 이전 위치로
        }
    }
    private void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitRay))
        {
            objectHitPostion = hitRay.point;
        }
        int layerMask = 1 << LayerMask.NameToLayer("ROAD");
        if (Physics.Raycast(ray, out hitLayerMask, Mathf.Infinity, layerMask))
        {
            prevpos = hitLayerMask.collider.gameObject.transform;
        }
    }

    private void OnMouseDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);

        int layerMask = 1 << LayerMask.NameToLayer("ROAD");
        if (Physics.Raycast(ray, out hitLayerMask, Mathf.Infinity, layerMask))
        {
            hitLayerMask.collider.gameObject.GetComponent<MapGrid>().ChangeColor();
            float H = Camera.main.transform.position.y;
            float h = objectHitPostion.y;
            if (distance == Vector3.zero) distance = this.transform.position - hitLayerMask.point;

            newPos
                = (hitLayerMask.point * (H - h) + Camera.main.transform.position * h) / H;
            gameObject.transform.position = hitLayerMask.point + distance;
        }
    }

}
