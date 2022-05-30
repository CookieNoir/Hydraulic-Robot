using System.Collections;
using UnityEngine;

public class RigidbodyAccessory : RigidbodyAttachableObject
{
    protected bool Equipped { get; private set; }
    protected AccessoryJoinPoint parentObject;
    private IEnumerator setFixedDistance;

    protected override void Awake()
    {
        base.Awake();
        setFixedDistance = SetFixedDistanceSmoothly();
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

    protected virtual void FixedUpdate()
    {
        if (Equipped)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                FirstAction();
            }
            if (Input.GetKey(KeyCode.Mouse1))
            {
                SecondAction();
            }
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