using UnityEngine;
using UnityEditor;

public class ArticulationTargetShower : EditorWindow
{
    public Transform parentTransform;
    private ArticulationBody[] articulationBodies;
    private Vector2 scrollPosition;
    [MenuItem("Tools/Debugging/Articulation Body Target Shower")]

    public static void ShowWindow()
    {
        GetWindow(typeof(ArticulationTargetShower));
    }

    void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        parentTransform = EditorGUILayout.ObjectField("Parent", parentTransform, typeof(Transform), true) as Transform;

        GUILayout.Space(20);
        if (GUILayout.Button("Get Targets"))
        {
            articulationBodies = parentTransform.GetComponentsInChildren<ArticulationBody>();
        }

        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("Targets:");
        EditorGUILayout.Space(15);

        if (articulationBodies != null)
        {
            EditorGUILayout.BeginVertical();
            for (int i = 0; i < articulationBodies.Length; ++i)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(articulationBodies[i].name);
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField(articulationBodies[i].xDrive.target.ToString());
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }
        GUILayout.EndScrollView();
    }
}
