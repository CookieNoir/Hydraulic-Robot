using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(GeneralCenterOfMass)), CanEditMultipleObjects]
public class GeneralCenterOfMassEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GeneralCenterOfMass generalCenterOfMass = (GeneralCenterOfMass)target;
        if (GUILayout.Button("Fill"))
        {
            generalCenterOfMass.CalculateCenterOfMass();
        }
    }
}