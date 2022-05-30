using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TransformsFromVector3 : EditorWindow
{
    public Transform parentTransform;
    public List<Vector3> vectors;
    public string pointName;
    private Vector2 scrollPosition;

    [MenuItem("Tools/Creators/Transforms From Vectors")]
    public static void ShowWindow()
    {
        GetWindow(typeof(TransformsFromVector3));
    }

    void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        parentTransform = EditorGUILayout.ObjectField("Parent", parentTransform, typeof(Transform), true) as Transform;
        pointName = EditorGUILayout.TextField("Point Name", pointName);
        GUILayout.Space(15);

        int newCount = Mathf.Max(0, EditorGUILayout.IntField("Vector3 List Size", vectors.Count));
        while (newCount < vectors.Count)
            vectors.RemoveAt(vectors.Count - 1);
        while (newCount > vectors.Count)
            vectors.Add(Vector3.zero);

        for (int i = 0; i < vectors.Count; i++)
        {
            vectors[i] = EditorGUILayout.Vector3Field("Element " + i, vectors[i]);
        }

        GUILayout.Space(20);
        if (GUILayout.Button("Create Transforms"))
        {
            CreateTransforms();
        }

        GUILayout.EndScrollView();
    }

    private void CreateTransforms()
    {
        for (int i = 0; i < vectors.Count; ++i)
        {
            GameObject newGameObject = new GameObject
            {
                name = pointName + " " + (i + 1)
            };
            newGameObject.transform.parent = parentTransform;
            newGameObject.transform.position = vectors[i];
        }
    }
}
