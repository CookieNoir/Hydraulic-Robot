using UnityEngine;

public class ArticulationBodyChainedXDriveModification : ArticulationBodyXDriveModification
{
    [System.Serializable]
    public class ModificationMultiplierPairs
    {
        public ArticulationBodyXDriveModification modification;
        public float multiplier = 1f;
    }

    public ModificationMultiplierPairs[] chainedModifications;

    protected override void OnDecreaseAction()
    {
        base.OnDecreaseAction();
        SetTargets();
    }

    protected override void OnIncreaseAction()
    {
        base.OnIncreaseAction();
        SetTargets();
    }

    private void SetTargets()
    {
        for (int i = 0; i < chainedModifications.Length; ++i)
        {
            chainedModifications[i].modification.MoveTo(expectedTarget * chainedModifications[i].multiplier);
        }
    }
}