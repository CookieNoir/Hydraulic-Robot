using UnityEngine;

public static class PolarCoordinateSystem
{
    public static Vector3 PolarToCartesianXZ(in float angleRad, in float radius) // Угол в радианах
    {
        return new Vector3(radius * Mathf.Cos(angleRad), 0f, radius * Mathf.Sin(angleRad));
    }
}
