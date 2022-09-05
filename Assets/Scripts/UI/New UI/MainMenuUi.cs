using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuUi : MonoBehaviour
{
    [SerializeField] private MenuView _startMenuView;
    [SerializeField] private MenuView _pauseMenuView;
    private MenuView _currentMenuView;
    private Stack<MenuView> _menuViewStack;

    private void Start()
    {
        _menuViewStack = new Stack<MenuView>();
        _SetView(_startMenuView);
    }

    public void SelectNext(InputAction.CallbackContext obj)
    {
        _currentMenuView?.SelectNext();
    }

    public void SelectPrevious(InputAction.CallbackContext obj)
    {
        _currentMenuView?.SelectPrev();
    }

    public void OpenView(MenuView menuView)
    {
        if (_currentMenuView)
        {
            if (_menuViewStack.Contains(menuView))
            {
                while (_menuViewStack.Peek() != menuView) _menuViewStack.Pop();
                _menuViewStack.Pop();
            }
            else
            {
                _menuViewStack.Push(_currentMenuView);
            }
        }

        _SetView(menuView);
    }

    public void Pause(InputAction.CallbackContext obj)
    {
        if (_currentMenuView)
        {
            Back();
        }
        else
        {
            _SetView(_pauseMenuView);
        }
    }

    public void Back(InputAction.CallbackContext obj)
    {
        Back();
    }

    public void Back()
    {
        if (_currentMenuView)
        {
            MenuView prev;
            if (_menuViewStack.TryPop(out prev))
            {
                _SetView(prev);
            }
            else
            {
                Continue();
            }
        }
    }

    public void Continue()
    {
        _menuViewStack.Clear();
        _SetView(null);
    }

    private void _SetView(MenuView menuView)
    {
        _currentMenuView?.Disable();
        _currentMenuView = menuView;
        _currentMenuView?.Enable();
    }
}