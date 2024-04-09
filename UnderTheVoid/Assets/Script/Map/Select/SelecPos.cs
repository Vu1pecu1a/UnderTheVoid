using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelecPos : MonoBehaviour
{
    [SerializeField]
    Vector3 objectHitPostion;
    Vector3 newPos;
    Vector3 distance;
    public Transform prevpos;//��������ġ

    [SerializeField]
    SelectGrid _Sg;
    [SerializeField]
    Renderer CH_Ma,WP_Ma,SubWP_Ma;
    

    private void Awake()
    {
        
    }

    void OutLiine()
    {
        CH_Ma.material.SetFloat("_OutlineWidth", 0.01f);
        WP_Ma.material.SetFloat("_OutlineWidth", 1f);
        if(SubWP_Ma!=null)
        SubWP_Ma.material.SetFloat("_OutlineWidth", 1f);
    }

    void RemoveOutLine()
    {
        CH_Ma.material.SetFloat("_OutlineWidth", 0);
        WP_Ma.material.SetFloat("_OutlineWidth", 0);
        if (SubWP_Ma != null)
            SubWP_Ma.material.SetFloat("_OutlineWidth", 0);
    }

   public void OccupancyCheck(MapGrid alfa)
    {
        if(prevpos!=null)
        prevpos.GetComponent<MapGrid>().isoccupancy(false, null);
        alfa.isoccupancy(true, gameObject);
    }//��ġ�� �̵�

    private void OnMouseUp()
    {
        RemoveOutLine();
        if (D_calcuate.isbattel == true)
            return;

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
            if (alfa.occupanction()==false)//���� ���°� �ƴҶ�
            {
                OccupancyCheck(alfa);
                prevpos = hitLayerMask.collider.gameObject.transform;
            }
            else//���� �����϶�
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
            gameObject.transform.position = prevpos.position;//�ùٸ� ��ġ�� �ƴϸ� ���� ��ġ��
        }
        SelectGrid.i.Check();
    }
    private void OnMouseDown()
    {
        if (D_calcuate.isbattel == true)
            return;
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
        if (D_calcuate.isbattel == true)
            return;
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
        OutLiine();
    }

}
