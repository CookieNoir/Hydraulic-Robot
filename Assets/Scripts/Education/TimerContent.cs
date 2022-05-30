using UnityEngine;
using UnityEngine.UI;
public class TimerContent : MonoBehaviour
{
    public MaskableGraphic timerImage;
    public Text timerText;
    public Color defaultColor;
    public Color endColor;
    public Color dangerColor;
    public Color terminateColor;

    public MaskableGraphic penaltyTimeImage;

    private bool isPenaltyTimeActive = false;

    public void UpdateTime(float seconds)
    {
        if (seconds < 0f && !isPenaltyTimeActive)
        {
            isPenaltyTimeActive = true;
            penaltyTimeImage.gameObject.SetActive(isPenaltyTimeActive);
        }
        timerImage.color = Color.Lerp(dangerColor, defaultColor, SimpleFunctions.Smoothstep(Mathf.Clamp01(seconds * 0.2f)));
        int secondsFloor = (int)Mathf.Abs(Mathf.Floor(seconds));
        timerText.text = (secondsFloor / 60).ToString() + ':' + (secondsFloor % 60).ToString("D2");
    }

    public void SetDefaultValues()
    {
        timerImage.color = defaultColor;
        penaltyTimeImage.color = dangerColor;
        isPenaltyTimeActive = false;
        penaltyTimeImage.gameObject.SetActive(isPenaltyTimeActive);
    }

    public void SetEndColor()
    {
        timerImage.color = isPenaltyTimeActive ? dangerColor : endColor;
    }

    public void SetTerminateColor()
    {
        timerImage.color = terminateColor;
        penaltyTimeImage.color = terminateColor;
    }
}
