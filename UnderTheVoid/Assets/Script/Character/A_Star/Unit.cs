using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] Transform _target;
    float _speed = 10;
    [SerializeField]
    Vector3[] _path;
    int _nextIndex;

    private void Start()
    {
        {
            PathRequestManager.RequestPath(transform.position, _target.position, OnPathFound);
        }
    }

    public void OnPathFound(Vector3[] newPath,bool pathSuccessful)
    {
        if(pathSuccessful)
        {
            _path = newPath;
            _nextIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }
    
    IEnumerator FollowPath()
    {
        Vector3 currentWayPoint = _path[0];
        while(true)
        {
            if(transform.position == currentWayPoint)
            {
                _nextIndex++;
                if (_nextIndex >= _path.Length)
                    yield break;
                currentWayPoint = _path[_nextIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, _speed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if(_path != null)
        {
            for(int n = _nextIndex; n<_path.Length; n++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(_path[n], Vector3.one);
                if (n == _nextIndex)
                    Gizmos.DrawLine(transform.position, _path[n]);
                else
                    Gizmos.DrawLine(_path[n - 1], _path[n]);
            }
        }
        
    }
}
