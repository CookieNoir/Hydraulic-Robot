using UnityEngine;

public class WallBreakingPattern : Task
{
    [Header("Объекты, связанные с задачей разрушения стены")]
    public Transform hammerTransform;
    public Transform hammerDefaultPoint;

    public GameObject wallPrefab;
    public Transform wallDefaultPoint;
    public Transform area;
    public Transform areaDefaultPosition;
    public Transform grid; // Является дочерним объектом к Area, 
    // содержит объекты с компонентом ReachablePoint

    public GameObject[] otherObjects;

    protected int reachablePointsAmount;
    protected ReachablePoint[] reachablePoints;
    protected int[] visitedPoints;

    private GameObject instantiatedWall;

    protected void EnableWallBreakingObjects()
    {
        foreach (GameObject obj in otherObjects)
        {
            obj.SetActive(true);
        }
        hammerTransform.gameObject.SetActive(true);
        hammerTransform.position = hammerDefaultPoint.position;
        hammerTransform.rotation = hammerDefaultPoint.rotation;
        instantiatedWall = Instantiate(wallPrefab, wallDefaultPoint.position, wallDefaultPoint.rotation);
        area.gameObject.SetActive(true);
        area.transform.position = areaDefaultPosition.position;
        area.transform.rotation = areaDefaultPosition.rotation;
        reachablePointsAmount = grid.childCount;
        reachablePoints = new ReachablePoint[reachablePointsAmount];
        visitedPoints = new int[reachablePointsAmount];
        for (int i = 0; i < reachablePointsAmount; ++i)
        {
            reachablePoints[i] = grid.GetChild(i).GetComponent<ReachablePoint>();
            visitedPoints[i] = 0;
        }
    }

    protected void DisableWallBreakingObjects()
    {
        foreach (GameObject obj in otherObjects)
        {
            obj.SetActive(false);
        }
        robot.accessoryJoinPoint.UnequipAccessory();
        hammerTransform.gameObject.SetActive(false);
        Destroy(instantiatedWall);
        area.gameObject.SetActive(false);
    }

    protected int CountVisitedPoints()
    {
        int amount = 0;
        for (int i = 0; i < reachablePointsAmount; ++i)
        {
            if (reachablePoints[i].IsReached()) visitedPoints[i] = 1;
            amount += visitedPoints[i];
        }
        return amount;
    }
}
