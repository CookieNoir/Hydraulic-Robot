using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(CenterOfMass)), CanEditMultipleObjects]
public class CenterOfMassEditor : Editor
{
    CenterOfMass centerOfMass;
    private void OnEnable()
    {
        centerOfMass = (CenterOfMass)target;
    }
    private void OnSceneGUI()
    {
        Vector3 offset = centerOfMass.transform.InverseTransformPoint(Handles.PositionHandle(centerOfMass.transform.TransformPoint(centerOfMass.centerOfMass), Quaternion.identity));
        if (Vector3.Magnitude(offset-centerOfMass.centerOfMass) > 0.00001f)
            centerOfMass.centerOfMass = offset;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}