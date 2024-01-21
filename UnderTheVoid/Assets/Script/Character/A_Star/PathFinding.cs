using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    [SerializeField] Transform _seeker,_target;

    Grided _grid;
    // Start is called before the first frame update
    private void Awake()
    {
        _grid = GetComponent<Grided>();
    }

    private void Update()
    {
        FindPath(_seeker.position, _target.position);
    }

    void FindPath(Vector3 startPos,Vector3 goalPos)
    {
        Node startNode = _grid.NodeFormWoldPosition(startPos);
        Node goalNode = _grid.NodeFormWoldPosition(goalPos);

        Heap<Node> openSet = new Heap<Node>(_grid._maxSize);
        HashSet<Node> closeSet = new HashSet<Node>();//끝난길을 넣어두는곳
        openSet.Add(startNode);

        while(openSet._count >0)
        {
            /*
            Node node = openSet[0];
            for(int n = 1; n< openSet.Count;n++)
            {
                if(openSet[n]._fCost<node._fCost|| openSet[n]._fCost == node._fCost)
                {
                    if (openSet[n]._hCost < node._hCost)
                        node = openSet[n];
                }
            }
            openSet.Remove(node);
            
             */
            Node node = openSet.RemoveFirst();

            closeSet.Add(node);

            if(node == goalNode)//시작점과 골인지점이 같으면 걸러주는 용도
            {
                //패스 재설정
                RetracePath(startNode,goalNode);
                return;
            }

            foreach(Node neighbour in _grid.GetNeighbours(node))
            {
                if (!neighbour._walkable || closeSet.Contains(neighbour))
                    continue;

                int newCostToNeighbour = node._gCost + GetDistance(node, neighbour);

                if(newCostToNeighbour<neighbour._gCost || !openSet.Contains(neighbour))
                {
                    neighbour._gCost = newCostToNeighbour;
                    neighbour._hCost = GetDistance(neighbour, goalNode);
                    neighbour._parent = node;
                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }
    void RetracePath(Node startNode,Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while(currentNode!= startNode)
        {
            path.Add(currentNode);//현재 노드 추가
            currentNode = currentNode._parent; //현재 노드는 노드의 부모노드
        }
        path.Reverse();

        _grid._path = path;
    }

    int GetDistance(Node nodeA,Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA._gX - nodeB._gX);
        int dstY = Mathf.Abs(nodeA._gY - nodeB._gY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
