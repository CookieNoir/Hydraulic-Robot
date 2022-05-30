using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMovement : ArticulationBodyMovement
{
    public override void Move(Vector3 forceDirection, float forceMultiplier, Vector3 torqueDirection, float torqueMultiplier)
    {
        articulationBody.AddForce(forceDirection * currentLinearSpeed * forceMultiplier);
        articulationBody.AddTorque(torqueDirection * currentAngularSpeed * torqueMultiplier);
    }
}