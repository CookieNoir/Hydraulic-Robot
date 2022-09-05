using UnityEngine;
using UnityEngine.InputSystem;

public class UiController : MonoBehaviour
{
    [SerializeField] private MainMenuUi _mainMenuUi;
    [SerializeField] private InputActionReference _selectNextAction;
    private InputAction _selectNext;
    [SerializeField] private InputActionReference _selectPreviousAction;
    private InputAction _selectPrevious;
    [SerializeField] private InputActionReference _backAction;
    private InputAction _back;
    [SerializeField] private InputActionReference _pauseAction;
    private InputAction _pause;

    private void Start()
    {
        if (_selectNextAction)
        {
            _selectNext = _selectNextAction;
            if (!_selectNext.enabled) _selectNext.Enable();
            _selectNext.performed += _mainMenuUi.SelectNext;
        }
        if (_selectPreviousAction)
        {
            _selectPrevious = _selectPreviousAction;
            if (!_selectPrevious.enabled) _selectPrevious.Enable();
            _selectPrevious.performed += _mainMenuUi.SelectPrevious;
        }
        if (_backAction)
        {
            _back = _backAction;
            if (!_back.enabled) _back.Enable();
            _back.performed += _mainMenuUi.Back;
        }
        if (_pauseAction)
        {
            _pause = _pauseAction;
            if (!_pause.enabled) _pause.Enable();
            _pause.performed += _mainMenuUi.Pause;
        }
    }
}