using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NotificationSystem : UiMovement
{
    public static NotificationSystem instance;

    public enum NotificationTypes { message, warning, alert }
    [Header("Система уведомления")]
    public Image iconObject;
    public Text textObject;
    public MaskableGraphic background;
    [Header("Сообщения")]
    public Sprite messageSprite;
    public Color messageForegroundColor;
    public Color messageBackgroundColor;
    [Header("Предупреждения")]
    public Sprite warningSprite;
    public Color warningForegroundColor;
    public Color warningBackgroundColor;
    [Header("Сигналы тревоги")]
    public Sprite alertSprite;
    public Color alertForegroundColor;
    public Color alertBackgroundColor;

    private IEnumerator refreshVisibility;

    protected override void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Instance of NotificationSystem " + " already exists");
            Destroy(this);
            return;
        }
        base.Awake();
    }

    private void Start()
    {
        refreshVisibility = RefreshVisibility();
    }

    public void ChangeScale(bool value)
    {
        if (value)
        {
            background.rectTransform.anchorMax = new Vector3(0.7f, 0f);
        }
        else
        {
            background.rectTransform.anchorMax = new Vector3(1f, 0f);
        }
    }

    public void Notify(NotificationTypes type, string textString)
    {
        switch (type)
        {
            case NotificationTypes.message:
                {
                    SetValues(messageSprite, textString, messageForegroundColor, messageBackgroundColor);
                    break;
                }
            case NotificationTypes.warning:
                {
                    SetValues(warningSprite, textString, warningForegroundColor, warningBackgroundColor);
                    break;
                }
            case NotificationTypes.alert:
                {
                    SetValues(alertSprite, textString, alertForegroundColor, alertBackgroundColor);
                    break;
                }
        }
        StopCoroutine(refreshVisibility);
        refreshVisibility = RefreshVisibility();
        StartCoroutine(refreshVisibility);
    }

    private IEnumerator RefreshVisibility()
    {
        StopCoroutine(activeMovement);
        activeMovement = SmoothMove(GetComponent<RectTransform>().anchoredPosition, newPosition, movingFunction);
        StartCoroutine(activeMovement);
        yield return new WaitForSeconds(3f);
        StopCoroutine(activeMovement);
        activeMovement = SmoothMove(GetComponent<RectTransform>().anchoredPosition, defaultPosition, movingFunction);
        StartCoroutine(activeMovement);
    }

    private void SetValues(Sprite icon, string textString, Color mainColor, Color backgroundColor)
    {
        iconObject.sprite = icon;
        textObject.text = textString;
        iconObject.color = mainColor;
        textObject.color = mainColor;
        background.color = backgroundColor;
    }
}
