using UnityEngine;

public static class SimpleFunctions
{
    public static float Smoothstep(float f)
    {
        return f * f * (3f - f - f);
    }

    public static float SquareRootOfOneMinusSquareSrc(float src)
    {
        return Mathf.Sqrt(1 - src * src);
    }

    public static float CosOfAngleBetweenTwoVectors(Vector3 v1, Vector3 v2)
    {
        return (v1.x * v2.x + v1.y * v2.y + v1.z * v2.z) / (v1.magnitude * v2.magnitude);
    }
}
