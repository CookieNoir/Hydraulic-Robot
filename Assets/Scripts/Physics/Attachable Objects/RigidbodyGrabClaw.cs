using UnityEngine;

public class RigidbodyGrabClaw : MonoBehaviour
{
    public bool isRotationAxisInverted = false;

    private Vector3 rotationAxis;

    private void Awake()
    {
        rotationAxis = isRotationAxisInverted ? Vector3.down : Vector3.up;
    }

    public void SetLocalRotation(float angle)
    {
        transform.localRotation = Quaternion.Euler(rotationAxis * angle);
    }
}
