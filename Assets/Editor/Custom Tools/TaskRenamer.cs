using UnityEngine;
using UnityEditor;

public class TaskRenamer : EditorWindow
{
    Transform parentTransform;
    string textBefore;
    bool usePrefix;
    bool useBody;
    bool useSuffix;
    string newPrefix;
    Color prefixColor;
    string coloredPrefix;

    [MenuItem("Tools/Task Renamer")]
    public static void ShowWindow()
    {
        GetWindow(typeof(TaskRenamer));
    }

    private void OnGUI()
    {
        parentTransform = EditorGUILayout.ObjectField("Parent", parentTransform, typeof(Transform), true) as Transform;

        EditorGUILayout.Space(10);
        textBefore = EditorGUILayout.TextField("Text Before Prefix", textBefore);
        usePrefix = EditorGUILayout.Toggle("Use Prefix", usePrefix);
        useBody = EditorGUILayout.Toggle("Use Body", useBody);
        useSuffix = EditorGUILayout.Toggle("Use Suffix", useSuffix);

        if (GUILayout.Button("Name All Child Tasks"))
        {
            Task[] tasks = parentTransform.GetComponentsInChildren<Task>();
            string resultString;
            foreach (Task task in tasks)
            {
                resultString = textBefore;
                if (usePrefix) resultString += task.taskNamePrefix;
                if (useBody) resultString += task.taskNameBody;
                if (useSuffix) resultString += task.taskNameSuffix;
                task.name = resultString;
            }
        }

        EditorGUILayout.Space(20);
        newPrefix = EditorGUILayout.TextField("New Prefix", newPrefix);
        prefixColor = EditorGUILayout.ColorField("Prefix Color", prefixColor);
        coloredPrefix = "<color=#" + ColorUtility.ToHtmlStringRGBA(prefixColor) + ">" + newPrefix + "</color>.";
        EditorGUILayout.LabelField("New colored prefix is:   " + coloredPrefix);

        if (GUILayout.Button("Set Colored Prefix to All Child Tasks"))
        {
            Task[] tasks = parentTransform.GetComponentsInChildren<Task>();
            foreach (Task task in tasks)
            {
                task.taskNamePrefix = coloredPrefix;
            }
        }
    }
}
