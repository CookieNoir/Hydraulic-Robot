using UnityEngine;
using UnityEngine.UI;

public class SelectLevelView : MenuView
{
    [SerializeField] private RobotSelector _robotSelector;
    [SerializeField] private LevelsButtonsGenerator _levelsButtonsGenerator;
    [SerializeField] private ScrollRect _levelScrollView;
    private RobotTasks _previousTaskHandler;

    protected override void OnMenuViewEnabled()
    {
        RobotTasks tasks = _robotSelector.SelectedRobotController.robotTasks;
        if (tasks == _previousTaskHandler) firstSelectedButton = _levelsButtonsGenerator.RefreshLevels();
        else firstSelectedButton = _levelsButtonsGenerator.GenerateLevels(tasks.Tasks);
        _previousTaskHandler = tasks;
    }

    protected override void OnButtonSelected(Button button)
    {
        UiHelper.SnapTo(_levelScrollView, button.GetComponent<RectTransform>());
    }
}