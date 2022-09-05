using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour {

    public static CameraController instance;
    public Camera targetCamera;
    public InputActionReference action;
    private InputAction inAction;
    private float inputValue;
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

        if (action is not null) {
            inAction = action;
            inAction.performed += OnCameraChanged;
            if (!inAction.enabled) inAction.Enable();
        } 
        else Debug.Log(string.Format("No action specified for game object {0}.", gameObject.name));
    }

    private void OnCameraChanged(InputAction.CallbackContext ctx) {
        if (camerasSet && !isUsingSpecialCamera) {
            inputValue = inAction.ReadValue<float>();

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

    public void SetFree()
    {
        targetCamera.transform.parent = null;
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
