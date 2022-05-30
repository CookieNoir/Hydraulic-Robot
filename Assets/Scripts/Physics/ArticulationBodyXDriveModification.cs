using UnityEngine;
[RequireComponent(typeof(ArticulationBody))]
public class ArticulationBodyXDriveModification : MonoBehaviour
{
    public bool independent = true;
    public float speed = 90f;
    [Space(10)]
    public KeyCode decreaseValueButton;
    public KeyCode increaseValueButton;

    private ArticulationBody articulationBody;
    private ArticulationDrive xDrive;
    private float fixedSpeed;
    private float angleModifier;
    private bool rotationAllowed = true;
    private bool inRadians;
    protected float expectedTarget = 0f;

    protected virtual void Awake()
    {
        articulationBody = GetComponent<ArticulationBody>();
        fixedSpeed = speed * Time.fixedDeltaTime;
        inRadians = articulationBody.jointType != ArticulationJointType.PrismaticJoint;
        angleModifier = inRadians ? Mathf.Rad2Deg : 1f;
    }

    private void FixedUpdate()
    {
        if (independent && rotationAllowed)
        {
            if (Input.GetKey(decreaseValueButton))
            {
                OnDecreaseAction();
            }

            if (Input.GetKey(increaseValueButton))
            {
                OnIncreaseAction();
            }
        }
    }

    protected virtual void OnDecreaseAction()
    {
        DecreaseTarget();
    }

    protected virtual void OnIncreaseAction()
    {
        IncreaseTarget();
    }

    private void DecreaseTarget()
    {
        MoveTo(articulationBody.jointPosition[0] * angleModifier - fixedSpeed);
    }

    private void IncreaseTarget()
    {
        MoveTo(articulationBody.jointPosition[0] * angleModifier + fixedSpeed);
    }

    private void SetZeroVelocity()
    {
        articulationBody.velocity = Vector3.zero;
        articulationBody.angularVelocity = Vector3.zero;
    }

    public void MoveTo(float value)
    {
        expectedTarget = value;
        xDrive = articulationBody.xDrive;
        xDrive.target = value;
        articulationBody.xDrive = xDrive;
    }

    public void AllowRotation(bool value)
    {
        rotationAllowed = value;
    }

    public float GetXDriveTarget()
    {
        return articulationBody.xDrive.target;
    }
}