using UnityEngine;
using UnityEditor;

public class SetTagToAllGameObjectsInTree : EditorWindow
{
    string tag;
    Transform root;

    [MenuItem("Tools/GameObject Tree Tag Setter")]
    public static void ShowWindow()
    {
        GetWindow(typeof(SetTagToAllGameObjectsInTree));
    }

    private void OnGUI()
    {
        tag = EditorGUILayout.TextField("Tag", tag);
        root = EditorGUILayout.ObjectField("Root", root, typeof(Transform), true) as Transform;

        EditorGUILayout.Space(10);
        if (GUILayout.Button("Set tag"))
        {
            root.tag = tag;
            foreach (Transform t in root.GetComponentsInChildren<Transform>())
            {
                t.tag = tag;
            }
        }
    }
}
