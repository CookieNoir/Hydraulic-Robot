using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyClickingAndHolding : Task
{
    [Header("Объекты, связанные с задачей")]
    public List<KeyCode> keyCodes;
    public bool trackNumberOfClicks;
    public float timeForCompletion = 1f;
    public int clicksForCompletion = 0;
    private float remainingTime;
    private int remainingNumberOfClicks;
    private Func<KeyCode, bool> getKeyFunction;
    private Func<int> onKeyPressedFunction;

    protected override void EnableTaskGameObjects()
    {
        if (trackNumberOfClicks)
        {
            remainingNumberOfClicks = clicksForCompletion;
            getKeyFunction = Input.GetKeyDown;
            onKeyPressedFunction = OnKeyPressedDown;
        }
        else
        {
            remainingTime = timeForCompletion;
            getKeyFunction = Input.GetKey;
            onKeyPressedFunction = OnKeyPressed;
        } 
    }

    protected override int Task_0()
    {
        for (int i = 0; i < keyCodes.Count; i++)
        {
            if (getKeyFunction(keyCodes[i]))
            {
                return onKeyPressedFunction();
            }
        }
        return 0;
    }

    private int OnKeyPressed()
    {
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0)
        {
            SetStage(1, CompleteTask);
            return 1;
        }
        else return 0;
    }

    private int OnKeyPressedDown()
    {
        remainingNumberOfClicks--;
        if (remainingNumberOfClicks <= 0)
        {
            SetStage(1, CompleteTask);
            return 1;
        }
        else return 0;
    }
}
