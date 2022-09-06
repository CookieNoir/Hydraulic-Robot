using UnityEngine;
using UnityEngine.InputSystem;

public class RigidbodyHydraulicHammer : RigidbodyAccessory
{
    public GameObject hitPoints;
    [Space(10)]
    public string turnedOffIconName;
    public string turnedOnIconName;
    public InputActionReference action1;
    private InputAction inAction1;
    private bool canDestroy;

    protected override void Awake()
    {
        base.Awake();

        if (action1 is not null)
        {
            inAction1 = action1;
            inAction1.performed += FirstAction;
            if (!inAction1.enabled) inAction1.Enable();
        }
        else Debug.Log(string.Format("No action specified for game object {0}.", gameObject.name));
    }

    protected override void Start()
    {
        SetState(false);
        base.Start();
    }

    private void ChangeStateAndIcon()
    {
        SetState(!canDestroy);
        SetIcon();
    }

    private void SetState(bool value)
    {
        canDestroy = value;
        hitPoints.SetActive(canDestroy);
    }

    private void SetIcon()
    {
        AccessoryIcon.instance?.SetIconByName(canDestroy ? turnedOnIconName : turnedOffIconName);
    }

    protected override void OnEquip()
    {
        SetIcon();
    }

    protected override void OnUnequip()
    {
        SetState(false);
        AccessoryIcon.instance?.TurnOffIcon();
    }

    protected override void FirstAction(InputAction.CallbackContext ctx)
    {
        if (Equipped)
        {
            ChangeStateAndIcon();
        }
    }
}