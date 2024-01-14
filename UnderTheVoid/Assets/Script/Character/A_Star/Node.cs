using UnityEngine;

public class Node 
{
    public bool _walkable
    {
        get; set;
    }

    public Vector3 _wolrdPosition
    {
        get;set;
    }

    public Node(bool walkable,Vector3 wolrdpos)
    {
        _walkable = walkable;
        _wolrdPosition = wolrdpos;
    }

}
