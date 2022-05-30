using UnityEngine;

public class AccessoryDelivery : Task
{
    [Header("Объекты, связанные с задачей")]
    public Transform accessory;
    public Transform accessoryDefaultPoint;
    public PointOfInterest separationPoint;
    public Transform pedestal;
    public Transform pedestalDefaultPoint;
    public GameObject[] otherObjects;

    protected override void EnableTaskGameObjects()
    {
        foreach (GameObject obj in otherObjects)
        {
            obj.SetActive(true);
        }
        accessory.gameObject.SetActive(true);
        pedestal.gameObject.SetActive(true);
        accessory.position = accessoryDefaultPoint.position;
        accessory.rotation = accessoryDefaultPoint.rotation;
        pedestal.position = pedestalDefaultPoint.position;
        pedestal.rotation = pedestalDefaultPoint.rotation;
        separationPoint.ResetReached();
    }

    protected override void DisableTaskGameObjects()
    {
        foreach (GameObject obj in otherObjects)
        {
            obj.SetActive(false);
        }
        robot.accessoryJoinPoint.UnequipAccessory();
        accessory.gameObject.SetActive(false);
        pedestal.gameObject.SetActive(false);
        separationPoint.gameObject.SetActive(false);
    }

    protected override int Task_0() // Присоединить оборудование
    {
        if (robot.accessoryJoinPoint.Equipped)
        {
            separationPoint.gameObject.SetActive(true);
            SetStage(1, Task_1);
            return 1;
        }
        else return 0;
    }

    private int Task_1() // Сбросить оборудование
    {
        if (separationPoint.IsReached() && !robot.accessoryJoinPoint.Equipped)
        {
            SetStage(2, CompleteTask);
            return 1;
        }
        else return 0;
    }
}
