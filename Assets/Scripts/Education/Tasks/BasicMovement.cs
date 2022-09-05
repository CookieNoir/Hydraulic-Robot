using UnityEngine;

public class BasicMovement : Task
{
    public PointOfInterest pointOfInterest;
    public Transform[] pointOfInterestPositions;
    public GameObject[] otherObjects;
    private int length;
    private int currentPoint;
    protected override void EnableTaskGameObjects()
    {
        foreach (GameObject obj in otherObjects)
        {
            obj.SetActive(true);
        }
        length = pointOfInterestPositions.Length;
        currentPoint = 0;
        pointOfInterest.gameObject.SetActive(true);
        SetPosition(currentPoint);
    }

    protected override void DisableTaskGameObjects()
    {
        foreach (GameObject obj in otherObjects)
        {
            obj.SetActive(false);
        }
        pointOfInterest.gameObject.SetActive(false);
    }

    private void SetPosition(int index)
    {
        robot.SetState(null);
        pointOfInterest.transform.position = pointOfInterestPositions[index].position;
        pointOfInterest.transform.rotation = pointOfInterestPositions[index].rotation;
        pointOfInterest.ResetReached();
    }

    private int ChangePosition()
    {
        currentPoint++;
        if (currentPoint < pointOfInterestPositions.Length)
        {
            SetPosition(currentPoint);
            SetStage(currentPoint, Task_0);
        }
        else
        {
            SetStage(length, CompleteTask);
        }
        return 1;
    }

    protected override int Task_0()
    {
        if (pointOfInterest.IsReached())
        {
            SetStage(currentPoint, ChangePosition);
        }
        return 0;
    }
}