using UnityEngine;

public class HydrohammerUsage : Task
{
    [Header("Объекты, связанные с задачей")]
    public Transform hammerTransform;
    public RigidbodyHydraulicHammer hammer;
    public Transform hammerDefaultPoint;
    public GameObject[] otherObjects;

    protected override void EnableTaskGameObjects()
    {
        foreach (GameObject obj in otherObjects)
        {
            obj.SetActive(true);
        }
        hammerTransform.gameObject.SetActive(true);
        hammerTransform.transform.position = hammerDefaultPoint.position;
        hammerTransform.transform.rotation = hammerDefaultPoint.rotation;
    }

    protected override void DisableTaskGameObjects()
    {
        foreach (GameObject obj in otherObjects)
        {
            obj.SetActive(false);
        }
        robot.accessoryJoinPoint.UnequipAccessory();
        hammerTransform.gameObject.SetActive(false);
    }

    protected override int Task_0() // Присоединить оборудование
    {
        if (robot.accessoryJoinPoint.Equipped)
        {
            SetStage(1, Task_1);
            return 1;
        }
        else return 0;
    }

    private int Task_1() // Включить Гидромолот
    {
        if (hammer.hitPoints.activeSelf)
        {
            SetStage(2, Task_2);
            return 1;
        }
        else
        {
            if (robot.accessoryJoinPoint.Equipped)
            {
                return 0;
            }
            else
            {
                SetStage(0, Task_0);
                return 1;
            }
        }
    }

    private int Task_2() // Выключить Гидромолот
    {
        if (robot.accessoryJoinPoint.Equipped)
        {
            if (!hammer.hitPoints.activeSelf)
            {
                SetStage(3, CompleteTask);
                return 1;
            }
            else return 0;
        }
        else
        {
            SetStage(0, Task_0);
            return 1;
        }
    }
}
