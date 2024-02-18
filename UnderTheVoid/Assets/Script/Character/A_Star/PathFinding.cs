using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;//패스 체크용

public class PathFinding : MonoBehaviour
{
  //  [SerializeField] Transform _seeker,_target;

    Grided _grid;
    // Start is called before the first frame update
    private void Awake()
    {
        _grid = GetComponent<Grided>();
    }
    /*
    private void Update()
    {
        FindPath(_seeker.position, _target.position);
    }*/

    public void StartFindPath(Vector3 startPos,Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos,Vector3 goalPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] wayPoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = _grid.NodeFormWoldPosition(startPos);
        Node goalNode = _grid.NodeFormWoldPosition(goalPos);
        UnityEngine.Debug.Log(goalPos);
        UnityEngine.Debug.Log(goalNode._gX);
        UnityEngine.Debug.Log(goalNode._gY);

        startNode._parent = startNode;

        if(startNode._walkable && goalNode._walkable)
        {
            Heap<Node> openSet = new Heap<Node>(_grid._maxSize);
            HashSet<Node> closeSet = new HashSet<Node>();//끝난길을 넣어두는곳
            openSet.Add(startNode);

            while (openSet._count > 0)
            {
                Node currntNod = openSet.RemoveFirst();
                closeSet.Add(currntNod);

                if(currntNod == goalNode)
                {
                    sw.Stop();
                    print("path found :" + sw.ElapsedMilliseconds + "ms");
                    pathSuccess = true;
                    break;
                }

                int count = 0;

                foreach (Node neighbour in _grid.GetNeighbours(currntNod))
                {
                    if (!neighbour._walkable || closeSet.Contains(neighbour))
                        continue;
                    count++;

                    int newCostToNeighbour = currntNod._gCost + GetDistance(currntNod, neighbour)
                        +neighbour._movementPenalty;

                    if (newCostToNeighbour < neighbour._gCost || !openSet.Contains(neighbour))
                    {
                        neighbour._gCost = newCostToNeighbour;
                        neighbour._hCost = GetDistance(neighbour, goalNode);
                        neighbour._parent = currntNod;
                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
               // Debug.Log("neughbour check count" + count.ToString());
            }
        }

        yield return null;

        if(pathSuccess)
        {
            wayPoints = RetracePath(startNode, goalNode);
        }
        else
        {
            print(" 길 없음");
        }
        PathRequestManager._instance.FinishedProcessingPath(wayPoints, pathSuccess);

    }
    Vector3[] RetracePath(Node startNode,Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while(currentNode!= startNode)
        {
            path.Add(currentNode);//현재 노드 추가
            currentNode = currentNode._parent; //현재 노드는 노드의 부모노드
        }
        // path.Reverse();

        // _grid._path = path;

        Vector3[] wayPoints = SimplifyPath(path);
        Array.Reverse(wayPoints);

        return wayPoints;
    }

    int GetDistance(Node nodeA,Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA._gX - nodeB._gX);
        int dstY = Mathf.Abs(nodeA._gY - nodeB._gY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> wayPoints = new List<Vector3>();
        Vector2 direcstionOld = Vector2.zero;

        wayPoints.Add(path[0]._wolrdPosition);
        for(int n= 1;n<path.Count;n++)
        {
            Vector2 directionNew = new Vector2(path[n - 1]._gX - path[n]._gX, path[n - 1]._gY - path[n]._gY);

            if (directionNew != direcstionOld)
                wayPoints.Add(path[n]._wolrdPosition);
            direcstionOld = directionNew;
        }
        return wayPoints.ToArray();
    }
}
