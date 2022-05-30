using UnityEngine;

public class DestroyingArea : ReachablePoint
{
    private int requiredObjectsAmount;
    private RigidbodyGrab grab;

    public void SetRequiredObjectsAmount(int value) => requiredObjectsAmount = value;
    public void SetGrab(RigidbodyGrab grab) => this.grab = grab;

    public bool IsEnoughObjects()
    {
        if (objectsAtPoint >= requiredObjectsAmount)
            return true;
        else
            return false;
    }

    public override void ResetReached()
    {
        objectsAtPoint = 0;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        Transform otherTransform = other.transform;
        if (CheckForMatches(ref otherTransform))
        {
            RigidbodyAttachableObject ao = otherTransform.GetComponent<RigidbodyAttachableObject>();
            if (ao)
            {
                if (grab.AttachedObject != ao)
                {
                    objectsAtPoint++;
                    Destroy(ao.gameObject);
                }
            }
        }
    }

    protected override void OnTriggerExit(Collider other) { }
}
