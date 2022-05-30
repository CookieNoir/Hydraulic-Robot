using UnityEngine;
using UnityEditor;

public class DistanceChecker : EditorWindow
{
    Transform firstObject;
    Transform secondObject;

    [MenuItem("Tools/Debugging/Distance Checker")]
    public static void ShowWindow()
    {
        GetWindow(typeof(DistanceChecker));
    }

    private void OnGUI()
    {
        firstObject = EditorGUILayout.ObjectField("First Transform", firstObject, typeof(Transform), true) as Transform;
        secondObject = EditorGUILayout.ObjectField("Second Transform", secondObject, typeof(Transform), true) as Transform;
        if (firstObject && secondObject)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Distance is " + Vector3.Magnitude(firstObject.position - secondObject.position).ToString());
        }
    }
}