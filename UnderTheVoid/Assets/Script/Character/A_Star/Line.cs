using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line
{
    const float _verticalLineGradient = 1e5f;//»ó¼ö?

    float _gradient;
    float _interceptY;
    Vector2 _pointLine1;
    Vector2 _pointLine2;
    float _gradientPerpendicular;

    bool _approachSide;
    
    public Line(Vector2 pointOnLine,Vector2 pointperpendicularToLine)
    {
        float dx = pointOnLine.x - pointperpendicularToLine.x;
        float dy = pointOnLine.y - pointperpendicularToLine.y;

        if (dx == 0)
            _gradientPerpendicular = _verticalLineGradient;
        else
            _gradientPerpendicular = dy / dx;
        if (_gradientPerpendicular == 0)
            _gradient = _verticalLineGradient;
        else
            _gradient = -1 / _gradientPerpendicular;

        _interceptY = pointOnLine.y - _gradient * pointOnLine.x;
        _pointLine1 = pointOnLine;
        _pointLine2 = pointOnLine + new Vector2(1, _gradient);
        _approachSide = false;
        _approachSide = GetSide(pointperpendicularToLine);
    }

    bool GetSide(Vector2 point)
    {
        return (point.x - +_pointLine1.x) * (_pointLine2.y - _pointLine1.y) > 
            (point.y - _pointLine1.y) * (_pointLine2.x - _pointLine1.x);
    }

    public bool HasCroosedLine(Vector2 point)
    {
        return GetSide(point) != _approachSide;
    }

    public float DistanceFromPoint(Vector2 point)
    {
        float yInterceptPerpendicular = point.y - _gradientPerpendicular * point.x;
        float intersectX = (yInterceptPerpendicular - _interceptY) / (_gradient - _gradientPerpendicular);
        float intersectY = _gradient * intersectX + _interceptY;
        return Vector2.Distance(point, new Vector2(intersectX, intersectY));
    }

    public void DrawWithGizmos(float length)
    {
        Vector3 lintDir = new Vector3(1, 0, _gradient).normalized;
        Vector3 lineCenter = new Vector3(_pointLine1.x, 0, _pointLine1.y)+Vector3.up;
        Gizmos.DrawLine(lineCenter - lintDir * length / 2f, lineCenter + lintDir * length / 2f);
    }
}
