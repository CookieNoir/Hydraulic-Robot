using UnityEngine;
[RequireComponent(typeof(CapsuleCollider))]
public class PointOfInterest : ReachablePoint
{
    [Header("Модификаторы размера")]
    public float radius = 1f;
    public float physicalRadiusOffset = 0.05f;
    public float height = 1f;

    private void OnValidate()
    {
        transform.localScale = new Vector3(radius, height, radius);
        GetComponent<CapsuleCollider>().radius = 1 - physicalRadiusOffset / radius;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + new Vector3(0, 0, radius - physicalRadiusOffset), transform.position + new Vector3(0, 0, radius));
        Gizmos.DrawLine(transform.position - new Vector3(0, 0, radius - physicalRadiusOffset), transform.position - new Vector3(0, 0, radius));
        Gizmos.DrawLine(transform.position + new Vector3(radius - physicalRadiusOffset, 0, 0), transform.position + new Vector3(radius, 0, 0));
        Gizmos.DrawLine(transform.position - new Vector3(radius - physicalRadiusOffset, 0, 0), transform.position - new Vector3(radius, 0, 0));
    }
}
