using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
   
    [SerializeField] Transform _target;
    const float _minPathUpdataTime = 0.2f;
    const float _pathUpdateMoveThreshold = 0.5f;
    float _speed = 10;
    float _trunDest = 5;
    float _turnSpeed = 3;
    float _stoppinDst = 10;
    // [SerializeField]
    // Vector3[] _path;
    // int _nextIndex;
    A_Path _path;

    private void Start()
    {
        {
            // PathRequestManager.RequestPath(transform.position, _target.position, OnPathFound);
            StartCoroutine(UpdatePath());
        }
    }

    public void OnPathFound(Vector3[] newPath,bool pathSuccessful)
    {
        if(pathSuccessful)
        {
            //_path = newPath;
            //_nextIndex = 0;

            _path = new A_Path(newPath, transform.position, _trunDest,_stoppinDst);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }
    
    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad > 0.3f)
            yield return new WaitForSeconds(0.3f);
        PathRequestManager.RequestPath(transform.position, _target.position, OnPathFound);
        float sqrMoveThreshold = _pathUpdateMoveThreshold * _pathUpdateMoveThreshold;
        Vector3 targetposOld = _target.position;
        while(true)
        {
            yield return new WaitForSeconds(_minPathUpdataTime);
            if ((_target.position - targetposOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(transform.position, _target.position, OnPathFound);
                targetposOld = _target.position;
            }
        }
    }

    IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(_path._lockPoint[0]);
        float speedPercont = 1;
        
        //Vector3 currentWayPoint = _path[0];
        while(followingPath)
        {
            {
                //if(transform.position == currentWayPoint)
            //{
            //    _nextIndex++;
            //    if (_nextIndex >= _path.Length)
            //        yield break;
            //    currentWayPoint = _path[_nextIndex];
            //}

            //transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, _speed * Time.deltaTime);

            }
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
            while(_path._turnBoundaries[pathIndex].HasCroosedLine(pos2D))
            {
                if (pathIndex == _path._finishLineIndex)
                {
                    followingPath = false;
                    break;
                }
                else
                    pathIndex++;
            }
            if(followingPath)
            {
                if(pathIndex >= _path._slowDownIndex && _stoppinDst > 0)
                {
                    float val = _path._turnBoundaries[_path._finishLineIndex].DistanceFromPoint(pos2D) / _stoppinDst;
                    speedPercont = Mathf.Clamp01(val);
                    if (speedPercont < 0.01f)
                        followingPath = false;
                }
            }
            Quaternion targetRotation = Quaternion.LookRotation(_path._lockPoint[pathIndex] - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _turnSpeed);
            transform.Translate(Vector3.forward * Time.deltaTime * _speed * speedPercont, Space.Self);
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if(_path != null)
        {
            _path.DrawWithGizoms();
            //for(int n = _nextIndex; n<_path.Length; n++)
            //{
            //    Gizmos.color = Color.black;
            //    Gizmos.DrawCube(_path[n], Vector3.one);
            //    if (n == _nextIndex)
            //        Gizmos.DrawLine(transform.position, _path[n]);
            //    else
            //        Gizmos.DrawLine(_path[n - 1], _path[n]);
            //}
        }
        
    }
}
