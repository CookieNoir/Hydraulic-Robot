using UnityEngine;

public class ArticulationBodyCenterOfMass : CenterOfMass
{
    public ArticulationBody articulationBody;

    protected virtual void Awake()
    {
        articulationBody.centerOfMass = centerOfMass;
        articulationBody.mass = mass;
    }
}
