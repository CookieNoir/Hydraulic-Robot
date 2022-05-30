using UnityEngine;
[RequireComponent(typeof(ArticulationBody))]
public class ArticulationBodyMovement : MonoBehaviour
{
    protected ArticulationBody articulationBody;
    [SerializeField] private float _startLinearSpeed;
    [SerializeField] private float _startAngularSpeed;
    protected float currentLinearSpeed;
    protected float currentAngularSpeed;

    private void Awake()
    {
        RecalculateSpeed(_startLinearSpeed, _startAngularSpeed);
    }

    private void RecalculateSpeed(float linearSpeed, float angularSpeed)
    {
        currentLinearSpeed = linearSpeed * Time.fixedDeltaTime;
        currentAngularSpeed = angularSpeed * Time.fixedDeltaTime;
    }

    private void Start()
    {
        articulationBody = GetComponent<ArticulationBody>();
    }

    public virtual void Move(Vector3 forceDirection, float forceMultiplier, Vector3 torqueDirection, float torqueMultiplier)
    {
        
    }
}