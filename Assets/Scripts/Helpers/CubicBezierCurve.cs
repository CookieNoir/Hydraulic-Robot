using System;
using UnityEngine;

public class CubicBezierCurve : MonoBehaviour
{
    [SerializeField] private Transform _transform0;
    [SerializeField] private Transform _transform1;
    [SerializeField] private Transform _transform2;
    [SerializeField] private Transform _transform3;
    [SerializeField, Min(2)] private int _pointsInCurve = 2;
    private Vector3[] _points;
    private Vector3 _point0;
    private Vector3 _point1;
    private Vector3 _point2;
    private Vector3 _point3;
    private float _length;
    private float _shortestSegment;
    private float _longestSegment;
    private float _step;
    private const float _THRESHOLD = 0.0001f;

    public void CreateCurve()
    {
        _points = new Vector3[_pointsInCurve];
        int segmentsCount = _pointsInCurve - 1;
        _step = 1f / segmentsCount;
        _point0 = _transform0.position;
        _point1 = _transform1.position;
        _point2 = _transform2.position;
        _point3 = _transform3.position;
        _points[0] = _transform0.position;
        _length = 0f;
        _shortestSegment = float.MaxValue;
        _longestSegment = 0f;
        float segmentLength;
        for (int i = 1; i < segmentsCount; ++i)
        {
            _points[i] = GetPointByParameter(i * _step);
            segmentLength = (_points[i] - _points[i - 1]).magnitude;
            _length += segmentLength;
            if (segmentLength < _shortestSegment) _shortestSegment = segmentLength;
            if (segmentLength > _longestSegment) _longestSegment = segmentLength;
        }
        _points[segmentsCount] = _transform3.position;
        segmentLength = (_points[segmentsCount] - _points[segmentsCount - 1]).magnitude;
        _length += segmentLength;
        if (segmentLength < _shortestSegment) _shortestSegment = segmentLength;
        if (segmentLength > _longestSegment) _longestSegment = segmentLength;
    }

    public float GetLength()
    {
        return _length;
    }

    public Vector3 GetPointByParameter(float t)
    {
        float tSquare = t * t;
        float mt = 1f - t;
        float mtSquare = mt * mt;
        return mt * mtSquare * _point0 + 3f * t * mtSquare * _point1 + 3f * tSquare * mt * _point2 + t * tSquare * _point3;
    }

    public Vector3 GetProjectionOfPoint(Vector3 point, out float shortestDistance)
    {
        int point1index, point2index;
        _GetIndexesOfNearestSegment(point, out point1index, out point2index, out shortestDistance);
        Vector3 ab = _points[point2index] - _points[point1index];
        Vector3 ap = point - _points[point1index];
        return _points[point1index] + Vector3.Dot(ap, ab) / Vector3.Dot(ab, ab) * ab;
    }

    public float GetProjectionAndParameterValueOfPoint(Vector3 point, out float shortestDistance, ref Vector3 projection)
    {
        float result;
        int point1index, point2index;
        _GetIndexesOfNearestSegment(point, out point1index, out point2index, out shortestDistance);
        Vector3 ab = _points[point2index] - _points[point1index];
        Vector3 ap = point - _points[point1index];
        projection = _points[point1index] + Vector3.Dot(ap, ab) / Vector3.Dot(ab, ab) * ab;
        ap = projection - _points[point1index];
        if (Math.Abs(ab.x) > _THRESHOLD)
        {
            result = ap.x / ab.x;
        }
        else
        {
            if (Math.Abs(ab.y) > _THRESHOLD)
            {
                result = ap.y / ab.y;
            }
            else
            {
                if (Math.Abs(ab.z) > _THRESHOLD)
                {
                    result = ap.z / ab.z;
                }
                else
                {
                    result = 0f;
                }
            }
        }
        result += point1index * _step;
        return result;
    }

    private void _GetIndexesOfNearestSegment(Vector3 point, out int point1, out int point2, out float shortestDistance)
    {
        point1 = 0;
        float sqrDistance = (_points[0] - point).sqrMagnitude;
        float nearestSqrDistance = sqrDistance;
        for (int i = 1; i < _pointsInCurve; ++i)
        {
            sqrDistance = (_points[i] - point).sqrMagnitude;
            if (sqrDistance < nearestSqrDistance)
            {
                point1 = i;
                nearestSqrDistance = sqrDistance;
            }
        }
        if (point1 > 0)
        {
            point2 = point1 - 1;
            float secondSqrDistance = (_points[point2] - point).sqrMagnitude;
            if (point1 < _pointsInCurve - 1)
            {
                sqrDistance = (_points[point1 + 1] - point).sqrMagnitude;
                if (sqrDistance < secondSqrDistance)
                {
                    point2 = point1 + 1;
                }
            }
        }
        else
        {
            point2 = 1;
        }
        if (point2 < point1)
        {
            point1 += point2;
            point2 = point1 - point2;
            point1 -= point2;
        }
        shortestDistance = Mathf.Sqrt(sqrDistance);
    }

    private void OnValidate()
    {
        if (_transform0 && _transform1 && _transform2 && _transform3)
        {
            CreateCurve();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_points != null && _length > _THRESHOLD)
        {
            Gizmos.color = Color.green;
            for (int i = 1; i < _pointsInCurve; ++i)
            {
                Gizmos.DrawLine(_points[i - 1], _points[i]);
            }
        }
    }
}