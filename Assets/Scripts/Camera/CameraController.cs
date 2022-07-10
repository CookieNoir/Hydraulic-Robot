using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour {

    public static CameraController instance;

    private InputController.RobotBaseActions baseAction;
    private float inputValue;

    public Camera targetCamera;
    public KeyCode nextCameraKeyCode = KeyCode.RightBracket;
    public KeyCode previousCameraKeyCode = KeyCode.LeftBracket;
    private int currentCamera;
    private bool isUsingSpecialCamera = false;
    private bool camerasSet = false;

    private Transform regularCamerasContainer;
    private int regularTransformsCount;
    private Transform specialCamerasContainer;
    private int specialTransformsCount;

    public enum SpecialCameras { Class_3_Tasks };

    private void Awake()
    {
        if (instance)
        {
            Debug.Log("Instance of CameraController already exists");
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        //Input system initialization
        baseAction = (new InputController()).RobotBase;
        baseAction.Enable();

        //Debug.Log(string.Format("Assign buttons to game object {0}\nIncrease button: {1}\nDecrease button: {2}", gameObject.name, increaseValueButton, decreaseValueButton));
        baseAction.ChangeCamera.AddCompositeBinding("Axis")
            .With("Positive", string.Format("<Keyboard>/{0}", nextCameraKeyCode))
            .With("Negative", string.Format("<Keyboard>/{0}", previousCameraKeyCode))
            .With("MaxValue", "1")
            .With("MinVlaue", "-1");
        baseAction.ChangeCamera.performed += OnCameraChanged;
    }

    private void OnCameraChanged(InputAction.CallbackContext ctx) {
        if (camerasSet && !isUsingSpecialCamera) {
            inputValue = baseAction.ChangeCamera.ReadValue<float>();

            if (inputValue > 0) {
                currentCamera = (currentCamera + 1) % regularTransformsCount;
                AttachCameraToTransform(regularCamerasContainer.GetChild(currentCamera));
            }
            if (inputValue < 0) {
                currentCamera = (currentCamera - 1 + regularTransformsCount) % regularTransformsCount;
                AttachCameraToTransform(regularCamerasContainer.GetChild(currentCamera));

            }
        }
    }

    public void SetCamerasContainers(RobotController robotController)
    {
        regularCamerasContainer = robotController.regularCamerasContainer;
        regularTransformsCount = regularCamerasContainer.childCount;
        specialCamerasContainer = robotController.specialCamerasContainer;
        specialTransformsCount = specialCamerasContainer.childCount;

        currentCamera = 0;
        AttachCameraToTransform(regularCamerasContainer.GetChild(currentCamera));
        camerasSet = true;
    }

    public void SetSpecialCamera(SpecialCameras specialCamera)
    {
        isUsingSpecialCamera = true;
        AttachCameraToTransform(specialCamerasContainer.GetChild((int)specialCamera));
    }

    public void SetRegularCamera()
    {
        isUsingSpecialCamera = false;
        AttachCameraToTransform(regularCamerasContainer.GetChild(currentCamera));
    }

    public void AttachCameraToTransform(Transform targetTransform)
    {
        targetCamera.transform.SetParent(targetTransform);
        targetCamera.transform.localPosition = Vector3.zero;
        targetCamera.transform.localRotation = Quaternion.identity;
    }
}
