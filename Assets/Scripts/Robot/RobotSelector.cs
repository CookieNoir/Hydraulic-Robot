using System;
using System.Collections.Generic;
using UnityEngine;

public class RobotSelector : MonoBehaviour
{
    [SerializeField] private RobotController _startRobotController;
    [SerializeField] private List<RobotController> _robotControllers;
    public RobotController SelectedRobotController { get; private set; }
    public event Action<int> OnRobotSelected;
    private RobotController _previousRobotController;
    private RobotController _nextRobotController;

    private void Start()
    {
        SelectRobot(_startRobotController);
    }

    public void SelectRobot(RobotController robotController)
    {
        if (SelectedRobotController != robotController)
        {
            _previousRobotController = SelectedRobotController;
            _nextRobotController = robotController;
            DarkScreen.instance.ExecuteInDarkScreen(1f, _SelectRobot);
        }
    }

    public int GetIndexOfSelectedRobot()
    {
        if (SelectedRobotController)
        {
            return _robotControllers.IndexOf(SelectedRobotController);
        }
        else
        {
            return -1;
        }
    }

    public void DropProgression()
    {
        foreach (RobotController robot in _robotControllers)
        {
            robot.robotTasks.ResetAllTasks();
        }
    }

    private void _SelectRobot()
    {
        if (_previousRobotController) _previousRobotController.gameObject.SetActive(false);
        SelectedRobotController = _nextRobotController;
        SelectedRobotController.gameObject.SetActive(true);
        SelectedRobotController.SetState(null);
        if (JoinCamera.instance) JoinCamera.instance.joinCamera = SelectedRobotController.accessoryJoinPoint.joinCamera;
        CameraController.instance?.SetCamerasContainers(SelectedRobotController);
        EducationHandler.instance.DropProgression();
        OnRobotSelected?.Invoke(GetIndexOfSelectedRobot());
    }
}
