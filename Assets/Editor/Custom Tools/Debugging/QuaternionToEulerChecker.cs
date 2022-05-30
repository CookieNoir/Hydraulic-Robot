using UnityEngine;
using UnityEditor;

public class QuaternionToEulerChecker : EditorWindow
{
    Quaternion quaternion;
    Vector3 eulerRotation;
    [MenuItem("Tools/Debugging/Quaternion To Euler Angles Checker")]
    public static void ShowWindow()
    {
        GetWindow(typeof(QuaternionToEulerChecker));
    }

    private void OnGUI()
    {
        Vector4 v = EditorGUILayout.Vector4Field("Quaternion", new Vector4(quaternion.x, quaternion.y, quaternion.z, quaternion.w));
        quaternion = new Quaternion(v.x, v.y, v.z, v.w);

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Quaternion as Euler Angles");
        EditorGUILayout.LabelField(quaternion.eulerAngles.ToString());

        EditorGUILayout.Space(10);
        eulerRotation = EditorGUILayout.Vector3Field("Rotation Euler Vector", eulerRotation);

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Multiplication of 2 quaternions");
        EditorGUILayout.LabelField((quaternion * Quaternion.Euler(eulerRotation)).ToString());
        EditorGUILayout.LabelField((quaternion * Quaternion.Euler(eulerRotation)).eulerAngles.ToString());
    }
}
