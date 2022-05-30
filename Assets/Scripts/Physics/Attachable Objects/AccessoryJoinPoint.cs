using System.Collections.Generic;
using UnityEngine;

public class AccessoryJoinPoint : OverloadDetection
{
    [Space(10, order = 0), Header("Ќавесное оборудование", order = 1)]
    public Camera joinCamera;
    public float cameraTurnOnDistance; // ƒистанци€, при которой включаетс€ камера дл€ отслеживани€ соединени€
    public float equipDistance; // ƒистанци€, при которой присоединение оборудовани€ возможно
    public bool Selected { get; private set; } = false;
    public bool Equippable { get; private set; } = false;
    public bool Equipped { get; private set; } = false;
    public KeyCode equipKeyCode;

    private HashSet<RigidbodyAccessory> accessories;
    private RigidbodyAccessory accessory;

    private float distance;

    public void SetAccessory(RigidbodyAccessory newAccessory)
    {
        accessory = newAccessory;
        EquipAccessory(false);
    }

    private void EquipAccessory(bool smooth = true)
    {
        accessory.Equip(this, smooth);
        Equipped = true;
        NotificationSystem.instance?.Notify(NotificationSystem.NotificationTypes.message, "ѕрисоединено оборудование \"" + accessory.name + "\"");
    }

    public void UnequipAccessory()
    {
        if (Equipped)
        {
            NotificationSystem.instance?.Notify(NotificationSystem.NotificationTypes.message, "ќборудование \"" + accessory.name + "\" сн€то");
            accessory.Unequip();
            Equipped = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Accessory"))
        {
            RigidbodyAccessory rigidbodyAccessory = other.GetComponent<RigidbodyAccessory>();
            if (rigidbodyAccessory)
            {
                accessories.Add(rigidbodyAccessory);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Accessory"))
        {
            RigidbodyAccessory rigidbodyAccessory = other.GetComponent<RigidbodyAccessory>();
            if (rigidbodyAccessory)
            {
                accessories.Remove(rigidbodyAccessory);
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        Selected = false;
        Equipped = false;
        accessory = null;
        accessories = new HashSet<RigidbodyAccessory>();
    }

    private void Start()
    {
        if (JoinCamera.instance) NotificationSystem.instance?.ChangeScale(JoinCamera.instance.IsEnabled());
    }

    private void Update()
    {
        if (!Equipped)
            if (!Selected)
            {
                if (accessories.Count > 0)
                {
                    foreach (RigidbodyAccessory ac in accessories)
                    {
                        float dist = Vector3.Magnitude(transform.position - ac.transform.position);
                        if (dist < cameraTurnOnDistance)
                        {
                            accessory = ac;
                            JoinCamera.instance?.ChangeActivity(true);
                            JoinCamera.instance?.SetTextValue(dist.ToString("F8") + " m");
                            Selected = true;
                            break;
                        }
                    }
                }
                else
                {
                    if (JoinCamera.instance && JoinCamera.instance.IsEnabled())
                    {
                        JoinCamera.instance.ChangeActivity(false);
                    }
                }
            }
            else
            {
                distance = Vector3.Magnitude(transform.position - accessory.transform.position);
                if (distance >= cameraTurnOnDistance)
                {
                    accessory = null;
                    JoinCamera.instance?.ChangeActivity(false);
                    Selected = false;
                }
                else
                {
                    JoinCamera.instance?.SetTextValue(distance.ToString("F8") + " m");
                    Equippable = distance < equipDistance;
                    JoinCamera.instance?.ChangeReadiness(Equippable);

                    if (Input.GetKeyDown(equipKeyCode))
                    {
                        if (Equippable)
                        {
                            EquipAccessory();
                        }
                        else
                        {
                            NotificationSystem.instance?.Notify(NotificationSystem.NotificationTypes.warning, "ƒл€ присоединени€ оборудовани€ сократите дистанцию");
                        }
                    }
                }
            }
        else
        {
            if (JoinCamera.instance && JoinCamera.instance.IsEnabled())
            {
                JoinCamera.instance.ChangeActivity(false);
            }

            if (Input.GetKeyDown(equipKeyCode))
            {
                UnequipAccessory();
            }
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, cameraTurnOnDistance);
    }

    public void SetEnabled(bool value)
    {
        enabled = value;
        joinCamera.gameObject.SetActive(value);
    }
}