using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapGrid : MonoBehaviour
{
    SelectGrid _selectGrid;
    Vector2Int _coordinate;
    bool _isoccupancy = false;
    public GameObject player;
    public Vector2Int Coordinate { get { return _coordinate; } }
    void Awake()
    {
        if(_selectGrid==null)
            _selectGrid = transform.parent.GetComponent<SelectGrid>();
    }
    
    private void OnMouseEnter()
    {
        ChangeColor();
    }

    public void isoccupancy(bool isoccupancy,PlayerBase pb)
    {
        _isoccupancy = isoccupancy;
        player = pb.gameObject;
    }
    public bool occupanction()
    {
        return _isoccupancy;
    }

    public void ChangeColor()
    {
        if (!_isoccupancy)
            GetComponent<MeshRenderer>().material.color = Color.white;
        else
            GetComponent<MeshRenderer>().material.color = Color.red;
    }
   
    public void coordinateReturn(int x,int y)
    {
        _coordinate = new Vector2Int(x,y);
    }
    public Vector2Int coordinateReturn()
    {
        return _coordinate;
    }
}
