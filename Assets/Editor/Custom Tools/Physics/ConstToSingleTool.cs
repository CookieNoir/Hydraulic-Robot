using UnityEngine;
using UnityEditor;

public class ConstToSingleTool : EditorWindow
{
    Transform parentTransform;

    [MenuItem("Tools/Physics/Const To Local Look At Tool")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ConstToSingleTool));
    }

    private void OnGUI()
    {
        parentTransform = EditorGUILayout.ObjectField("Parent", parentTransform, typeof(Transform), true) as Transform;

        if (GUILayout.Button("Change From Const To Single"))
        {
            SingleLookAt[] singleLookAts = parentTransform.GetComponentsInChildren<SingleLookAt>();
            for (int i = 0; i < singleLookAts.Length; ++i)
            {
                SingleLookAt singleLookAt = singleLookAts[i].gameObject.AddComponent<SingleLookAt>();
                singleLookAt.target = singleLookAts[i].target;
                singleLookAt.isOppositeDirection = singleLookAts[i].isOppositeDirection;
                DestroyImmediate(singleLookAts[i]);
            }
            SingleTriangulation[] singleTriangulations = parentTransform.GetComponentsInChildren<SingleTriangulation>();
            for (int i = 0; i < singleTriangulations.Length; ++i)
            {
                SingleTriangulation singleTriangulation = singleTriangulations[i].gameObject.AddComponent<SingleTriangulation>();
                singleTriangulation.point1 = singleTriangulations[i].point1;
                singleTriangulation.point2 = singleTriangulations[i].point2;
                DestroyImmediate(singleTriangulations[i]);
            }
        }

        if (GUILayout.Button("Change From Single To Const"))
        {
            SingleLookAt[] singleLookAts = parentTransform.GetComponentsInChildren<SingleLookAt>();
            for (int i = 0; i < singleLookAts.Length; ++i)
            {
                ConstLookAt constLookAt = singleLookAts[i].gameObject.AddComponent<ConstLookAt>();
                constLookAt.target = singleLookAts[i].target;
                constLookAt.isOppositeDirection = singleLookAts[i].isOppositeDirection;
                DestroyImmediate(singleLookAts[i]);
            }
            SingleTriangulation[] singleTriangulations = parentTransform.GetComponentsInChildren<SingleTriangulation>();
            for (int i = 0; i < singleTriangulations.Length; ++i)
            {
                ConstTriangulation constTriangulation = singleTriangulations[i].gameObject.AddComponent<ConstTriangulation>();
                constTriangulation.point1 = singleTriangulations[i].point1;
                constTriangulation.point2 = singleTriangulations[i].point2;
                DestroyImmediate(singleTriangulations[i]);
            }
        }
    }
}
