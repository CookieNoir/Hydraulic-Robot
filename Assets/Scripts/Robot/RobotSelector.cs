using System.Collections.Generic;
using UnityEngine;

public class RobotSelector : MonoBehaviour
{
    public static RobotSelector instance;

    public List<RobotController> RobotControllers;
    public RobotController selectedRobotController;

    private void Awake()
    {
        if (instance)
        {
            Debug.Log("Instance of RobotSelector already exists");
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        RobotControllers.ForEach(r => r.SetEnabled(false));
        SelectRobot();
    }

    public void SelectRobot(RobotController robotController)
    {
        if (selectedRobotController != robotController)
        {
            if (selectedRobotController) selectedRobotController.SetEnabled(false);
            selectedRobotController = robotController;
            SelectRobot();
        }
    }

    private void SelectRobot()
    {
        selectedRobotController.SetEnabled(true);
        if (JoinCamera.instance) JoinCamera.instance.joinCamera = selectedRobotController.accessoryJoinPoint.joinCamera;
        CameraController.instance?.SetCamerasContainers(selectedRobotController);
    }
}
