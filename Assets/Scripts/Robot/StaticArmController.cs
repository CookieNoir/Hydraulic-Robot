using System.Collections.Generic;
using UnityEngine;

public class StaticArmController : MonoBehaviour
{
    public List<XDriveSimulation> rotations;
    [Space(10)]
    public List<SingleTriangulation> triangulations;
    public List<SingleLookAt> lookAts;
    private const int lookAtsIterations = 5;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetState(RobotState newState)
    {
        transform.position = newState.statePoint.position;
        transform.rotation = newState.statePoint.rotation;

        for (int i = 0; i < rotations.Count; ++i)
        {
            rotations[i].SetTarget(newState.armStatesValues[i].value);
        }

        NormalizeLookAts();
    }

    private void NormalizeLookAts()
    {
        for (int iteration = 0; iteration < lookAtsIterations; ++iteration)
        {
            for (int i = 0; i < triangulations.Count; ++i)
            {
                triangulations[i].Triangulate();
            }
            for (int i = 0; i < lookAts.Count; ++i)
            {
                lookAts[i].LookAt();
            }
        }
    }
}
