using UnityEngine;
using UnityEngine.InputSystem;
using System.Text.RegularExpressions;

[RequireComponent(typeof(ArticulationBody))]
public class ArticulationBodyXDriveModification : MonoBehaviour
{
    public bool independent = true;
    public float speed = 90f;
    [Space(10)]
    public KeyCode decreaseValueButton;
    public KeyCode increaseValueButton;

    //Input System
    private PlayerInput playerInput;
    private InputController.RobotArmActions armAction;
    private float inputValue;

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

        //Input system initialization
        armAction = (new InputController()).RobotArm;
        armAction.Enable();
        armAction.MoveJoint.AddCompositeBinding("Axis")
            .With("Positive", string.Format("<Keyboard>/{0}", increaseValueButton))
            .With("Negative", string.Format("<Keyboard>/{0}", decreaseValueButton))
            .With("MaxValue", "1")
            .With("MinVlaue", "-1");
    }

    private void Update() {
        if (independent && rotationAllowed) 
        {
            inputValue = armAction.MoveJoint.ReadValue<float>();

            if (inputValue > 0)
                OnIncreaseAction();
            if (inputValue < 0)
                OnDecreaseAction();
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

    private void DecreaseTarget() {
        MoveTo(articulationBody.jointPosition[0] * angleModifier - fixedSpeed);
    }

    private void IncreaseTarget() {
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