using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapGrid : MonoBehaviour
{
    SelectGrid _selectGrid;
    Vector2Int _coordinate;
    [SerializeField]
    bool _isoccupancy = false;
    public GameObject player;
    public Vector2Int Coordinate { get { return _coordinate; } }
    void Awake()
    {
        if(_selectGrid==null)
            _selectGrid = transform.parent.GetComponent<SelectGrid>();
        _selectGrid.CheckAction += ChangeColor;
    }

    private void Start()
    {
        ChangeColor();
    }
    //private void OnMouseEnter()
    //{
    //    ChangeColor();
    //}

    public void isoccupancy(bool isoccupancy,GameObject pb)
    {
        _isoccupancy = isoccupancy;
        player = pb;
    }
    public bool occupanction()
    {
        return _isoccupancy;
    }

    public void ChangeColor()
    {
        if (!_isoccupancy)
            GetComponent<MeshRenderer>().material.SetColor("EmissionColor", Color.red);
        else
            GetComponent<MeshRenderer>().material.color = Color.white;
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
