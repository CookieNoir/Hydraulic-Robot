using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RigidbodyAccessory : RigidbodyAttachableObject
{
    public InputActionReference action1;
    public InputActionReference action2;
    private InputAction inAction1;
    private InputAction inAction2;
    private float inputValue1;
    private float inputValue2;
    protected bool Equipped { get; private set; }
    protected AccessoryJoinPoint parentObject;
    private IEnumerator setFixedDistance;

    protected override void Awake()
    {
        base.Awake();
        setFixedDistance = SetFixedDistanceSmoothly();
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

    protected virtual void Start()
    {
        rigidbody.gameObject.SetActive(false);
    }

    public void Equip(AccessoryJoinPoint newTarget, bool smooth = true)
    {
        parentObject = newTarget;
        LeaveHandler(parentObject.transform);

        if (smooth)
        {
            StopCoroutine(setFixedDistance);
            setFixedDistance = SetFixedDistanceSmoothly();
            StartCoroutine(setFixedDistance);
        }
        else
        {
            StopCoroutine(setFixedDistance);
            SetFixedDistanceInstantly();
        }
    }

    private void Update()
    {
        inputValue1 = Equipped ? inAction1.ReadValue<float>() : 0f;
        inputValue2 = Equipped ? inAction2.ReadValue<float>() : 0f;
    }

    protected virtual void FixedUpdate()
    {
        if (inputValue1 != 0f)
        {
            FirstAction();
        }
        if (inputValue2 != 0f)
        {
            SecondAction();
        }
    }

    protected virtual void FirstAction()
    {
    }

    protected virtual void SecondAction()
    {
    }

    protected virtual void OnEquip()
    {
    }

    public void Unequip()
    {
        parentObject.RemoveObjectCenterOfMass(this);
        OnUnequip();
        ReturnToHandler();
        parentObject = null;
        Equipped = false;
    }

    protected virtual void OnUnequip()
    {
    }

    private IEnumerator SetFixedDistanceSmoothly()
    {
        Vector3 startPosition = transform.localPosition;
        Quaternion startRotation = transform.localRotation;
        Quaternion endRotation = Quaternion.Euler(0f, 0f, startRotation.eulerAngles.z);
        float fixedTimeStep = Time.fixedDeltaTime;
        float sqrtf;
        for (float f = fixedTimeStep; f < 0.5f; f += fixedTimeStep)
        {
            yield return new WaitForFixedUpdate();
            sqrtf = Mathf.Sqrt(f + f);
            transform.localPosition = Vector3.Lerp(startPosition, Vector3.zero, sqrtf);
            transform.localRotation = Quaternion.Lerp(startRotation, endRotation, sqrtf);
        }
        SetFixedDistanceInstantly();
    }

    private void SetFixedDistanceInstantly()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0f, 0f, transform.localRotation.eulerAngles.z);
        parentObject.AddObjectCenterOfMass(this);
        OnEquip();
        Equipped = true;
    }
}