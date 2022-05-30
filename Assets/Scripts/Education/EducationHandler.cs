using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EducationHandler : MonoBehaviour
{
    public static EducationHandler instance;

    [Space(10)]
    public Text taskDescription;
    public Text instruction;
    public MaskableGraphic descriptionObject;
    public MaskableGraphic instructionObject;

    [Header("UI Эффекты")]
    public float effectDuration = 1f;

    [Header("Таймер")]
    public TimerContent timerContent;
    public string timeoutText;
    private float valueMultiplier;
    private float penaltyTime;
    private Task task = null;
    private bool taskValuesAreSet = false;
    private bool taskNotEvaluated = false;
    private Task previousTask = null;
    private IEnumerator maskFlicker;
    private float alpha = 1f;
    private float step;
    private IEnumerator dropTaskDelay;

    private void Awake()
    {
        if (instance)
        {
            Debug.Log("Instance of EducationHandler already exists");
            Destroy(this);
        }
        else
        {
            instance = this;
            dropTaskDelay = DropTaskDelay();
        }
    }

    private float timeLeft = 0f;
    private bool timerIsUsed = false;

    private IEnumerator MaskFlicker(string newTaskDescription = null, string newInstruction = null)
    {
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * step;
            SetMaskAlpha(alpha * alpha);
            yield return null;
        }
        alpha = 0;

        SetTextValues(newTaskDescription, newInstruction);
        if (task.isWaitingForCompletion)
        {
            if (task.isSuccesfullyEnded)
                timerContent.SetEndColor();
            else
                timerContent.SetTerminateColor();
            timerIsUsed = false;
        }

        while (alpha < 1f)
        {
            alpha += Time.deltaTime * step;
            SetMaskAlpha(alpha * alpha);
            yield return null;
        }
        alpha = 1f;
        SetMaskAlpha(alpha);
    }

    private void SetMaskAlpha(float alpha)
    {
        Color result = Color.white * alpha;
        descriptionObject.color = result;
        instructionObject.color = result;
    }

    private void ResetEffect()
    {
        StopCoroutine(maskFlicker);
        alpha = 1f;
        SetMaskAlpha(alpha);
    }

    private void Start()
    {
        ChangeWindowsActivity(false);
        maskFlicker = MaskFlicker(null, null);
        SetMaskAlpha(1f);
        step = 2f / effectDuration;
    }

    public void SetTextValues(string newTaskDescription, string newInstruction)
    {
        if (newTaskDescription != null && newTaskDescription.Length > 0)
        {
            descriptionObject.gameObject.SetActive(true);
            taskDescription.text = newTaskDescription;
        }
        else
        {
            descriptionObject.gameObject.SetActive(false);
        }
        if (newInstruction != null && newInstruction.Length > 0)
        {
            instructionObject.gameObject.SetActive(true);
            instruction.text = newInstruction;
        }
        else
        {
            instructionObject.gameObject.SetActive(false);
        }
    }

    private void SetTimer(float value)
    {
        if (value <= 0)
        {
            timerIsUsed = false;
        }
        else
        {
            timerIsUsed = true;
            timeLeft = value;
            timerContent.UpdateTime(timeLeft);
            timerContent.SetDefaultValues();
        }
        timerContent.gameObject.SetActive(timerIsUsed);
    }

    public void SetTask(Task newTask)
    {
        task = newTask;
        if (DarkScreen.instance)
        {
            DarkScreen.instance.ExecuteInDarkScreen(1f, SetValuesFromTask);
        }
        else
        {
            SetValuesFromTask();
        }
    }

    private void SetValuesFromTask()
    {
        if (RobotSelector.instance)
        {
            RobotSelector.instance.selectedRobotController.SetState(null);
            RobotSelector.instance.SelectRobot(task.Take());
            ChangeWindowsActivity(true);
            SetTextValues(task.GetCurrentDescription(), task.GetCurrentInstruction());
            SetTimer(task.GetTimeWithDelay());
            penaltyTime = -task.timeLimit;
            valueMultiplier = 1f;
            ResetEffect();
            taskValuesAreSet = true;
            taskNotEvaluated = true;
        }
    }

    public void EndTask(bool result)
    {
        task.Evaluate(result, valueMultiplier);
        taskNotEvaluated = false;
        StopCoroutine(dropTaskDelay);
        dropTaskDelay = DropTaskDelay();
        StartCoroutine(dropTaskDelay);
    }

    private void ChangeWindowsActivity(bool value)
    {
        descriptionObject.gameObject.SetActive(value);
        instructionObject.gameObject.SetActive(value);
    }

    private void Update()
    {
        if (taskValuesAreSet && taskNotEvaluated)
        {
            UpdateTime();
            int result = task.CheckStageTask();
            switch (result)
            {
                case 1: // если 1, то промежуточный этап задачи успешно выполнен
                    {
                        StopCoroutine(maskFlicker);
                        maskFlicker = MaskFlicker(task.GetCurrentDescription(), task.GetCurrentInstruction());
                        StartCoroutine(maskFlicker);
                        break;
                    }
                case 2: // если 2, то задача успешно выполнена
                    {
                        // установить число очков за задание, равное value в task

                        EndTask(true);
                        break;
                    }
                case -1: // если -1, то достигнут этап, при котором завершить задачу невозможно, требуется перезапуск задания
                    {
                        // предложить пользователю перезапустить задание или вернуться в меню
                        EndTask(false);
                        break;
                    }
                default: break; // если 0, то выполняется промежуточный этап задачи
            }
        }
    }

    private void UpdateTime()
    {
        if (timerIsUsed && !task.isWaitingForCompletion)
        {
            timeLeft -= Time.deltaTime;
            timerContent.UpdateTime(timeLeft);
            if (timeLeft <= 0)
            {
                valueMultiplier = (penaltyTime - timeLeft) / penaltyTime;
                if (timeLeft <= penaltyTime)
                {
                    task.TerminateTask();
                    StopCoroutine(maskFlicker);
                    maskFlicker = MaskFlicker(timeoutText, null);
                    StartCoroutine(maskFlicker);
                    timerIsUsed = false;
                }
            }
        }
    }

    public void ResetTask()
    {
        if (task)
        {
            StopCoroutine(dropTaskDelay);
            DropTask();
            SetTask(previousTask);
        }
        else if (previousTask)
        {
            SetTask(previousTask);
        }
    }

    public void DropAndSetTask(Task task)
    {
        StopCoroutine(dropTaskDelay);
        DropTask();
        if (task) SetTask(task);
    }

    public void DropTask()
    {
        if (taskValuesAreSet)
        {
            task.Drop();
            previousTask = task;
            task = null;
            taskValuesAreSet = false;
        }
        ChangeWindowsActivity(false);
    }

    private IEnumerator DropTaskDelay()
    {
        yield return new WaitForSeconds(5f);
        DropTask();
    }
}
