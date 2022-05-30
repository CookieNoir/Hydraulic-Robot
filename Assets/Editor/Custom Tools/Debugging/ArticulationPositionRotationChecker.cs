using UnityEngine;
using UnityEditor;

public class ArticulationPositionRotationChecker : EditorWindow
{
    Vector3 parentAnchorPosition;
    Quaternion parentAnchorRotation;
    Vector3 anchorPosition;
    Quaternion anchorRotation;
    float value;
    [MenuItem("Tools/Debugging/Articulation Position and Rotation Checker")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ArticulationPositionRotationChecker));
    }

    private void OnGUI()
    {
        parentAnchorPosition = EditorGUILayout.Vector3Field("Parent Anchor Position", parentAnchorPosition);
        Vector4 v = EditorGUILayout.Vector4Field("Parent Anchor Rotation", new Vector4(parentAnchorRotation.x, parentAnchorRotation.y, parentAnchorRotation.z, parentAnchorRotation.w));
        parentAnchorRotation = new Quaternion(v.x, v.y, v.z, v.w);
        anchorPosition = EditorGUILayout.Vector3Field("Anchor Position", anchorPosition);
        Vector4 u = EditorGUILayout.Vector4Field("Anchor Rotation", new Vector4(anchorRotation.x, anchorRotation.y, anchorRotation.z, anchorRotation.w));
        anchorRotation = new Quaternion(u.x, u.y, u.z, u.w);

        value = EditorGUILayout.FloatField("Value", value);
        Vector3 modificationVector = Vector3.right * value;
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField((parentAnchorPosition + parentAnchorRotation * modificationVector).ToString());
        EditorGUILayout.LabelField((parentAnchorRotation * Quaternion.Euler(modificationVector)).eulerAngles.ToString());
    }
}
