using UnityEngine;

public class TunnelManeuvering : Task
{
    public PlayerCollisionDetector[] moveableObjects;
    public Transform[] moveableObjectsDefaultPositions;
    public PointOfInterest endPoint;
    public float objectOffsetThreshold = 0.1f;

    protected override void EnableTaskGameObjects()
    {
        for (int i = 0; i < moveableObjects.Length; ++i)
        {
            moveableObjects[i].gameObject.SetActive(true);
            moveableObjects[i].transform.position = moveableObjectsDefaultPositions[i].position;
            moveableObjects[i].transform.rotation = moveableObjectsDefaultPositions[i].rotation;
            moveableObjects[i].DropValues();
        }
        endPoint.gameObject.SetActive(true);
        endPoint.ResetReached();
    }

    protected override void DisableTaskGameObjects()
    {
        for (int i = 0; i < moveableObjects.Length; ++i)
        {
            moveableObjects[i].gameObject.SetActive(false);
        }
        endPoint.gameObject.SetActive(false);
    }

    protected override int Task_0()
    {
        if (ObjectsWerentMoved())
        {
            if (endPoint.IsReached())
            {
                SetStage(1, CompleteTask); // Задание пройдено
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            SetStage(2, TerminateTask); // Задание прервано. Объекты должны быть неподвижны
            return 1;
        }
    }

    protected bool ObjectsWerentMoved()
    {
        bool result = true;
        for (int i = 0; i < moveableObjects.Length; ++i)
        {
            if (moveableObjects[i].GetOffsetSqrMagnitude() > objectOffsetThreshold)
            {
                result = false;
                break;
            }
        }
        return result;
    }
}