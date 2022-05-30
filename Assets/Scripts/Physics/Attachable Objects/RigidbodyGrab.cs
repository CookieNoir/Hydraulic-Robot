using System.Collections.Generic;
using UnityEngine;

public class RigidbodyGrab : RigidbodyAccessory
{
    [Space(10, order = 0), Header("Грейферный захват", order = 1)]
    public RigidbodyGrabClaw leftClaw;
    public RigidbodyGrabClaw rightClaw;

    public HashSet<RigidbodyAttachableObject> leftAttachableObjectSet;
    public HashSet<RigidbodyAttachableObject> rightAttachableObjectSet;
    [Space(10)]
    public float clawRotationSpeed = 25f;
    public float CurrentRotationAngle { get; private set; } = 0f;
    [Space(10)]
    public string iconName;
    public RigidbodyAttachableObject AttachedObject { get; private set; }
    public const float minRotationAngle = -9f;
    public const float maxRotationAngle = 52f;
    private float rotationAngleOnAttach;
    private bool objectAttached = false;

    private float fixedSpeed;

    protected override void Awake()
    {
        base.Awake();
        fixedSpeed = clawRotationSpeed * Time.fixedDeltaTime;
        leftAttachableObjectSet = new HashSet<RigidbodyAttachableObject>();
        rightAttachableObjectSet = new HashSet<RigidbodyAttachableObject>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (Equipped && !objectAttached) TryToAttachObject();
    }

    private void TryToAttachObject()
    {
        foreach (RigidbodyAttachableObject attachableObject in leftAttachableObjectSet)
        {
            if (rightAttachableObjectSet.Contains(attachableObject))
            {
                AttachObject(attachableObject);
                break;
            }
        }
    }

    private void AttachObject(RigidbodyAttachableObject attachableObject)
    {
        leftAttachableObjectSet.Remove(attachableObject);
        rightAttachableObjectSet.Remove(attachableObject);

        AttachedObject = attachableObject;
        AttachedObject.LeaveHandler(transform);
        parentObject.AddObjectCenterOfMass(AttachedObject);
        rotationAngleOnAttach = CurrentRotationAngle;
        objectAttached = true;
    }

    private void FreeAttachedObject()
    {
        parentObject.RemoveObjectCenterOfMass(AttachedObject);
        AttachedObject.ReturnToHandler();
        AttachedObject = null;
        objectAttached = false;
    }

    private void RotateClaws()
    {
        CurrentRotationAngle = Mathf.Clamp(CurrentRotationAngle, minRotationAngle, maxRotationAngle);
        leftClaw.SetLocalRotation(CurrentRotationAngle);
        rightClaw.SetLocalRotation(CurrentRotationAngle);
    }

    protected override void OnEquip()
    {
        AccessoryIcon.instance?.SetIconByName(iconName);
    }

    protected override void OnUnequip()
    {
        if (objectAttached) FreeAttachedObject();
        AccessoryIcon.instance?.TurnOffIcon();
    }

    protected override void FirstAction()
    {
        if (!objectAttached)
        {
            CurrentRotationAngle += fixedSpeed;
            RotateClaws();
        }
    }

    protected override void SecondAction()
    {
        CurrentRotationAngle -= fixedSpeed;
        RotateClaws();
        if (objectAttached)
        {
            FreeAttachedObject();
        }
    }
}