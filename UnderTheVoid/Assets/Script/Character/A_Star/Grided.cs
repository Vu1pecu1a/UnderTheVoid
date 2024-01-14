using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grided : MonoBehaviour
{
    [SerializeField] LayerMask _unwalkableMask;
    [SerializeField] Vector2 _gridWorldSize;
    [SerializeField] float _nodeRadius;

    Node[,] _grid;//2차원 배열
    float _nodeDiameter; //지름
    int _gridSizeX, _gridSizeY;//노드크기

    private void Awake()
    {
        _nodeDiameter = _nodeRadius*2;
        _gridSizeX = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiameter);
        _gridSizeY = Mathf.RoundToInt(_gridWorldSize.y / _nodeDiameter);
    }

    // Start is called before the first frame update
    void Start()
    {
        CreatGird();
    }

    void CreatGird()
    {
        _grid = new Node[_gridSizeX, _gridSizeY];
        Vector3 wolrdBottomLeft = 
            transform.position - Vector3.right * _gridWorldSize.x / 2-Vector3.forward * _gridWorldSize.y/2;
        
        for(int x = 0; x<_gridSizeX; x++)
        {
            for(int y = 0; y<_gridSizeY; y++)
            {
                Vector3 wolrdPoint = wolrdBottomLeft 
                    + Vector3.right * (x * _nodeDiameter + _nodeRadius) 
                    + Vector3.forward * (y * _nodeDiameter + _nodeRadius);

                bool walkable = !(Physics.CheckSphere(wolrdPoint, _nodeRadius, _unwalkableMask));

                _grid[x, y] = new Node(walkable, wolrdPoint);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_gridWorldSize.x, 1, _gridWorldSize.y));

        if(_grid != null)//그리드가 있을 때 만
        {
            foreach(Node node in _grid)
            {
                Gizmos.color = node._walkable ? Color.white : Color.red;
                Gizmos.DrawCube(node._wolrdPosition, Vector3.one * (_nodeDiameter - 0.1f));
            }
        }
    }

    public Node NodeFormWoldPosition(Vector3 wolrdposition)//월드 포지션 기반으로 그리드 위치 받아오는 함수
    {
        float percentX = (wolrdposition.x + _gridWorldSize.x / 2) / wolrdposition.x;
        float percentY = (wolrdposition.z + _gridWorldSize.y / 2) / wolrdposition.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);

        return _grid[x, y];
    }
}
