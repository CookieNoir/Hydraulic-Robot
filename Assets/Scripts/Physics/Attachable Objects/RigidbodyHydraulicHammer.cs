using UnityEngine;

public class RigidbodyHydraulicHammer : RigidbodyAccessory
{
    public GameObject hitPoints;
    [Space(10)]
    public string turnedOffIconName;
    public string turnedOnIconName;
    private bool canDestroy;

    protected override void Start()
    {
        SetState(false);
        base.Start();
    }

    private void ChangeStateAndIcon(bool state)
    {
        if (canDestroy != state)
        {
            SetState(state);
            SetIcon();
        }
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

    protected override void FirstAction()
    {
        ChangeStateAndIcon(true);
    }

    protected override void SecondAction()
    {
        ChangeStateAndIcon(false);
    }
}