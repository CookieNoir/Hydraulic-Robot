using UnityEngine;
using UnityEditor;

public class QuaternionVectorMultiplicationChecker : EditorWindow
{
    Vector3 vector3;
    Vector3 rotationEulerVector;

    [MenuItem("Tools/Debugging/Vector Rotation Checker")]
    public static void ShowWindow()
    {
        GetWindow(typeof(QuaternionVectorMultiplicationChecker));
    }

    private void OnGUI()
    {
        vector3 = EditorGUILayout.Vector3Field("Vector3", vector3);
        rotationEulerVector = EditorGUILayout.Vector3Field("Rotation Euler Vector", rotationEulerVector);

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField((Quaternion.Euler(rotationEulerVector) * vector3).ToString());
    }
}
