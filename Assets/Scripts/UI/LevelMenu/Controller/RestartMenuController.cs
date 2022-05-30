using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using View;

public interface IRestartMenuView : IMenuView
{
    event Action ResetEvent;
    event Action BackEvent;
}
public class RestartMenuController : IController<IRestartMenuView>
{
    private IRestartMenuView _view;
    public void OnOpen(IRestartMenuView view)
    {
        _view = view;
        _view.ResetEvent += Reset;
        _view.BackEvent += Back;
    }

    public void OnClose(IRestartMenuView view)
    {
        _view.ResetEvent -= Reset;
        _view.BackEvent -= Back;
        _view = null;
    }

    public void Reset()
    {
        EducationHandler.instance?.ResetTask();
        _view.PauseMenuManager.AllBack();
    }

    public void Back()
    {
        _view.PauseMenuManager.RemoveStackView();
        _view.PauseMenuManager.GetView().Open();
        _view.Close(this);
    }
}
