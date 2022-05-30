using UnityEngine;

public class SingleTriangulation : MonoBehaviour
{
    public Transform point1;
    public Transform point2;

    private float difference;
    private float distance;
    private float distanceFromPoint0toPoint2;
    private float height;
    private float toPoint2;
    private Vector3 point1local;
    private Vector3 point2local;
    private Vector3 point0;

    private void Awake()
    {
        float toPoint1 = Vector3.Magnitude(transform.position - point1.position);
        toPoint1 *= toPoint1;
        toPoint2 = Vector3.Magnitude(transform.position - point2.position);
        toPoint2 *= toPoint2;
        difference = toPoint2 - toPoint1;
    }

    public void Triangulate()
    {
        point1local = transform.InverseTransformPoint(point1.position);
        point2local = transform.InverseTransformPoint(point2.position);
        distance = Vector3.Magnitude(point1local - point2local);
        distanceFromPoint0toPoint2 = (difference + distance * distance) / (distance + distance);
        height = Mathf.Sqrt(toPoint2 - distanceFromPoint0toPoint2 * distanceFromPoint0toPoint2);

        point0 = point1local + (distance - distanceFromPoint0toPoint2) / distance * (point2local - point1local);
        height = height / distance;
        distance = (point2local.z - point1local.z) * height;
        distanceFromPoint0toPoint2 = (point2local.y - point1local.y) * height;
        transform.localPosition += new Vector3(0, point0.y - distance, point0.z + distanceFromPoint0toPoint2);
    }
}
