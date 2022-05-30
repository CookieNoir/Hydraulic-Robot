using UnityEngine;
[AddComponentMenu("Center Of Mass/Local Center Of Mass")]
public class CenterOfMass : MonoBehaviour
{
    public Vector3 centerOfMass;
    public float mass;

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.TransformPoint(centerOfMass), 0.1f);
    }
}
