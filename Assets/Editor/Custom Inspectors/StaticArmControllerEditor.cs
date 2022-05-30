using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StaticArmController))]
public class StaticArmControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Set Fields From Children"))
        {
            StaticArmController staticArmController = target as StaticArmController;

            staticArmController.rotations = new List<XDriveSimulation>(staticArmController.GetComponentsInChildren<XDriveSimulation>());
            staticArmController.triangulations = new List<SingleTriangulation>(staticArmController.GetComponentsInChildren<SingleTriangulation>());
            staticArmController.lookAts = new List<SingleLookAt>(staticArmController.GetComponentsInChildren<SingleLookAt>());
        }
    }
}
