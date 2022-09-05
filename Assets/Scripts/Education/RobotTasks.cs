using System;
using UnityEngine;

public class RobotTasks : MonoBehaviour
{
    public Task[] Tasks;

    public void ResetAllTasks()
    {
        foreach (Task task in Tasks)
        {
            task.RemoveValue();
        }
    }
}