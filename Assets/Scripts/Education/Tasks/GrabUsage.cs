using UnityEngine;

public class GrabUsage : Task
{
    [Header("Объекты, связанные с задачей")]
    public Transform grabTransform;
    public RigidbodyGrab grab;
    public Transform grabDefaultPoint;
    public GameObject[] otherObjects;

    private float highestAngle;
    private float lowestAngle;

    protected override void EnableTaskGameObjects()
    {
        foreach (GameObject obj in otherObjects)
        {
            obj.SetActive(true);
        }
        grabTransform.gameObject.SetActive(true);
        grabTransform.transform.position = grabDefaultPoint.position;
        grabTransform.transform.rotation = grabDefaultPoint.rotation;
        highestAngle = RigidbodyGrab.maxRotationAngle - 0.5f;
        lowestAngle = RigidbodyGrab.minRotationAngle + 0.5f;
    }

    protected override void DisableTaskGameObjects()
    {
        foreach (GameObject obj in otherObjects)
        {
            obj.SetActive(false);
        }
        robot.accessoryJoinPoint.UnequipAccessory();
        grabTransform.gameObject.SetActive(false);
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

    private int Task_1() // Развести клешни на наибольший угол (минимальный угол Захвата)
    {
        if (grab.CurrentRotationAngle >= highestAngle)
        {
            SetStage(2, Task_2);
            return 1;
        }
        else return 0;
    }

    private int Task_2() // Соединить клешни (максимальный угол Захвата)
    {
        if (grab.CurrentRotationAngle <= lowestAngle)
        {
            SetStage(3, CompleteTask);
            return 1;
        }
        else return 0;
    }
}
