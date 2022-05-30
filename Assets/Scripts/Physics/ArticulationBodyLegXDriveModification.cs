using UnityEngine;
[RequireComponent(typeof(ArticulationBody))]
public class ArticulationBodyLegXDriveModification : ArticulationBodyXDriveModification
{
    public ArticulationBodyXDriveModification[] restrictingModifications;
    [Min(0f)] public float targetDeviation = 1f;

    private bool lowerRule()
    {
        float lowerTarget = restrictingModifications[0].GetXDriveTarget();
        for (int i = 1; i < restrictingModifications.Length; ++i)
        {
            float newTarget = restrictingModifications[i].GetXDriveTarget();
            if (newTarget < lowerTarget) lowerTarget = newTarget;
        }
        return GetXDriveTarget() < lowerTarget + targetDeviation;
    }

    protected override void OnIncreaseAction()
    {
        if (lowerRule())
        {
            base.OnIncreaseAction();
        }
    }
}