using UnityEngine;
using UnityEditor;

public class ScalerTool : EditorWindow
{
    Transform transform;
    BoxCollider boxCollider;
    CapsuleCollider capsuleCollider;
    SphereCollider sphereCollider;
    CenterOfMass centerOfMass;
    float scaleModifier;
    bool nullReferenceAfterModifying;

    [MenuItem("Tools/Scaler Tool")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ScalerTool));
    }

    private void OnGUI()
    {
        nullReferenceAfterModifying = EditorGUILayout.Toggle("Remove Reference After Modifying", nullReferenceAfterModifying);
        scaleModifier = EditorGUILayout.FloatField("Scale Modifier", scaleModifier);

        EditorGUILayout.Space(15);
        transform = EditorGUILayout.ObjectField("Transform", transform, typeof(Transform), true) as Transform;
        if (GUILayout.Button("Apply scale to Transform local position"))
        {
            transform.localPosition *= scaleModifier;
            if (nullReferenceAfterModifying) transform = null;
        }

        EditorGUILayout.Space(15);
        boxCollider = EditorGUILayout.ObjectField("Box Collider", boxCollider, typeof(BoxCollider), true) as BoxCollider;
        if (GUILayout.Button("Apply scale to Box Collider center and size"))
        {
            boxCollider.center *= scaleModifier;
            boxCollider.size *= scaleModifier;
            boxCollider = null;
            if (nullReferenceAfterModifying) boxCollider = null;
        }

        EditorGUILayout.Space(15);
        sphereCollider = EditorGUILayout.ObjectField("Sphere Collider", sphereCollider, typeof(SphereCollider), true) as SphereCollider;
        if (GUILayout.Button("Apply scale to Sphere Collider center and radius"))
        {
            sphereCollider.center *= scaleModifier;
            sphereCollider.radius *= scaleModifier;
            if (nullReferenceAfterModifying) sphereCollider = null;
        }

        EditorGUILayout.Space(15);
        capsuleCollider = EditorGUILayout.ObjectField("Capsule Collider", capsuleCollider, typeof(CapsuleCollider), true) as CapsuleCollider;
        if (GUILayout.Button("Apply scale to Capsule Collider center, height and radius"))
        {
            capsuleCollider.center *= scaleModifier;
            capsuleCollider.height *= scaleModifier;
            capsuleCollider.radius *= scaleModifier;
            if (nullReferenceAfterModifying) capsuleCollider = null;
        }

        EditorGUILayout.Space(15);
        centerOfMass = EditorGUILayout.ObjectField("Center Of Mass", centerOfMass, typeof(CenterOfMass), true) as CenterOfMass;
        if (GUILayout.Button("Apply scale to Center Of Mass local position"))
        {
            centerOfMass.centerOfMass *= scaleModifier;
            if (nullReferenceAfterModifying) centerOfMass = null;
        }
    }
}