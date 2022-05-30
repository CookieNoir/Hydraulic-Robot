using UnityEngine;

public class ObjectMovement : Task
{
    [Space(15)]
    [Tooltip("При false для задания потребуется всего 3 описания: подбор захвата, подбор объекта и его сброс, завершение задания")]
    public bool showUniqueDescriptions = true;
    [Space(10, order = 0)]
    [Header("Объекты, связанные с задачей", order = 1)]
    public Transform grabTransform;
    public RigidbodyGrab grab;
    public Transform grabDefaultPoint;
    public PointOfInterest pointOfInterest;
    public Transform pointOfInterestDefaultPosition;
    public RigidbodyAttachableObject[] targetObjects;
    public Transform[] targetObjectsDefaultPositions;
    public GameObject[] otherObjects;
    public Marker marker;

    private int currentObject = 0;
    private int length;

    protected override void EnableTaskGameObjects()
    {
        length = targetObjects.Length;
        for (int i = 0; i < length; ++i)
        {
            Transform parent = targetObjects[i].transform.parent;
            parent.gameObject.SetActive(true);
            parent.position = targetObjectsDefaultPositions[i].position;
            parent.rotation = targetObjectsDefaultPositions[i].rotation;
        }
        foreach (GameObject obj in otherObjects)
        {
            obj.SetActive(true);
        }
        grabTransform.gameObject.SetActive(true);
        grabTransform.transform.position = grabDefaultPoint.position;
        grabTransform.transform.rotation = grabDefaultPoint.rotation;
        pointOfInterest.transform.position = pointOfInterestDefaultPosition.position;
        pointOfInterest.transform.rotation = pointOfInterestDefaultPosition.rotation;
    }

    protected override void DisableTaskGameObjects()
    {
        robot.accessoryJoinPoint.UnequipAccessory();
        for (int i = 0; i < length; ++i)
        {
            targetObjects[i].transform.parent.gameObject.SetActive(false);
        }
        foreach (GameObject obj in otherObjects)
        {
            obj.SetActive(false);
        }
        pointOfInterest.targetObjects.Clear();
        grabTransform.gameObject.SetActive(false);
        pointOfInterest.gameObject.SetActive(false);
        marker.gameObject.SetActive(false);
    }

    protected override int Task_0() // Присоединить оборудование
    {
        if (robot.accessoryJoinPoint.Equipped)
        {
            pointOfInterest.gameObject.SetActive(true);
            currentObject = 0;
            SetStage(currentObject + 1, Task_ObjectSelector);
        }
        return 0;
    }

    private int Task_ObjectSelector() // Выбрать следующий объект
    {
        if (currentObject < length)
        {
            marker.gameObject.SetActive(true);
            marker.UpdatePosition(targetObjects[currentObject].gameObject);
            pointOfInterest.targetObjects.Clear();
            pointOfInterest.targetObjects.Add(targetObjects[currentObject].transform);
            pointOfInterest.ResetReached();
            if (showUniqueDescriptions) SetStage(currentObject + 1, Task_1);
            else SetStage(1, Task_1);
        }
        else
        {
            if (showUniqueDescriptions) SetStage(length + 1, CompleteTask);
            else SetStage(2, CompleteTask);
        }
        return 1;
    }

    private int Task_1() // Взять объект
    {
        if (grab.AttachedObject == targetObjects[currentObject])
        {
            marker.gameObject.SetActive(false);
            if (showUniqueDescriptions) SetStage(currentObject + 1, Task_2);
            else SetStage(1, Task_2);
        }
        return 0;
    }

    private int Task_2() // Сбросить объект в указанную область
    {
        if (pointOfInterest.IsReached() && !grab.AttachedObject)
        {
            currentObject++;
            if (showUniqueDescriptions) SetStage(currentObject + 1, Task_ObjectSelector);
            else SetStage(1, Task_ObjectSelector);
        }
        else
        {
            if (grab.AttachedObject != targetObjects[currentObject])
            {
                marker.gameObject.SetActive(true);
                marker.UpdatePosition(targetObjects[currentObject].gameObject);

                if (showUniqueDescriptions) SetStage(currentObject + 1, Task_1);
                else SetStage(1, Task_1);
            }
        }
        return 0;
    }
}
