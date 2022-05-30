using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class AllTasks : MonoBehaviour
{
    //уровни нельзя менять местами, если уже начался сеанс пользователя.
    //если порядок был изменен, то необходимо сбросить все очки, полученные пользователем (начать заново)
    // private List<Task> Tasks;
    public Task[] Tasks;
    public Task[] RandomTasks;
    public int CountRandomTasks;
    public Task ResearchTask;

    private void Awake()
    {
        SetRandomTasks();
    }

    public void ResetTask()
    {
        //новые случ. таски
        SetRandomTasks();
        foreach (var task in Tasks)
        {
            if (task)
            {
                task.RemoveValue();
            }
        }
    }

    public void SetTasks(Task[] tasks)
    {
        Tasks = tasks;
        SetRandomTasks();
    }

    /*
    public void SetTasks()
    {
        int count = transform.childCount;
        Tasks = new List<Task>();
        for (int i = 0; i < count; ++i)
        {
            Transform child = transform.GetChild(i);
            Task task = child.GetComponent<Task>();
            if (!task) // Встретили каталог
            {
                int innerCount = child.childCount;
                for (int j = 0; j < innerCount; ++j)
                {
                    Tasks.Add(child.GetChild(j).GetComponent<Task>());
                }
            }
            else Tasks.Add(task);
        }
    } */

    private void SetRandomTasks()
    {
        RandomTasks = new Task[CountRandomTasks];
        for (int i = 0; i < CountRandomTasks;)
        {
            Task randomTask = Tasks[Random.Range(0, Tasks.Length)];
            if (!RandomTasks.Contains(randomTask))
            {
                RandomTasks[i] = randomTask;
                i++;
            }
        }
    }

    /*
    public Task this[int key]
    {
        get => Tasks[key];
    }

    public int Length
    {
        get => Tasks.Count;
    }*/
}
