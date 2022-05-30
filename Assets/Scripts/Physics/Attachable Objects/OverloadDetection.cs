using System.Collections.Generic;
using UnityEngine;

public class OverloadDetection : ArticulationBodyCenterOfMass
{
    public bool IsFree { get; private set; }
    public float SpeedModifier { get; private set; }
    public float maxOverload;
    [Range(0f, 1f)] public float speedModifierSignificance = 0.2f;
    private float eps = 0.000001f;
    private List<CenterOfMass> addedObjects;

    protected override void Awake()
    {
        base.Awake();
        addedObjects = new List<CenterOfMass>();
        addedObjects.Add(this);
        IsFree = true;
        SpeedModifier = 1f;
    }

    private void RecalculateLocalCenterOfMassAndSpeed()
    {
        Vector3 sumVector = Vector3.zero;
        float newMass = 0;
        foreach (CenterOfMass cm in addedObjects)
        {
            sumVector += cm.transform.TransformPoint(cm.centerOfMass) * cm.mass;
            newMass += cm.mass;
        }
        articulationBody.centerOfMass = transform.InverseTransformPoint(sumVector / newMass);
        articulationBody.mass = newMass;

        float significantValue = speedModifierSignificance * SimpleFunctions.Smoothstep(Mathf.Clamp01((maxOverload + mass - newMass) / maxOverload));
        SpeedModifier = (1f - speedModifierSignificance) + significantValue;
        if (significantValue < eps)
        {
            IsFree = false;
            NotificationSystem.instance?.Notify(NotificationSystem.NotificationTypes.alert, "Перегруз");
        }
        else
        {
            IsFree = true;
        }
    }

    public void AddObjectCenterOfMass(CenterOfMass centerOfMass)
    {
        addedObjects.Add(centerOfMass);
        RecalculateLocalCenterOfMassAndSpeed();
    }

    public void RemoveObjectCenterOfMass(CenterOfMass centerOfMass)
    {
        addedObjects.Remove(centerOfMass);
        RecalculateLocalCenterOfMassAndSpeed();
    }
}
