using UnityEngine;

public class TunnelClearing : Task
{
    [Header("Объекты, связанные с задачей")]
    public Transform grabTransform;
    public RigidbodyGrab grab;
    [Space(15)]
    public ReachableBoxArea boxArea;
    public Transform boxAreaDefaultPoint;
    [Space(15)]
    public RigidbodyAttachableObject[] targetObjects;
    public Transform[] targetObjectsDefaultPositions;
    private int length;

    protected override void EnableTaskGameObjects()
    {
        grabTransform.gameObject.SetActive(true);
        robot.accessoryJoinPoint.SetAccessory(grab);
        length = targetObjects.Length;

        boxArea.gameObject.SetActive(true);
        boxArea.transform.position = boxAreaDefaultPoint.position;
        boxArea.transform.rotation = boxAreaDefaultPoint.rotation;
        boxArea.targetObjects.Clear();
        boxArea.ResetReached();
        boxArea.requiredObjectsAmount = length;

        for (int i = 0; i < length; ++i)
        {
            Transform parent = targetObjects[i].transform.parent;
            parent.gameObject.SetActive(true);
            parent.position = targetObjectsDefaultPositions[i].position;
            parent.rotation = targetObjectsDefaultPositions[i].rotation;
            boxArea.targetObjects.Add(parent);
        }
    }

    protected override void DisableTaskGameObjects()
    {
        robot.accessoryJoinPoint.UnequipAccessory();
        grabTransform.gameObject.SetActive(false);

        boxArea.gameObject.SetActive(false);

        for (int i = 0; i < length; ++i)
        {
            targetObjects[i].transform.parent.gameObject.SetActive(false);
        }
    }

    protected override int Task_0()
    {
        if (boxArea.IsEnoughObjects() > 0)
        {
            SetStage(1, CompleteTask);
            return 1;
        }
        else
        {
            return 0;
        }
    }
}