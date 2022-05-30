using UnityEngine;
using UnityEngine.UI;

public class JoinCamera : MonoBehaviour
{
    public static JoinCamera instance;

    public Camera joinCamera;
    public float aspectRatio = 1.6f;
    [Space(10)]
    public MaskableGraphic joinCameraUI;
    public Text joinCameraText;
    public Color colorReadyToConnect;
    public Color colorNotReady;

    private void Awake()
    {
        if (instance)
        {
            Debug.Log("Instance of JoinCamera already exists");
            Destroy(this);
        }
        else
        {
            instance = this;
            SetJoinCameraRectByAspectRatio();
        }
    }

    private void SetJoinCameraRectByAspectRatio()
    {
        float x = joinCamera.rect.x;
        float width = joinCamera.rect.width;
        float widthInPixels = (width - x) * Screen.width;
        float height = widthInPixels / (aspectRatio * Screen.height);
        joinCamera.rect = new Rect(x, joinCamera.rect.y, width, height);
        joinCameraUI.rectTransform.anchorMax = new Vector2(width, height);
    }

    public void ChangeActivity(bool value)
    {
        joinCamera.enabled = value;
        NotificationSystem.instance?.ChangeScale(value);
        joinCameraUI.gameObject.SetActive(value);
    }

    public void SetTextValue(string value)
    {
        joinCameraText.text = value;
    }

    public void ChangeReadiness(bool value)
    {
        joinCameraUI.color = value ? colorReadyToConnect : colorNotReady;
    }

    public bool IsEnabled()
    {
        return joinCamera.enabled;
    }
}
