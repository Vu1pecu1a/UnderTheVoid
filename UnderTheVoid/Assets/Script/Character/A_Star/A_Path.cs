using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Path
{
    public readonly Vector3[] _lockPoint;
    public readonly Line[] _turnBoundaries;
    public readonly int _finishLineIndex;
    public readonly int _slowDownIndex;

    public A_Path(Vector3[] waypoints, Vector3 startPos, float turnDst,float stoppingDst)
    {
        _lockPoint = waypoints;
        _turnBoundaries = new Line[_lockPoint.Length];
        _finishLineIndex = _turnBoundaries.Length - 1;

        Vector2 previousPoint = V3ToV2(startPos);
        for (int n = 0; n<_lockPoint.Length;n++)
        {
            Vector2 currnet = V3ToV2(_lockPoint[n]);
            Vector2 dirToCurrentPoint = (currnet - previousPoint).normalized;
            Vector2 trunBoundaryPoint = (n == _finishLineIndex) ? currnet : currnet - dirToCurrentPoint * turnDst;
            _turnBoundaries[n] = new Line(trunBoundaryPoint, previousPoint - dirToCurrentPoint * turnDst);
            previousPoint = trunBoundaryPoint;
        }
        float dstFromEndPoint = 0;
        for(int n = _lockPoint.Length -1;n>0;n--)
        {
            dstFromEndPoint += Vector3.Distance(_lockPoint[n], _lockPoint[n - 1]);
            if(dstFromEndPoint>stoppingDst)
            {
                _slowDownIndex = n;
                break;
            }
        }
    }

    Vector2 V3ToV2(Vector3 v3)
    {
        return new Vector2(v3.x, v3.z);
    }

    public void DrawWithGizoms()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 point in _lockPoint)
        {
            Gizmos.DrawCube(point + Vector3.up, Vector3.one);
        }
        Gizmos.color = Color.white;
        foreach(Line l in _turnBoundaries)
        {
            l.DrawWithGizmos(10);
        }
    }
}
