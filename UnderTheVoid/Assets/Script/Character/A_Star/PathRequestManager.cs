using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour
{
    static PathRequestManager _uniqueInstance;

    struct PathRequest
    {
        public Vector3 _pathStart;
        public Vector3 _pathEnd;
        public Action<Vector3[], bool> _callback;

        public PathRequest(Vector3 start,Vector3 end,Action<Vector3[],bool> callback)
        {
            _pathStart = start;
            _pathEnd = end;
            _callback = callback;
        }
    }
    struct PathRequestV2
    {
        public Vector2Int _pathStart;
        public Vector2Int _pathEnd;
        public Action<Vector2[], bool> _callback;

        public PathRequestV2(Vector2Int start, Vector2Int end, Action<Vector2[], bool> callback)
        {
            _pathStart = start;
            _pathEnd = end;
            _callback = callback;
        }
    }
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    Queue<PathRequestV2> pathRequestQueueV2 = new Queue<PathRequestV2>();
    PathRequest _currentPathRequest;
    PathRequestV2 _currentPathRequestv2;
    PathFinding _pathfinding;

    bool _isProceesingPath;

    public static PathRequestManager _instance
    {
        get { return _uniqueInstance; }
    }

    private void Awake()
    {
        _uniqueInstance = this;
        _pathfinding = GetComponent<PathFinding>();
        _isProceesingPath = false;
    }

    public static void RequestPath(Vector3 pathStart,Vector3 pathEnd,Action<Vector3[],bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        _instance.pathRequestQueue.Enqueue(newRequest);
        _instance.TryProcessNext();
    }
    public static void RequestPath(Vector2Int pathStart, Vector2Int pathEnd, Action<Vector2[], bool> callback)
    {
        PathRequestV2 newRequest = new PathRequestV2(pathStart, pathEnd, callback);
        _instance.pathRequestQueueV2.Enqueue(newRequest);
        _instance.TryProcessNextV2();
    }
    void TryProcessNext()
    {
        if(!_isProceesingPath && pathRequestQueue.Count >0)
        {
            _currentPathRequest = pathRequestQueue.Dequeue();
            _isProceesingPath = true;
            // 여기서 길을 찾는다.
           _pathfinding.StartFindPath(_currentPathRequest._pathStart,_currentPathRequest._pathEnd);
        }
    }
    void TryProcessNextV2()
    {
        if (!_isProceesingPath && pathRequestQueueV2.Count > 0)
        {
            _currentPathRequestv2 = pathRequestQueueV2.Dequeue();
            _isProceesingPath = true;
            // 여기서 길을 찾는다.
            _pathfinding.StartFindPath(_currentPathRequestv2._pathStart, _currentPathRequestv2._pathEnd);
        }
    }
    public void FinishedProcessingPath(Vector3[] path,bool success)
    {
        _currentPathRequest._callback(path, success);
        _isProceesingPath = false;
        TryProcessNext();
    }

    public void FinishedProcessingPath(Vector2[] path, bool success)
    {
        _currentPathRequestv2._callback(path, success);
        _isProceesingPath = false;
        TryProcessNextV2();
    }
}
