using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ArticulationBody))]
public class ArticulationBodyXDriveModification : MonoBehaviour
{
    public bool independent = true;
    public float speed = 90f;
    [Space(10)]
    [Header("Input properties")]
    public InputActionReference action;
    //Input System 
    private InputAction inAction;
    private float inputValue;
    private InputActionSetupExtensions.CompositeSyntax binding;

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

        if (action is not null) 
        {
            inAction = action;

            // Пример программного добавления привязки.
            //inAction.AddCompositeBinding("Axis")
            //    .With("Positive", "<Keyboard>/" + increaseValueButton.ToString())
            //    .With("Negative", "<Keyboard>/" + decreaseValueButton.ToString())
            //    .With("MaxValue", "1")
            //    .With("MinVlaue", "-1");

            if (!inAction.enabled) inAction.Enable();
        }
        else Debug.Log(string.Format("No action specified for game object {0}.", gameObject.name));
    }

    void OnDisable() {
        if (inAction.enabled) inAction.Disable();
    }

    void OnEnable() {
        if (!inAction.enabled) inAction.Enable();
    }

    private void Update() {
        if (independent && rotationAllowed)
        {
            inputValue = inAction.ReadValue<float>();
        }
        else inputValue = 0f;
    }

    private void FixedUpdate()
    {
        if (inputValue > 0) OnIncreaseAction();
        if (inputValue < 0) OnDecreaseAction();
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