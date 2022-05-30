using UnityEngine;

public class SingleLookAt : MonoBehaviour
{
    public Transform target;
    public bool isOppositeDirection = false;

    private Vector3 targetPosition;
    private float multiplier;

    private void Awake()
    {
        multiplier = isOppositeDirection ? -1f : 1f;
    }

    public void LookAt()
    {
        targetPosition = multiplier * transform.InverseTransformPoint(target.position);
        transform.Rotate(Mathf.Atan2(targetPosition.z, targetPosition.magnitude) * Mathf.Rad2Deg, 0, 0);
    }
}
