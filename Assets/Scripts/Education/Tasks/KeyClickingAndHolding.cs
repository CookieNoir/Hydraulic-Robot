using UnityEngine;
using UnityEngine.InputSystem;

public class KeyClickingAndHolding : Task
{
    [Header("Объекты, связанные с задачей")]
    public bool trackNumberOfClicks;
    public float timeForCompletion = 1f;
    public int clicksForCompletion = 0;
    public InputActionReference action;
    private InputAction inAction;
    private float remainingTime;
    private int remainingNumberOfClicks;
    private int taskResult = 0;
    private bool isTimerActive;

    protected override void EnableTaskGameObjects()
    {
        if (action is not null)
        {
            //inAction = (new InputController()).FindAction(action.name);
            inAction = action;
            if (!inAction.enabled) inAction.Enable();
        }
        else Debug.Log(string.Format("No action specified for game object {0}.", gameObject.name));

        taskResult = 0;
        isTimerActive = false;

        if (trackNumberOfClicks)
        {
            remainingNumberOfClicks = clicksForCompletion;
            inAction.performed += OnClicksPerformed;
        }
        else
        {
            remainingTime = timeForCompletion;
            inAction.started += StartTimer;
            inAction.canceled += StopTimer;
        }
    }

    private void OnClicksPerformed(InputAction.CallbackContext ctx)
    {
        if (--remainingNumberOfClicks <= 0) taskResult = 1;
    }

    private void StartTimer(InputAction.CallbackContext ctx)
    {
        isTimerActive = true;
    }

    private void StopTimer(InputAction.CallbackContext ctx)
    {
        isTimerActive = false;
    }

    protected override int Task_0()
    {
        if (isTimerActive)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0f) taskResult = 1;
        }
        if (taskResult > 0) SetStage(1, CompleteTask);
        return taskResult;
    }

}
