using UnityEngine;

public class RailLaying : Task
{
    [Header("Объекты, связанные с задачей")]
    public Transform grabTransform;
    public RigidbodyGrab grab;
    [Space(15)]
    public RigidbodyAttachableObject rail;
    public Transform railDefaultPoint;
    public Transform railPoint1;
    public Transform railPoint2;
    public GameObject transparentRail;
    public Transform transparentRailDefaultPoint;
    public Transform transparentRailPoint1;
    public Transform transparentRailPoint2;
    [Space(15)]
    public GameObject[] otherObjects;
    private Transform parent;

    protected override void EnableTaskGameObjects()
    {
        grabTransform.gameObject.SetActive(true);
        robot.accessoryJoinPoint.SetAccessory(grab);

        parent = rail.transform.parent;
        parent.gameObject.SetActive(true);
        parent.position = railDefaultPoint.position;
        parent.rotation = railDefaultPoint.rotation;
        transparentRail.SetActive(true);

        for (int i = 0; i < otherObjects.Length; ++i)
        {
            otherObjects[i].SetActive(true);
        }
    }

    protected override void DisableTaskGameObjects()
    {
        robot.accessoryJoinPoint.UnequipAccessory();
        grabTransform.gameObject.SetActive(false);

        rail.transform.parent.gameObject.SetActive(false);
        transparentRail.SetActive(false);

        for (int i = 0; i < otherObjects.Length; ++i)
        {
            otherObjects[i].SetActive(false);
        }

        parent = null;
    }

    protected override int Task_0()
    {
        if (isRailSet() && isAngleCorrect() && !grab.AttachedObject)
        {
            SetStage(1, CompleteTask);
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private bool isRailSet()
    {
        float distance1 = (railPoint1.position - transparentRailPoint1.position).sqrMagnitude;
        float distance2 = (railPoint1.position - transparentRailPoint2.position).sqrMagnitude;
        if (distance1 < distance2)
        {
            distance2 = (railPoint2.position - transparentRailPoint2.position).sqrMagnitude;
        }
        else
        {
            distance1 = (railPoint2.position - transparentRailPoint1.position).sqrMagnitude;
        }
        return (distance1 < 0.0025f) && (distance2 < 0.0025f);
    }

    private bool isAngleCorrect()
    {
        float angle = Mathf.DeltaAngle(parent.eulerAngles.z, 0f);
        return angle < 10f;
    }
}