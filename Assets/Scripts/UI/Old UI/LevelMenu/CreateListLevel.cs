using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateListLevel : MonoBehaviour
{
    public Task[] Tasks;
    private List<ButtonInfo> _buttons = new List<ButtonInfo>();

    [Header("Для генерации кнопок")]
    public GameObject Button;
    public Transform ButtonParent;

    //isEducationMode
    //true - обычный режим обучения
    //false - режим доп. тренировок
    public void SetTasks(PauseMenuManager menuManager, Task[] task)
    {
        Tasks = task;
        GenerateLevels(menuManager);
    }

    private void OnEnable()
    {
        //при каждом открытии, обновляем очки уровней
        UpdateLevels();
    }

    public void GenerateLevels(PauseMenuManager menuManager)
    {
        RemoveList();
        int levelCount = Tasks.Length;
        for (int i = 0; i < levelCount; i++)
        {
            if (Tasks[i])
            {
                ButtonInfo button = Instantiate(Button, ButtonParent).GetComponent<ButtonInfo>();
                _buttons.Add(button);
                Task task = Tasks[i];
                button.SetTask(task, i + 1);
                button.SetEvent(() => EducationHandler.instance?.DropAndSetTask(task));
                button.SetEvent(() => menuManager.AllBack());
            }
        }
        _buttons[0].Button.Select();
    }

    public void RemoveList()
    {
        foreach (var button in _buttons)
        {
            Destroy(button.gameObject);
        }
        _buttons.Clear();
    }

    public void UpdateLevels()
    {
        foreach (var button in _buttons)
        {
            if (Tasks.Contains(button.GetTask()))
            {
                button.UpdateInfo();
            }
        }
        _buttons[0].Button.Select();
    }
}
