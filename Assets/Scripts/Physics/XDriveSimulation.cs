using System;
using UnityEngine;

public class XDriveSimulation : MonoBehaviour
{
    public float target = 0f;
    [Space(10)]
    public bool inRadians = true;
    public Vector3 anchorRotation;
    [Space(10)]
    public bool limited = false;
    public Vector2 limitationVector;
    private Vector3 localPosition;
    private Quaternion localRotation;
    private Vector3 modifiedRight;
    private Action action;

    private void Awake()
    {
        localPosition = transform.localPosition;
        localRotation = transform.localRotation;
        if (inRadians) action = SetAngle;
        else action = SetLocalPosition;
        modifiedRight = localRotation * Quaternion.Euler(anchorRotation) * Vector3.right;
    }

    private void SetLocalPosition()
    {
        transform.localPosition = localPosition + target * modifiedRight;
    }

    private void SetAngle()
    {
        transform.localRotation = localRotation;
        transform.Rotate(Vector3.right, target);
    }

    public void SetTarget(float newTarget)
    {
        target = limited ? Mathf.Clamp(newTarget, limitationVector.x, limitationVector.y) : newTarget;
        action();
    }
}
