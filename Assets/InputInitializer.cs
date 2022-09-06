using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputInitializer : MonoBehaviour {

    [SerializeField]
    public static InputController inputController;
    public static InputController.UIActions uiActionMap;
    public static InputController.RobotBaseActions robotBaseActionMap;
    public static InputController.RobotArmActions robotArmActionMap;


    void Awake() {
        if (inputController == null) {
            inputController = new InputController();
            uiActionMap = inputController.UI;
            robotBaseActionMap = inputController.RobotBase;
            robotArmActionMap = inputController.RobotArm;      
        }
    }
}