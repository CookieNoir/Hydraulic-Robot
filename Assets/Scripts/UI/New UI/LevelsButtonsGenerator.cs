using UnityEngine;
using UnityEngine.UI;

public class LevelsButtonsGenerator : MonoBehaviour
{
    [SerializeField] private MainMenuUi _mainMenuUi;
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private Transform _buttonsParent;
    private ButtonInfo[] _buttons;

    public Button GenerateLevels(Task[] tasks)
    {
        _removeList();
        _buttons = new ButtonInfo[tasks.Length];
        for (int i = 0; i < _buttons.Length; i++)
        {
            if (tasks[i])
            {
                ButtonInfo button = Instantiate(_buttonPrefab, _buttonsParent).GetComponent<ButtonInfo>();
                _buttons[i] = button;
                Task task = tasks[i];
                button.SetTask(task, i + 1);
                button.SetEvent(() =>
                    {
                        EducationHandler.instance?.DropAndSetTask(task);
                        _mainMenuUi.Continue();
                    });
            }
        }
        return _buttons[0].Button;
    }

    private void _removeList()
    {
        if (_buttons != null)
        {
            for (int i = 0; i < _buttons.Length; ++i)
            {
                Destroy(_buttons[i].gameObject);
            }
        }
    }

    public Button RefreshLevels()
    {
        for (int i = 0; i < _buttons.Length; ++i)
        {
            _buttons[i].UpdateInfo();
        }
        return _buttons[0].Button;
    }
}