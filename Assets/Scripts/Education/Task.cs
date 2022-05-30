using UnityEngine;
using System;

public enum TaskMode
{
    Education,
    Training
}

public interface TaskCurrentValue
{
    int currentValue { get; set; } //хранит текущее кол-во очков
    int currentExtraValue { get; set; } //хранит текущее доп. кол-во очков(для задач доп. тренировки)
}

public class Task : MonoBehaviour, TaskCurrentValue
{
    public RobotController robot;
    public int currentValue { get; set; }
    public int currentExtraValue { get; set; }
    public TaskMode taskMode = TaskMode.Education; // Определяет тип задачи: в режиме обучения или доп. тренировок
    [Header("Наименование задачи")]
    public string taskNamePrefix;
    public string taskNameBody;
    public string taskNameSuffix;
    [HideInInspector] public string taskName; // Название задачи, будет отображаться в меню уровней. Собирается из префикса, основной части и суффикса
    [Header("Свойства задачи")]
    public RobotState robotState;
    public int value; // Количество очков, которое получит пользователь за выполнение задания
    public string[] taskDescriptions; // Набор описаний задачи
    public string[] instructions; // Набор инструкций для решения задачи
    public Vector2Int[] pairs; // Набор из индексов описания и инструкций, который будет выведен на заданном этапе
    [Header("Таймер")]
    public float timeLimit = 0f; // Количество секунд, выделяемое на задание. Если меньше или равно 0, то таймер не запускается
    protected int stage = 0; // Этап выполнения задания. Стартовый этап имеет индекс 0
    /* если 0, то выполняется промежуточный этап задачи
       если 1, то промежуточный этап задачи успешно выполнен (изменяются описание и инструкции задания)
       если 2, то задача успешно выполнена
       если -1, то достигнут этап, при котором завершить задачу невозможно, требуется перезапуск уровня */
    protected Func<int> stageTask;
    protected string currentDescription;
    protected string currentInstruction;
    [HideInInspector] public bool isSuccesfullyEnded = false;
    [HideInInspector] public bool isWaitingForCompletion = false;

    protected void Awake()
    {
        ConnectNameParts();
    }

    private void ConnectNameParts()
    {
        taskName = taskNamePrefix + ' ' + taskNameBody + ' ' + taskNameSuffix;
    }

    public string GetCurrentDescription()
    {
        return currentDescription;
    }

    public string GetCurrentInstruction()
    {
        return currentInstruction;
    }

    public int CheckStageTask()
    {
        return stageTask();
    }

    public float GetTimeWithDelay()
    {
        return (timeLimit > 0 && robot) ? timeLimit + robot.timeForSettingState : 0f;
    }

    protected void SetStage(int nextStage, Func<int> newTask)
    {
        stage = nextStage;
        stageTask = newTask;
        if (stage > -1 && stage < pairs.Length)
        {
            if (pairs[stage].x > -1 && pairs[stage].x < taskDescriptions.Length)
            {
                currentDescription = taskDescriptions[pairs[stage].x];
            }
            else
            {
                currentDescription = null;
            }
            if (pairs[stage].y > -1 && pairs[stage].y < instructions.Length)
            {
                currentInstruction = instructions[pairs[stage].y];
            }
            else
            {
                currentInstruction = null;
            }
        }
        else
        {
            currentDescription = null;
            currentInstruction = null;
        }
    }

    protected virtual void EnableTaskGameObjects()
    {
        // Включить и настроить все GameObject'ы, связанные с этим заданием
    }

    protected virtual void DisableTaskGameObjects()
    {
        // Выключить все GameObject'ы, связанные с этим заданием
    }

    protected void RobotSetStartPosition()
    {
        robot.accessoryJoinPoint.UnequipAccessory();
        robot.SetState(robotState);
        OnSettingSpecialState();
    }

    protected virtual void OnSettingSpecialState()
    {
        // mainGameObject.cameraController.SetSpecialCamera(CameraController.SpecialCameras.Class_3_Tasks);
    }

    private void DropSpecialState()
    {
        CameraController.instance.SetRegularCamera();
    }

    public RobotController Take()
    {
        RobotSetStartPosition();
        EnableTaskGameObjects();
        isWaitingForCompletion = false;
        SetStage(0, Task_0);
        return robot;
    }

    public void Drop()
    {
        DisableTaskGameObjects();
        DropSpecialState();
    }

    public int Evaluate(bool isCompleted, float valueMultiplier)
    {
        if (isCompleted)
        {
            int newValue = Mathf.FloorToInt(valueMultiplier * value);
            if (taskMode == TaskMode.Education)
            {
                currentValue = currentValue < newValue ? newValue : currentValue;
            }
            else
            {
                currentExtraValue = currentExtraValue < newValue ? newValue : currentExtraValue;
            }
            return value;
        }
        else
            return 0;
    }

    // Рекомендуется использовать данный способ наименования для последующих заданий
    protected virtual int Task_0() // Обязательная функция, отвечающая за выполнение первой части задания
    {
        return 0;
    }

    protected int CompleteTask() // При успешном завершении задания требуется установить делегат stageTask, равный CompleteTask
    {
        isSuccesfullyEnded = true;
        isWaitingForCompletion = true;
        return 2;
    }

    public int TerminateTask() // При неудачном завершении задания требуется установить делегат stageTask, равный TerminateTask
    {
        isSuccesfullyEnded = false;
        isWaitingForCompletion = true;
        return -1;
    }

    public void RemoveValue()
    {
        currentValue = 0;
        currentExtraValue = 0;
    }
}
