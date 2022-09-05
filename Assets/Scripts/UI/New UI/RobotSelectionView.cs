using UnityEngine;
using UnityEngine.UI;

public class RobotSelectionView : MenuView
{
    [SerializeField] MainMenuUi _mainMenuUi;
    [SerializeField] private RobotSelector _robotSelector;
    [SerializeField] private Button[] _robotButtons; // ƒолжны повтор€ть пор€док роботов в классе RobotSelector

    protected override void OnMenuViewEnabled()
    {
        _HideSelectedRobot(_robotSelector.GetIndexOfSelectedRobot());
        _robotSelector.OnRobotSelected += _HideSelectedRobot;
    }

    protected override void OnMenuViewDisabled()
    {
        _robotSelector.OnRobotSelected -= _HideSelectedRobot;
    }

    public void SelectRobot(RobotController robot)
    {
        EducationHandler.instance.DropTask();
        _robotSelector.SelectRobot(robot);
        _mainMenuUi.Continue();
    }

    private void _HideSelectedRobot(int index)
    {
        for (int i = 0; i < _robotButtons.Length; ++i)
        {
            _robotButtons[i].interactable = i != index;
        }
    }
}