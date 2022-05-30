using UnityEngine;

public class SpiralPoints : CircleSegmentBorders
{
    [Min(0f)] public float lowerRadius;
    [Min(0f)] public float upperRadius;
    [Space(15)]
    [Min(0)] public int stepAtStart;

    private const float VOGEL = 16.4495926918f;

    private int pointsCount;
    private float angleRadAddition; // Случайное число от 0 до 2*PI
    private float[] anglesRad;
    private int[] steps;
    private float vogelMultiplier;

    public void CreateSequence(in int count)
    {
        SetBorders();
        pointsCount = count;
        anglesRad = new float[pointsCount];
        steps = new int[pointsCount];
        angleRadAddition = Random.Range(0f, DOUBLE_PI);
        if (pointsCount > 0)
        {
            steps[0] = stepAtStart;
            SetVogelAngle(0);
            for (int i = 1; i < pointsCount; ++i)
            {
                steps[i] = steps[i - 1] + 1;
                SetVogelAngle(i);
            }
            vogelMultiplier = (upperRadius - lowerRadius) / Mathf.Sqrt(steps[pointsCount - 1]);
        }
    }

    public Vector3 GetWorldPositionOfPoint(in int i)
    {
        if (i > -1 && i < pointsCount)
            return transform.position + PolarCoordinateSystem.PolarToCartesianXZ(anglesRad[i],
                                        lowerRadius + vogelMultiplier * Mathf.Sqrt(steps[i]));
        else return Vector3.zero;
    }

    private void SetVogelAngle(in int i)
    {
        int steps = this.steps[i];
        float angleRad = VOGEL * steps + angleRadAddition;
        while (OutsideBorders(angleRad))
        {
            steps++;
            angleRad = VOGEL * steps + angleRadAddition;
        }
        anglesRad[i] = angleRad;
        this.steps[i] = steps;
    }

    private void OnDrawGizmosSelected()
    {
        SetBorders();
        Gizmos.color = Color.green;
        Vector3 previousPointLower = transform.position + PolarCoordinateSystem.PolarToCartesianXZ(lowerAngleRad, lowerRadius),
                previousPointUpper = transform.position + PolarCoordinateSystem.PolarToCartesianXZ(lowerAngleRad, upperRadius),
            currentPointLower, currentPointUpper;
        Gizmos.DrawLine(previousPointLower, previousPointUpper);

        int n = 32;
        float nInversed = upperAngleRelativeToLowerRad / n, currentAngle;

        for (int i = 1; i <= n; ++i)
        {
            currentAngle = lowerAngleRad + i * nInversed;
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
