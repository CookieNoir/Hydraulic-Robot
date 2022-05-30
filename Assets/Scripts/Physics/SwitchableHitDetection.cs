using UnityEngine;

public class SwitchableHitDetection : HitDetection
{
    public bool trackCollisions = true;

    protected override void OnCollisionEnter(Collision collision)
    {
        if (trackCollisions) OnCollisionEnterAction();
    }
}
