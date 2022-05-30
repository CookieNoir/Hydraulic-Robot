using System.Collections.Generic;
using UnityEngine;

public class CircularPath : CircleSegmentBorders
{
    [Min(0f)] public float lowerRadius;
    [Min(0f)] public float upperRadius;
    public Vector2 heightRange;
    public float yellowZoneHeightAddition;
    [Space(15)]
    public Vector3[] mainPositions;
    private Vector3[] allPositions;
    private List<float> anglesRad;
    private int count;
    private float[] progressAtPoint;
    private float pathLength;

    private void Awake()
    {
        anglesRad = new List<float>();
    }

    public int SetPositions(int expectedSegmentsCount, int pointsInSegment) // Возвращает количество основных позиций
    {
        if (expectedSegmentsCount < 1) return 0;
        SetBorders();
        anglesRad.Clear();
        float step = DOUBLE_PI / expectedSegmentsCount, halfStep = step * 0.5f, threeSecondStep = step * 1.5f,
              angle = 0f, angleAddition = Random.Range(0f, DOUBLE_PI), direction = Mathf.Sign(Random.Range(-1f, 1f));
        angle = 0;
        anglesRad.Add(angle);
        while (DOUBLE_PI - angle > threeSecondStep)
        {
            angle += Random.Range(halfStep, threeSecondStep);
            anglesRad.Add(angle);
        }
        int anglesRadCount = anglesRad.Count;
        count = anglesRadCount * pointsInSegment;
        mainPositions = new Vector3[anglesRadCount];
        allPositions = new Vector3[count];
        progressAtPoint = new float[count];

        mainPositions[0] = transform.position + PolarCoordinateSystem.PolarToCartesianXZ(angleAddition, Random.Range(lowerRadius, upperRadius));
        if (OutsideBorders(angleAddition)) mainPositions[0] += Vector3.up * Random.Range(heightRange.x, heightRange.y);
        else mainPositions[0] += Vector3.up * (yellowZoneHeightAddition + Random.Range(heightRange.x, heightRange.y));

        allPositions[0] = mainPositions[0];
        progressAtPoint[0] = 0f;

        pathLength = 0;
        int curIndex = 0, prevIndex = 0;
        float prevLength = 0, pointsInSegmentInverted = 1f / pointsInSegment, f = 0;

        for (int i = 1; i < anglesRadCount; ++i)
        {
            curIndex += pointsInSegment;
            angle = angleAddition + direction * anglesRad[i];
            mainPositions[i] = transform.position + PolarCoordinateSystem.PolarToCartesianXZ(angle, Random.Range(lowerRadius, upperRadius));
            if (OutsideBorders(angle)) mainPositions[i] += Vector3.up * Random.Range(heightRange.x, heightRange.y);
            else mainPositions[i] += Vector3.up * (yellowZoneHeightAddition + Random.Range(heightRange.x, heightRange.y));
            prevLength = pathLength;
            pathLength += Vector3.Magnitude(mainPositions[i] - mainPositions[i - 1]);

            allPositions[curIndex] = mainPositions[i];
            progressAtPoint[curIndex] = pathLength;
        }
        pathLength += Vector3.Magnitude(mainPositions[0] - mainPositions[anglesRadCount - 1]);

        for (int i = 1; i < anglesRadCount; ++i)
        {
            curIndex = i * pointsInSegment;
            progressAtPoint[curIndex] /= pathLength;

            for (int j = 1; j < pointsInSegment; ++j)
            {
                f = j * pointsInSegmentInverted;
                allPositions[prevIndex + j] = Vector3.Lerp(allPositions[prevIndex], allPositions[curIndex], f);
                progressAtPoint[prevIndex + j] = Mathf.Lerp(progressAtPoint[prevIndex], progressAtPoint[curIndex], f);
            }
            prevIndex = curIndex;
        }
        curIndex = 0;
        progressAtPoint[curIndex] = 1f;
        for (int j = 1; j < pointsInSegment; ++j)
        {
            f = j * pointsInSegmentInverted;
            allPositions[prevIndex + j] = Vector3.Lerp(allPositions[prevIndex], allPositions[curIndex], f);
            progressAtPoint[prevIndex + j] = Mathf.Lerp(progressAtPoint[prevIndex], progressAtPoint[curIndex], f);
        }
        progressAtPoint[0] = 0f;

        return anglesRadCount;
    }

    public int GetCount()
    {
        return count;
    }

    public Vector3 GetPositionByIndex(int index)
    {
        index %= count;
        return allPositions[index];
    }

    public float GetProgressByIndex(int index)
    {
        index %= count;
        return progressAtPoint[index];
    }

    public float GetPathLength()
    {
        return pathLength;
    }

    private void OnDrawGizmosSelected()
    {
        SetBorders();
        Gizmos.color = Color.yellow;
        Vector3 positionWithUpAddition = transform.position + Vector3.up * yellowZoneHeightAddition;
        Vector3 previousPointLower = positionWithUpAddition + PolarCoordinateSystem.PolarToCartesianXZ(lowerAngleRad, lowerRadius),
                previousPointUpper = positionWithUpAddition + PolarCoordinateSystem.PolarToCartesianXZ(lowerAngleRad, upperRadius),
            currentPointLower, currentPointUpper;
        Gizmos.DrawLine(previousPointLower, previousPointUpper);

        int n = 32;
        float nInversed = upperAngleRelativeToLowerRad / n, currentAngle = 0;

        for (int i = 1; i <= n; ++i)
        {
            currentAngle = lowerAngleRad + i * nInversed;
            currentPointLower = positionWithUpAddition + PolarCoordinateSystem.PolarToCartesianXZ(currentAngle, lowerRadius);
            currentPointUpper = positionWithUpAddition + PolarCoordinateSystem.PolarToCartesianXZ(currentAngle, upperRadius);
            Gizmos.DrawLine(previousPointLower, currentPointLower);
            Gizmos.DrawLine(previousPointUpper, currentPointUpper);
            previousPointLower = currentPointLower;
            previousPointUpper = currentPointUpper;
        }
        Gizmos.DrawLine(previousPointLower, previousPointUpper);

        Gizmos.color = Color.green;
        previousPointLower -= Vector3.up * yellowZoneHeightAddition;
        previousPointUpper -= Vector3.up * yellowZoneHeightAddition;
        Gizmos.DrawLine(previousPointLower, previousPointUpper);
        nInversed = (DOUBLE_PI - upperAngleRelativeToLowerRad) / n;
        float upperAngleRad = lowerAngleRad + upperAngleRelativeToLowerRad;
        for (int i = 1; i <= n; ++i)
        {
            currentAngle = upperAngleRad + i * nInversed;
            currentPointLower = transform.position + PolarCoordinateSystem.PolarToCartesianXZ(currentAngle, lowerRadius);
            currentPointUpper = transform.position + PolarCoordinateSystem.PolarToCartesianXZ(currentAngle, upperRadius);
            Gizmos.DrawLine(previousPointLower, currentPointLower);
            Gizmos.DrawLine(previousPointUpper, currentPointUpper);
            previousPointLower = currentPointLower;
            previousPointUpper = currentPointUpper;
        }
        Gizmos.DrawLine(previousPointLower, previousPointUpper);
    }
}
