using UnityEngine;

public class Node  : IHeapItem<Node>
{
    int _gridX;
    int _gridY;
    int _Index;

    public Node _parent//부모노드
    {
        get;set;
    }

    public bool _walkable
    {
        get; set;
    }

    public Vector3 _wolrdPosition
    {
        get;set;
    }

    public int _gX
    {
        get { return _gridX; }
    }

    public int _gY
    {
        get { return _gridY; }
    }

    public int _gCost
    {
        get;set;
    }
    public int _hCost
    {
        get;set;
    }

    public int _fCost
    {
        get { return _gCost + _hCost; }
    }
    public int _heapIndex
    {
        get { return _Index; }
        set { _Index = value; }
    }


    public Node(bool walkable,Vector3 wolrdpos,int gridX,int gridY)
    {
        _walkable = walkable;
        _wolrdPosition = wolrdpos;
        _gridX = gridX;
        _gridY = gridY;
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = _fCost.CompareTo(nodeToCompare._fCost);
        if (compare == 0)
            compare = _hCost.CompareTo(nodeToCompare._hCost);
        return -compare;
    }
}
