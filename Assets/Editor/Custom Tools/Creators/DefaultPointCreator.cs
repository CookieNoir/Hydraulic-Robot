using UnityEngine;
using UnityEditor;

public class DefaultPointCreator : EditorWindow
{
    Transform transformCollection;
    Transform defaultPointHolder;

    [MenuItem("Tools/Creators/Default Point Creator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(DefaultPointCreator));
    }

    private void OnGUI()
    {
        transformCollection = EditorGUILayout.ObjectField("Transform Collection", transformCollection, typeof(Transform), true) as Transform;
        defaultPointHolder = EditorGUILayout.ObjectField("Default Point Holder", defaultPointHolder, typeof(Transform), true) as Transform;
        if (transformCollection && defaultPointHolder)
        {
            EditorGUILayout.Space(10);
            if (GUILayout.Button("Create Default Points"))
            {
                CreateDefaultPoints();
            }
        }
    }

    private void CreateDefaultPoints()
    {
        for (int i = 0; i < transformCollection.childCount; ++i)
        {
            Transform child = transformCollection.GetChild(i);
            if (child.gameObject.activeSelf)
            {
                GameObject newGameObject = new GameObject
                {
                    name = child.name + " Default Point",
                };
                newGameObject.transform.parent = defaultPointHolder;
                newGameObject.transform.position = child.position;
                newGameObject.transform.rotation = child.rotation;
            }
        }
    }
}
