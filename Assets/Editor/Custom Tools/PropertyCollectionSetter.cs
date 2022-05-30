using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PropertyCollectionSetter : EditorWindow
{
    Transform parentTransform;
    MonoBehaviour targetMonoBehaviour;
    int index = 0;

    [MenuItem("Tools/Property Collection Setter")]
    public static void ShowWindow()
    {
        GetWindow(typeof(PropertyCollectionSetter));
    }

    private void OnGUI()
    {
        parentTransform = EditorGUILayout.ObjectField("Parent Transform", parentTransform, typeof(Transform), true) as Transform;
        targetMonoBehaviour = EditorGUILayout.ObjectField("Target Mono Behaviour", targetMonoBehaviour, typeof(MonoBehaviour), true) as MonoBehaviour;

        if (targetMonoBehaviour)
        {
            FieldInfo[] fields = targetMonoBehaviour.GetType().GetFields();
            List<FieldInfo> fieldsList = new List<FieldInfo>();
            List<string> namesList = new List<string>();
            for (int i = 0; i < fields.Length; ++i)
            {
                if (fields[i].FieldType.IsArray || (!fields[i].FieldType.Equals(typeof(string)) && fields[i].FieldType.GetInterface(typeof(IEnumerable<>).FullName) != null))
                {
                    fieldsList.Add(fields[i]);
                    namesList.Add(fields[i].Name);
                }
            }
            FieldInfo[] popupFields = fieldsList.ToArray();
            string[] popupNames = namesList.ToArray();

            index = EditorGUILayout.Popup("Collections", index, popupNames);
            FieldInfo fieldInfo = popupFields[index];

            if (parentTransform)
            {
                EditorGUILayout.Space(10);
                if (GUILayout.Button("Set Collection Using Transform"))
                {
                    SetCollection(fieldInfo);
                }
            }
        }
    }

    private void SetCollection(FieldInfo fieldInfo)
    {
        Type fieldType = fieldInfo.FieldType;
        bool isArray = fieldType.IsArray;
        Type argumentType = isArray ? fieldType.GetElementType() : fieldType.GetGenericArguments()[0];

        var listType = typeof(List<>).MakeGenericType(argumentType);
        var list = (IList)Activator.CreateInstance(listType);

        for (int i = 0; i < parentTransform.childCount; ++i)
        {
            Transform child = parentTransform.GetChild(i);
            if (child.gameObject.activeSelf)
            {
                object component = parentTransform.GetChild(i).GetComponentInChildren(argumentType);
                if (component != null) list.Add(component);
            }
        }
        if (isArray)
        {
            object array = Array.CreateInstance(argumentType, list.Count);
            list.CopyTo((Array)array, 0);
            fieldInfo.SetValue(targetMonoBehaviour, array);
        }
        else fieldInfo.SetValue(targetMonoBehaviour, list);
    }
}