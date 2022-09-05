using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour
{
    private string _taskNumberString;
    private Task _task;
    public Button Button;
    public Text LevelName;
    public Text LevelScore;

    public void UpdateInfo()
    {
        LevelName.text = _taskNumberString + _task.taskName;
        LevelScore.text = _task.currentValue.ToString();
    }

    public void SetTask(Task task, int number)
    {
        _task = task;
        _taskNumberString = number.ToString() + ". ";
        LevelName.text = _taskNumberString + task.taskName;
        LevelScore.text = task.currentValue.ToString();
    }

    public Task GetTask()
    {
        return _task;
    }

    public void SetEvent(UnityAction call)
    {
        Button.onClick.AddListener(call);
    }
}
