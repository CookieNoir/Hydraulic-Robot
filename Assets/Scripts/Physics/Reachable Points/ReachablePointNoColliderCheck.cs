using UnityEngine;

public class ReachablePointNoColliderCheck : ReachablePoint
// Используется в уровне с описанием фигуры стрелой
{
    public override void ResetReached()
    {
        objectsAtPoint = 0;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        Transform otherTransform = other.transform;
        if (CheckForMatches(ref otherTransform))
        {
            objectsAtPoint++;
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
    }
}
