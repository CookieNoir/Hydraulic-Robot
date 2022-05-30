using UnityEngine;

public class BoxPyramid : Task
{
    public float stabilityTime = 5f;
    public float speedThreshold = 0.02f;

    [Header("Объекты, связанные с задачей")]
    public Transform accessory;
    public Transform accessoryDefaultPoint;
    public ReachableBoxArea[] levels;
    public Rigidbody[] rigidbodies;
    public Transform[] rigidbodiesDefaultPoints;
    public MeshRenderer pointOfInterest;
    public Transform pointOfInterestDefaultPosition;
    public GameObject[] otherObjects;

    private int levelAmount;
    private int rigidbodyAmount;
    private float timeSpent;
    private Material material;
    private bool reachedStage1;

    private void Start()
    {
        material = pointOfInterest.material;
    }

    protected override void EnableTaskGameObjects()
    {
        foreach (GameObject obj in otherObjects)
        {
            obj.SetActive(true);
        }
        accessory.gameObject.SetActive(true);
        accessory.position = accessoryDefaultPoint.position;
        accessory.rotation = accessoryDefaultPoint.rotation;
        rigidbodyAmount = rigidbodies.Length;
        for (int i = 0; i < rigidbodyAmount; ++i)
        {
            rigidbodies[i].gameObject.SetActive(true);
            rigidbodies[i].position = rigidbodiesDefaultPoints[i].position;
            rigidbodies[i].rotation = rigidbodiesDefaultPoints[i].rotation;
        }
        pointOfInterest.gameObject.SetActive(true);
        pointOfInterest.transform.position = pointOfInterestDefaultPosition.position;
        pointOfInterest.transform.rotation = pointOfInterestDefaultPosition.rotation;
        levelAmount = levels.Length;
        timeSpent = 0;
        material.SetFloat("_Value", 0);
        reachedStage1 = false;
    }

    protected override void DisableTaskGameObjects()
    {
        foreach (GameObject obj in otherObjects)
        {
            obj.SetActive(false);
        }
        robot.accessoryJoinPoint.UnequipAccessory();
        accessory.gameObject.SetActive(false);
        for (int i = 0; i < rigidbodyAmount; ++i)
        {
            rigidbodies[i].gameObject.SetActive(false);
        }
        pointOfInterest.gameObject.SetActive(false);
    }

    // Рекомендуется использовать данный способ наименования для последующих заданий
    protected override int Task_0() // Обязательная функция, отвечающая за выполнение первой части задания
    {
        int result = 0;
        int ret = 0;
        for (int i = 0; i < levelAmount; ++i)
        {
            result += levels[i].IsEnoughObjects();
        }
        if (result == levelAmount)
        {
            if (!reachedStage1)
            {
                SetStage(1, Task_0);
                reachedStage1 = true;
                ret = 1;
            }
            if (RigidbodiesAreStatic())
            {
                timeSpent += Time.deltaTime;
                material.SetFloat("_Value", timeSpent / stabilityTime);
                if (timeSpent >= stabilityTime)
                {
                    SetStage(2, CompleteTask);
                    return 1;
                }
            }
            else
            {
                timeSpent = 0;
                material.SetFloat("_Value", 0);
            }
        }
        else
        {
            timeSpent = 0;
            material.SetFloat("_Value", 0);
        }
        return ret;
    }

    private bool RigidbodiesAreStatic()
    {
        for (int i = 0; i < rigidbodyAmount; ++i)
        {
            if (!rigidbodies[i].useGravity || Vector3.SqrMagnitude(rigidbodies[i].velocity) > speedThreshold) return false;
        }
        return true;
    }
}
