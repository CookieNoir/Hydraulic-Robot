using UnityEngine;

public class AccessoryChange : Task
{
    [Header("Объекты, связанные с задачей")]
    public Transform accessory;
    public Transform accessoryDefaultPoint;
    public PointOfInterest joinPoint;
    public PointOfInterest separationPoint;
    public GameObject[] otherObjects;

    protected override void EnableTaskGameObjects()
    {
        foreach (GameObject obj in otherObjects)
        {
            obj.SetActive(true);
        }
        accessory.gameObject.SetActive(true);
        joinPoint.gameObject.SetActive(true);
        accessory.position = accessoryDefaultPoint.position;
        accessory.rotation = accessoryDefaultPoint.rotation;
    }

    protected override void DisableTaskGameObjects()
    {
        foreach (GameObject obj in otherObjects)
        {
            obj.SetActive(false);
        }
        robot.accessoryJoinPoint.UnequipAccessory();
        accessory.gameObject.SetActive(false);
        joinPoint.gameObject.SetActive(false);
        separationPoint.gameObject.SetActive(false);
    }

    protected override int Task_0() // Достичь точки присоединения
    {
        if (joinPoint.IsReached())
        {
            joinPoint.gameObject.SetActive(false);
            SetStage(1, Task_1);
            return 1;
        }
        else return 0;
    }

    private int Task_1() // Приблизить стрелу для включения камеры
    {
        if (robot.accessoryJoinPoint.Selected)
        {
            SetStage(2, Task_2);
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private int Task_2() // Получить зеленый цвет
    {
        if (robot.accessoryJoinPoint.Equippable)
        {
            SetStage(3, Task_3);
            return 1;
        }
        else
        {
            if (robot.accessoryJoinPoint.Selected)
            {
                return 0;
            }
            else
            {
                SetStage(1, Task_1);
                return 1;
            }
        }
    }

    private int Task_3() // Присоединить оборудование
    {
        if (robot.accessoryJoinPoint.Equipped)
        {
            separationPoint.gameObject.SetActive(true);
            SetStage(4, Task_4);
            return 1;
        }
        else
        {
            if (robot.accessoryJoinPoint.Selected)
            {
                if (robot.accessoryJoinPoint.Equippable)
                {
                    return 0;
                }
                else
                {
                    SetStage(2, Task_2);
                    return 1;
                }
            }
            else
            {
                SetStage(1, Task_1);
                return 1;
            }
        }
    }

    private int Task_4() // Сбросить оборудование в точке 2
    {
        if (separationPoint.IsReached() && !robot.accessoryJoinPoint.Equipped)
        {
            SetStage(5, CompleteTask);
            return 1;
        }
        else
        {
            if (robot.accessoryJoinPoint.Selected)
            {
                if (robot.accessoryJoinPoint.Equippable)
                {
                    if (robot.accessoryJoinPoint.Equipped)
                    {
                        return 0;
                    }
                    else
                    {
                        SetStage(3, Task_3);
                        return 1;
                    }
                }
                else
                {
                    SetStage(2, Task_2);
                    return 1;
                }
            }
            else
            {
                SetStage(1, Task_1);
                return 1;
            }
        }
    }
}
