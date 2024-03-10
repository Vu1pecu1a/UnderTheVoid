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

    [SerializeField]
    SelectGrid _Sg;

    private void Awake()
    {
        
    }

   public void OccupancyCheck(MapGrid alfa)
    {
        if(prevpos!=null)
        prevpos.GetComponent<MapGrid>().isoccupancy(false, null);
        alfa.isoccupancy(true, gameObject);
    }//위치로 이동

    private void OnMouseUp()
    {
        distance = Vector3.zero;
        objectHitPostion = newPos;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitLayerMask;
        int layerMask = 1 << LayerMask.NameToLayer("ROAD");
        if (Physics.Raycast(ray, out hitLayerMask, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);
            MapGrid alfa = hitLayerMask.collider.gameObject.GetComponent<MapGrid>();
            gameObject.transform.position = hitLayerMask.collider.gameObject.transform.position;
            if (alfa.occupanction()==false)//점유 상태가 아닐때
            {
                OccupancyCheck(alfa);
                prevpos = hitLayerMask.collider.gameObject.transform;
            }
            else//점유 상태일때
            {
                gameObject.transform.position = prevpos.position;
                if (alfa.player == gameObject)
                {
                    return;
                }
                //alfa.player.transform.position = prevpos.transform.position;
                //alfa.player.GetComponent<SelecPos>().OccupancyCheck(prevpos.GetComponent<MapGrid>());
                //alfa.player.GetComponent<SelecPos>().prevpos = this.prevpos;
                //OccupancyCheck(alfa);
            }
        }
        else
        {
            if(prevpos!=null)
            gameObject.transform.position = prevpos.position;//올바른 위치가 아니면 이전 위치로
        }
        _Sg.Check();
    }
    private void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitLayerMask;
        int layerMask = 1 << LayerMask.NameToLayer("ROAD");
        if (Physics.Raycast(ray, out hitLayerMask, Mathf.Infinity, layerMask))
        {
            objectHitPostion = hitLayerMask.point;
            //if(prevpos.GetComponent<MapGrid>().player == gameObject)
            //prevpos = hitLayerMask.collider.gameObject.transform;
        }
    }

    private void OnMouseDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitLayerMask;
        int layerMask = 1 << LayerMask.NameToLayer("ROAD");
        if (Physics.Raycast(ray, out hitLayerMask, Mathf.Infinity, layerMask))
        {
           // hitLayerMask.collider.gameObject.GetComponent<MapGrid>().ChangeColor();
            float H = Camera.main.transform.position.y;
            float h = objectHitPostion.y;
            if (distance == Vector3.zero) distance = transform.position - hitLayerMask.point;

            newPos
                = (hitLayerMask.point * (H - h) + Camera.main.transform.position * h) / H;
            gameObject.transform.position = hitLayerMask.point + distance;
        }
    }

}
