using UnityEngine;
using UnityEditor;
using System;

public class CopyComponentsTool : EditorWindow
{
    GameObject firstObject;
    GameObject secondObject;

    [MenuItem("Tools/Copy Components Tool")]
    public static void ShowWindow()
    {
        GetWindow(typeof(CopyComponentsTool));
    }

    private void OnGUI()
    {
        firstObject = EditorGUILayout.ObjectField("From GameObject", firstObject, typeof(GameObject), true) as GameObject;
        secondObject = EditorGUILayout.ObjectField("To GameObject", secondObject, typeof(GameObject), true) as GameObject;
        if (firstObject && secondObject)
        {
            EditorGUILayout.Space(10);
            if (GUILayout.Button("Copy all components (excluding Transform)"))
            {
                CopyComponents();
            }
        }
    }

    private void CopyComponents()
    {
        foreach (Component component in firstObject.GetComponents<Component>())
        {
            Type componentType = component.GetType();
            if (componentType != typeof(Transform))
            {
                UnityEditorInternal.ComponentUtility.CopyComponent(component);
                UnityEditorInternal.ComponentUtility.PasteComponentAsNew(secondObject);
                Debug.Log("Copied " + componentType + " component from " + firstObject.name + " to " + secondObject.name);
            }
        }
    }
}