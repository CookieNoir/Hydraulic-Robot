using System.Collections.Generic;
using UnityEngine;

public class XDriveModificationTracking : Task
{
    [System.Serializable]
    public struct RobotXDriveModificationIndexPair
    {
        public RobotController robot;
        public int index;
    }
    [System.Serializable]
    public struct ValueDirectionPair
    {
        public float value;
        public bool isGreater; // True, если текущее значение меньше целевого
    }

    [Header("Объекты, связанные с задачей")]
    public List<RobotXDriveModificationIndexPair> xDriveIndexVariations; // Содержит пары <Робот - Индекс>, где индекс - порядковый номер компонента XDriveModification в заданном роботе
    public bool getFromLegsList;
    public List<ValueDirectionPair> valueDirectionPairs;
    private int currentPair;
    private float targetValue;
    private bool targetCondition;
    private ArticulationBodyXDriveModification xDriveModification;

    protected override void EnableTaskGameObjects()
    {
        xDriveModification = robot.GetXDriveModificationByIndex(xDriveIndexVariations.Find(element => element.robot == robot).index, getFromLegsList);
        currentPair = 0;
    }

    protected override int Task_0()
    {
        if (currentPair < valueDirectionPairs.Count)
        {
            targetValue = valueDirectionPairs[currentPair].value;
            targetCondition = valueDirectionPairs[currentPair].isGreater;
            SetStage(currentPair, Task_1);
        }
        else
        {
            SetStage(valueDirectionPairs.Count, CompleteTask);
        }
        return 1;
    }

    private int Task_1()
    {
        if ((xDriveModification.GetXDriveTarget() >= targetValue) == targetCondition)
        {
            currentPair++;
            SetStage(currentPair, Task_0);
        }
        return 0;
    }
}
