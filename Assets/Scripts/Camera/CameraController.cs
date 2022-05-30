using UnityEngine;

public class CameraController : MonoBehaviour {

    public static CameraController instance;

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

    private void Update()
    {
        if (camerasSet && !isUsingSpecialCamera)
        {
            if (Input.GetKeyDown(nextCameraKeyCode))
            {
                currentCamera = (currentCamera + 1) % regularTransformsCount;
                AttachCameraToTransform(regularCamerasContainer.GetChild(currentCamera));
            }
            if (Input.GetKeyDown(previousCameraKeyCode))
            {
                currentCamera = (currentCamera - 1 + regularTransformsCount) % regularTransformsCount;
                AttachCameraToTransform(regularCamerasContainer.GetChild(currentCamera));
            }
        }
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
