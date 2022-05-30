using System;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyGrabTrigger : MonoBehaviour
{
    public RigidbodyGrab grab;
    public bool isLeft;
    private HashSet<RigidbodyAttachableObject> attachableObjects;

    private const string childTag = "Attachable Object Collider";

    private void Start()
    {
        attachableObjects = isLeft ? grab.leftAttachableObjectSet : grab.rightAttachableObjectSet;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnAttachableObjectFoundAction(attachableObjects.Add, other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnAttachableObjectFoundAction(attachableObjects.Remove, other);
    }

    private void OnAttachableObjectFoundAction(Func<RigidbodyAttachableObject, bool> action, Collider other)
    {
        if (!(other.gameObject.CompareTag("Player")))
        {
            RigidbodyAttachableObject attachableObject = other.GetComponent<RigidbodyAttachableObject>();
            if (attachableObject)
            {
                action(attachableObject);
            }
            else
            {
                if (other.gameObject.CompareTag(childTag))
                {
                    attachableObject = other.transform.parent.GetComponent<RigidbodyAttachableObject>();
                    action(attachableObject);
                }
            }
        }
    }
}
