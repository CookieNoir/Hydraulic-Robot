using UnityEngine;
using UnityEngine.UI;

public class RestartLevelButtonActivity : MonoBehaviour
{
    [SerializeField] private EducationHandler _educationHandler;
    [SerializeField] private Button _button;

    private void Start()
    {
        _educationHandler.OnLevelStarted += ToggleActivity;
    }

    public void ToggleActivity(bool state)
    {
        _button.interactable = state;
    }
}
