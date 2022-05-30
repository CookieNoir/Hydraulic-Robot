using UnityEngine;

public class CircleSegmentBorders : MonoBehaviour
{
    [Range(0f, 360f)] public float lowerAngle;
    [Range(0f, 360f)] public float upperAngle;
    protected const float DOUBLE_PI = Mathf.PI * 2f;
    protected float lowerAngleRad;
    protected float upperAngleRelativeToLowerRad;

    protected void SetBorders()
    {
        lowerAngleRad = lowerAngle * Mathf.Deg2Rad;
        upperAngleRelativeToLowerRad = upperAngle - lowerAngle;
        if (upperAngleRelativeToLowerRad < 0) upperAngleRelativeToLowerRad += 360f;
        upperAngleRelativeToLowerRad *= Mathf.Deg2Rad;
    }

    public bool OutsideBorders(float angleRad)
    {
        angleRad %= DOUBLE_PI;
        angleRad -= lowerAngleRad;
        if (angleRad < 0) angleRad += DOUBLE_PI;
        if (angleRad <= upperAngleRelativeToLowerRad) return false;
        else return true;
    }

    private void OnDrawGizmosSelected()
    {
        SetBorders();
        Gizmos.color = Color.green;
        Vector3 previousPoint = transform.position + PolarCoordinateSystem.PolarToCartesianXZ(lowerAngleRad, 1f),
            currentPoint;
        Gizmos.DrawLine(transform.position, previousPoint);

        int n = 32;
        float nInversed = upperAngleRelativeToLowerRad / n, currentAngle;

        for (int i = 1; i <= n; ++i)
        {
            currentAngle = lowerAngleRad + i * nInversed;
            currentPoint = transform.position + PolarCoordinateSystem.PolarToCartesianXZ(currentAngle, 1f);
            Gizmos.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }
        Gizmos.DrawLine(previousPoint, transform.position);
    }
}
