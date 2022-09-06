using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public InputActionReference action1;
    public InputActionReference action2;
    private InputAction inAction1;
    private InputAction inAction2;
    private float _inputValue1;
    private float _inputValue2;
    private float rotationAngleOnAttach;
    private bool objectAttached = false;

    private float fixedSpeed;

    protected override void Awake()
    {
        base.Awake();
        fixedSpeed = clawRotationSpeed * Time.fixedDeltaTime;
        leftAttachableObjectSet = new HashSet<RigidbodyAttachableObject>();
        rightAttachableObjectSet = new HashSet<RigidbodyAttachableObject>();

        if (action1 is not null)
        {
            inAction1 = action1;
            if (!inAction1.enabled) inAction1.Enable();
        }
        else Debug.Log(string.Format("No action specified for game object {0}.", gameObject.name));
        if (action2 is not null)
        {
            inAction2 = action2;
            if (!inAction2.enabled) inAction2.Enable();
        }
        else Debug.Log(string.Format("No action specified for game object {0}.", gameObject.name));
    }

    private void Update()
    {
        _inputValue1 = inAction1.ReadValue<float>();
        _inputValue2 = inAction2.ReadValue<float>();
    }

    private void FixedUpdate()
    {
        if (Equipped)
        {
            if (!objectAttached) TryToAttachObject();
            if (_inputValue1 != 0f) FirstAction(new InputAction.CallbackContext());
            if (_inputValue2 != 0f) SecondAction(new InputAction.CallbackContext());
        }
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

    protected override void FirstAction(InputAction.CallbackContext ctx)
    {
        if (!objectAttached)
        {
            CurrentRotationAngle += fixedSpeed;
            RotateClaws();
        }
    }

    protected override void SecondAction(InputAction.CallbackContext ctx)
    {
        CurrentRotationAngle -= fixedSpeed;
        RotateClaws();
        if (objectAttached)
        {
            FreeAttachedObject();
        }
    }
}