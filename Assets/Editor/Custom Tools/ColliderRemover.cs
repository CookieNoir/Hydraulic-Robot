using UnityEngine;
using UnityEditor;

public class ColliderRemover : EditorWindow
{
    public Transform parentTransform;

    [MenuItem("Tools/Child Collider Remover")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ColliderRemover));
    }

    void OnGUI()
    {
        GUILayout.Space(15);
        parentTransform = EditorGUILayout.ObjectField("Parent", parentTransform, typeof(Transform), true) as Transform;
        GUILayout.Space(20);
        if (GUILayout.Button("Remove All Child Colliders"))
        {
            RemoveColliders();
        }
    }

    private void RemoveColliders()
    {
        foreach (Transform child in parentTransform)
        {
            DestroyImmediate(child.GetComponent<Collider>());
        }
    }
}
