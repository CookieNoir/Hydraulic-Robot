using UnityEngine;
using UnityEditor;

class SelectAllWithTag : EditorWindow
{
    string tagName = "ExampleTag";

    [MenuItem("Tools/Select All With Tag")]
    public static void ShowWindow()
    {
        GetWindow(typeof(SelectAllWithTag));
    }

    private void OnGUI()
    {
        tagName = EditorGUILayout.TextField("Tag Name", tagName);

        if (GUILayout.Button("Select All With Tag"))
        {
            GameObject[] gos = GameObject.FindGameObjectsWithTag(tagName);
            Selection.objects = gos;
        }
    }
}