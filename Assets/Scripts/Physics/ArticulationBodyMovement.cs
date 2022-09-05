using UnityEngine;
[RequireComponent(typeof(ArticulationBody))]
public class ArticulationBodyMovement : MonoBehaviour
{
    protected ArticulationBody articulationBody;
    [SerializeField] protected float forceMultiplier;

    private void Start()
    {
        articulationBody = GetComponent<ArticulationBody>();
    }

    public virtual void Move(float axisLeft, float axisRight, float dynamicForceMultiplier) // Axis values should be in range [-1, 1]
    {
        
    }
}