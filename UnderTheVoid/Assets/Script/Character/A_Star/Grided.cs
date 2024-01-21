using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grided : MonoBehaviour
{
    [SerializeField] LayerMask _unwalkableMask;
    [SerializeField] Vector2 _gridWorldSize;
    [SerializeField] float _nodeRadius;
    [SerializeField] bool _onlyDisplayPathGizmos;//보이기 옵션


    Node[,] _grid;//2차원 배열
    float _nodeDiameter; //지름
    int _gridSizeX, _gridSizeY;//노드크기

    public List<Node> _path
    {
        get;set;
    }

    public int _maxSize
    {
        get { return _gridSizeX * _gridSizeY;}
    }

    private void Awake()
    {
        _nodeDiameter = _nodeRadius*2;
        _gridSizeX = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiameter);
        _gridSizeY = Mathf.RoundToInt(_gridWorldSize.y / _nodeDiameter);

        CreatGird();
    }

    // Start is called before the first frame update
    void Start()
    {
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

                _grid[x, y] = new Node(walkable, wolrdPoint,x,y);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_gridWorldSize.x, 1, _gridWorldSize.y));

        if(_onlyDisplayPathGizmos)
        {
            if (_path != null)
            {
                foreach (Node node in _path)
                {
                        Gizmos.color = Color.black;
                        Gizmos.DrawCube(node._wolrdPosition, Vector3.one * (_nodeDiameter - 0.1f));
                }
            }
        }
        else//그리드가 있을 때 만
        {
            if (_path != null)
            {
                foreach (Node node in _grid)
                {
                    Gizmos.color = node._walkable ? Color.white : Color.red;
                    if (_path != null)
                    {
                        if (_path.Contains(node))
                            Gizmos.color = Color.black;
                    }

                    Gizmos.DrawCube(node._wolrdPosition, Vector3.one * (_nodeDiameter - 0.1f));
                }
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for(int x = -1; x<=1; x++)
        {
            for(int y = -1;y<=1;y++)
            {
                if (x == 0 && y == 0)
                    continue;
                int checkX = node._gX + x;
                int checkY = node._gY + y;

                if(checkX >= 0 && checkX< _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
                {
                    neighbours.Add(_grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFormWoldPosition(Vector3 wolrdposition)//월드 포지션 기반으로 그리드 위치 받아오는 함수
    {
        float percentX = (wolrdposition.x + _gridWorldSize.x / 2) / _gridWorldSize.x;
        float percentY = (wolrdposition.z + _gridWorldSize.y / 2) / _gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);

        return _grid[x, y];
    }
}
